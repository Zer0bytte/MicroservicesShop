using Carter;
using InstagramDMs.API.Models.Instagram;
using Mapster;
using MediatR;

namespace InstagramDMs.API.Conversations.GetConversationMessages;
public record GetConversationMessagesResponse(InstagramConversationTimeLineEeventType EeventType, InstagramConversationTimeLineEeventSubType EeventSubType,
    DateTime Timestamp, string ActorName, object Payload);


public class GetConversationMessagesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("get-conversation-messages/{id}", async (int id, ISender sender) =>
        {
            var result = await sender.Send(new GetConversationMessagesQuery(id));
            var response = result.Adapt<IEnumerable<GetConversationMessagesResponse>>();

            return Results.Ok(response);
        });
    }
}
