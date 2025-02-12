using InstagramDMs.API.Conversations.GetConversations;
using InstagramDMs.API.Data;
using InstagramDMs.API.Models.Instagram;
using InstagramDMs.API.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace InstagramDMs.API.Endpoints
{
    public static class ConversationEndpoints
    {
        public static void MapConversationEndpoints(this WebApplication app)
        {
            app.MapPost("/assign-conversation", AssignConversation);

        }

        private static async Task<IResult> GetConversationMessages(ApplicationDbContext context, int id, InstagramWebhookService service)
        {
            var messages = await service.GetConversationTimeline(id);

            return Results.Ok(messages);
        }

        public static async Task<IResult> GetConversations(ApplicationDbContext context,ISender sender)
        {

            var conversations = await context.InstagramConversations.Include(c => c.Contact).Select(c => new GetConversationsResponse
            {
                Id = c.Id,
                ContactName = c.Contact.Name ?? c.Contact.UserId,
                RecepientId = c.Contact.UserId,
                UnreadCount = c.Messages.Count(m => m.ReadOn == null),
                LastMessageType = c.LastMessageType,
                LastMessageText = c.LastMessageText,
                LastMessageTime = c.LastMessageTimestamp,
                LastReceivedMessageTime = c.LastReceivedMessageTimestamp
            }).ToListAsync();

            return Results.Ok(conversations);
        }

        public static async Task<IResult> AssignConversation(ApplicationDbContext context, ChatAssignmentRequest request)
        {
            InstagramConversation conversation = await context.InstagramConversations.FindAsync(request.ConversationId);
            InstagramTeamMember teamMember = await context.InstagramTeamMembers.FindAsync(request.TeamMemberId);
            if (conversation is not null && teamMember is not null)
            {
                conversation.AssignedTo = teamMember;
                context.InstagramConversationTimelineEvents.Add(new InstagramConversationTimelineEvent
                {
                    Conversation = conversation,
                    EventType = InstagramConversationTimeLineEeventType.ChatAssigned,
                    Payload = JsonConvert.SerializeObject(new ConversationAssignedEvent
                    {
                        AssignedTo = new AssignedTeamMember
                        {
                            Id = teamMember.Id,
                            Name = teamMember.Name
                        },
                        AssignedBy = new AssignedTeamMember
                        {
                            Name = "Admin",
                            Id = 3
                        }
                    }),
                    ActorId = 3
                });
                await context.SaveChangesAsync();

                return Results.Ok();
            }
            return Results.BadRequest();
        }
    }
}
