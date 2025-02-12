using InstagramDMs.API.Models.Instagram;

public class InstagramMessage
{
    public int Id { get; set; }
    public int ContactId { get; set; }
    public InstagramContact Contact { get; set; }
    public string MessageId { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    public bool IsIncoming { get; set; }
    public DateTime? ReadOn { get; set; }
    public InstagramMessageType Type { get; set; }
    public int ConversationId { get; set; }
    public InstagramConversation Conversation { get; set; }
    public int? TimelineEventId { get; set; }
    public InstagramConversationTimelineEvent TimelineEvent { get; set; }
 
}
public enum InstagramMessageType
{
    Text,
    Image,
    Video,
    Template
}