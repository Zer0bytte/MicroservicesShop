
using InstagramDMs.API.Models.Instagram;

public class InstagramConversation
{
    public int Id { get; set; }
    public string Status { get; set; } = "OPEN";

    public InstagramContact Contact { get; set; }
    public int ContactId { get; set; }

    public MessageType LastMessageType { get; set; }
    public string? LastMessageText { get; set; }
    public DateTime LastMessageTimestamp { get; set; }
    public DateTime? LastReceivedMessageTimestamp { get; set; }

    public Guid BusinessId { get; set; }
    public InstagramBusiness Business { get; set; }

    public int? AssignedToId { get; set; }
    public InstagramTeamMember AssignedTo { get; set; }
    public ICollection<InstagramMessage> Messages { get; set; }
    public ICollection<InstagramConversationTimelineEvent> TimelineEvents { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

public enum MessageType
{
    Text,
    Media,
    Template
}
