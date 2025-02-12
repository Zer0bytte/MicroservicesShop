using InstagramDMs.API.Data;
using InstagramDMs.API.Hubs.IGHubs;
using InstagramDMs.API.Models.Instagram;
using InstagramDMs.API.Vms;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace InstagramDMs.API.Services
{
    public class InstagramWebhookService(ApplicationDbContext context, IHubContext<InstagramHub> hubContext)
    {

        public async Task ProcessEntry(Entry entry)
        {
            if (entry.Messaging != null && entry.Messaging.Any())
            {
                foreach (var messaging in entry.Messaging)
                {
                    if (messaging.Message != null)
                    {
                        await ProcessMessagingEventAsync(messaging, entry.Id);
                    }
                }
            }
            else if (entry.Changes != null && entry.Changes.Any())
            {
                foreach (var change in entry.Changes)
                {
                    if (change.Field == "comments")
                    {
                        await ProcessCommentEventAsync(change, entry.Id);
                    }

                }

            }
            else return;


        }

        private async Task ProcessCommentEventAsync(Change change, string id)
        {
            await Task.Delay(1);
        }

        public async Task ProcessMessagingEventAsync(Messaging messaging, string entryId)
        {
            string contactId = messaging.Sender.Id;
            string businessId = entryId;
            InstagramTeamMember? actor = null;
            if (messaging.Message.Is_Echo)
            {
                actor = context.InstagramTeamMemberMessages.Include(x => x.TeamMember).FirstOrDefault(x => x.MessageId == messaging.Message.Mid)?.TeamMember;
                contactId = messaging.Recipient.Id;
            }

            var conversation = await FindOrCreateConversationAsync(contactId, businessId);

            if (!messaging.Message.Is_Echo)
                conversation!.LastReceivedMessageTimestamp = DateTime.UtcNow;

            if (messaging.Message.Text != null)
            {
                conversation!.LastMessageType = MessageType.Text;
                conversation.LastMessageText = messaging.Message.Text;
            }
            else if (messaging.Message.Attachments != null)
            {
                if (messaging.Message.Attachments[0].Type == "image")
                {
                    conversation!.LastMessageType = MessageType.Media;
                    conversation.LastMessageText = "";
                }
            }

            var timelineEvent = new InstagramConversationTimelineEvent
            {
                Conversation = conversation!,
                EventType = InstagramConversationTimeLineEeventType.Message,
                EventSubtype = messaging.Message.Is_Echo ? InstagramConversationTimeLineEeventSubType.MessageSent : InstagramConversationTimeLineEeventSubType.MessageReceived,
                Timestamp = DateTime.UtcNow,
                Payload = JsonConvert.SerializeObject(messaging.Message),
                Actor = actor
            };

            var message = new InstagramMessage
            {
                MessageId = messaging.Message.Mid,
                Conversation = conversation,
                Contact = conversation.Contact,
                TimelineEvent = timelineEvent,
                IsIncoming = !messaging.Message.Is_Echo
            };

            context.InstagramConversationTimelineEvents.Add(timelineEvent);
            var timelineEventDto = new TimelineEventDto
            {
                ConversationId = conversation.Id,
                EventType = timelineEvent.EventType,
                EventSubType = timelineEvent.EventSubtype,
                Timestamp = timelineEvent.Timestamp,
                ActorName = timelineEvent.Actor != null ? timelineEvent.Actor.Name : null,
                Payload = MapPayloadBasedOnEventType(timelineEvent)
            };

            await hubContext.Clients.All.SendAsync("NewMessage", timelineEventDto);
            context.InstagramMessages.Add(message);
            await context.SaveChangesAsync();


        }

        private async Task<InstagramConversation?> FindOrCreateConversationAsync(string instagramUserId, string businessId)
        {
            var business = context.InstagramBusinesses.FirstOrDefault(b => b.InstagramPageId == businessId);
            if (business is null) return null;

            var contact = context.InstagramContacts.FirstOrDefault(c => c.UserId == instagramUserId && c.BusinessId == business.Id);
            if (contact is null)
            {
                contact = new InstagramContact
                {
                    Business = business,
                    UserId = instagramUserId
                };
            }

            var conversation = await context.InstagramConversations.SingleOrDefaultAsync(c => c.ContactId == contact.Id);
            if (conversation is null)
            {
                conversation = new InstagramConversation
                {
                    Contact = contact,
                    Business = business
                };
            }

            conversation.LastMessageTimestamp = DateTime.UtcNow;
            return conversation;
        }
        public static int CurrentUser { get; set; } = 1;
        public async Task<TimelineResponseDto> GetConversationTimeline(int conversationId, int page = 1, int pageSize = 20)
        {
            var query = context.InstagramConversationTimelineEvents
                .Include(e => e.Actor)
                .Where(e => e.ConversationId == conversationId)
                .OrderByDescending(e => e.Timestamp);

            var totalCount = await query.CountAsync();
            var events = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new TimelineEventDto
                {
                    EventType = e.EventType,
                    EventSubType = e.EventSubtype,
                    Timestamp = e.Timestamp,
                    ActorName = e.Actor != null ? e.Actor.Name : null,
                    Payload = MapPayloadBasedOnEventType(e)
                })
                .ToListAsync();

            events.Reverse();
            return new TimelineResponseDto
            {
                Events = events,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        private static object MapPayloadBasedOnEventType(InstagramConversationTimelineEvent e)
        {
            if (e.EventType == InstagramConversationTimeLineEeventType.Message)
            {
                return JsonConvert.DeserializeObject<Message>(e.Payload)!;
            }
            else if (e.EventType == InstagramConversationTimeLineEeventType.ChatAssigned)
            {
                var payload = JsonConvert.DeserializeObject<ConversationAssignedEvent>(e.Payload)!;

                if (payload.AssignedBy.Id == CurrentUser)
                    payload.AssignedBy.Name = "You";

                if (payload.AssignedTo.Id == CurrentUser)
                    payload.AssignedTo.Name = "You";

                return payload;

            }

            return null;
        }



        public async Task<List<GetConversationsResponse>> GetConversations()
        {
            var conversations = await context.InstagramConversations.Include(c => c.Contact).Select(c => new GetConversationsResponse
            {
                Id = c.Id,
                ContactName = c.Contact.Name ?? c.Contact.UserId,
                RecepientId = c.Contact.UserId
            }).ToListAsync();

            return conversations;
        }

    }

    public class GetConversationsResponse
    {
        public int Id { get; set; }
        public string? ContactName { get; set; }
        public required string RecepientId { get; set; }
        public int UnreadCount { get; set; }

        [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
        public MessageType LastMessageType { get; set; }
        public string? LastMessageText { get; set; }
        public DateTime LastMessageTime { get; set; }
        public DateTime? LastReceivedMessageTime { get; set; }

    }
}