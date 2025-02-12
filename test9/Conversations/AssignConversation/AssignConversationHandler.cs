using InstagramDMs.API.Data;
using InstagramDMs.API.Models.Instagram;
using MediatR;
using Newtonsoft.Json;

namespace InstagramDMs.API.Conversations.AssignConversation;
public record AssignConversationCommand(int ConversationId, int TeamMemberId) : IRequest<AssignConversationResult>;
public record AssignConversationResult(bool IsSuccess);

public class AssignConversationCommandHandler(ApplicationDbContext context) : IRequestHandler<AssignConversationCommand, AssignConversationResult>
{
    public async Task<AssignConversationResult> Handle(AssignConversationCommand request, CancellationToken cancellationToken)
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
            return new AssignConversationResult(true);
        }
        return new AssignConversationResult(false);

    }
}
