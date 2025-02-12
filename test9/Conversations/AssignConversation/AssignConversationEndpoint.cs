using Carter;
using Mapster;
using MediatR;

namespace InstagramDMs.API.Conversations.AssignConversation;
public record AssignConversationRequest(int ConversationId, int TeamMemberId);
public record AssignConversationResponse(bool IsSuccess);
public class AssignConversationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/assign-conversation", async (AssignConversationRequest request, ISender sender) =>
        {
            var result = await sender.Send(request.Adapt<AssignConversationCommand>());
            var response = result.Adapt<AssignConversationResponse>();
            if (response.IsSuccess)
                return Results.Ok(response);

            return Results.BadRequest();
        });
    }
}
