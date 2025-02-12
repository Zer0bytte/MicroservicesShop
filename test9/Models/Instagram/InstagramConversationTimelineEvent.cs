namespace InstagramDMs.API.Models.Instagram
{
    public class InstagramConversationTimelineEvent
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public InstagramConversationTimeLineEeventType EventType { get; set; } // message, assignment, status_change
        public InstagramConversationTimeLineEeventSubType EventSubtype { get; set; } // message, assignment, status_change
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public int? ActorId { get; set; }
        public string Payload { get; set; }
        public InstagramTeamMember Actor { get; set; }
        public InstagramConversation Conversation { get; set; }
    }
    public enum InstagramConversationTimeLineEeventType
    {
        Message,
        ChatAssigned
    }
    public enum InstagramConversationTimeLineEeventSubType
    {
        None,
        MessageReceived,
        MessageSent,
    }
}
