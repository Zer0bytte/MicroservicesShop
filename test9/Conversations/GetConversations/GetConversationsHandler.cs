using InstagramDMs.API.Data;
using InstagramDMs.API.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InstagramDMs.API.Conversations.GetConversations;
public record GetConversationsQuery() : IRequest<List<GetConversationsResult>>;
public record GetConversationsResult(int Id, string ContactName, string ContactId
    , int UnreadCount, MessageType LastMessageType,
    string LastMessageText, DateTime LastMessageTime, DateTime? LastReceivedMessageTime);

public class GetConversationsQueryHandler(ApplicationDbContext context,CurrentUser currentUser) : IRequestHandler<GetConversationsQuery, List<GetConversationsResult>>
{
    public async Task<List<GetConversationsResult>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
    {
        var rer = currentUser.BusinessId;
        var conversations = await context.InstagramConversations.Include(c => c.Contact).Select(c => new GetConversationsResult
        (
            c.Id,
        c.Contact.Name ?? c.Contact.UserId, c.Contact.UserId, c.Messages.Count(m => m.ReadOn == null), c.LastMessageType, c.LastMessageText, c.LastMessageTimestamp, c.LastReceivedMessageTimestamp)
        ).ToListAsync();


        return conversations;

    }
}
