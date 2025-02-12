using Carter;
using MediatR;

namespace InstagramDMs.API.Conversations.GetConversations;

public class GetConversationsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/get-conversations", async (ISender sender) =>
        {
            var result = await sender.Send(new GetConversationsQuery());

            return Results.Ok(result);
        }).RequireAuthorization();
    }
}
