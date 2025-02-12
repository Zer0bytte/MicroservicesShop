
using InstagramDMs.API.Data;
using InstagramDMs.API.Models.Instagram;
using InstagramDMs.API.Vms;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public record GetConversationMessagesQuery(int ConversationId, int Page = 1, int PageSize = 20) : IRequest<IEnumerable<GetConversationMessagesResult>>;
public record GetConversationMessagesResult(InstagramConversationTimeLineEeventType EeventType, InstagramConversationTimeLineEeventSubType EeventSubType,
    DateTime Timestamp, string ActorName, object Payload);


public class GetConversationMessagesQueryHandler(ApplicationDbContext context) : IRequestHandler<GetConversationMessagesQuery, IEnumerable<GetConversationMessagesResult>>
{
    public async Task<IEnumerable<GetConversationMessagesResult>> Handle(GetConversationMessagesQuery request, CancellationToken cancellationToken)
    {
        var query = context.InstagramConversationTimelineEvents
                       .Include(e => e.Actor)
                       .Where(e => e.ConversationId == request.ConversationId)
                       .OrderByDescending(e => e.Timestamp);

        var totalCount = await query.CountAsync();
        var events = await query.Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new GetConversationMessagesResult(e.EventType, e.EventSubtype, e.Timestamp, e.Actor != null ? e.Actor.Name : null,
            MapPayloadBasedOnEventType(e))).ToListAsync();

        events.Reverse();


        return events;
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

            if (payload.AssignedBy.Id == 1)
                payload.AssignedBy.Name = "You";

            if (payload.AssignedTo.Id == 1)
                payload.AssignedTo.Name = "You";

            return payload;

        }

        return null;
    }

}
