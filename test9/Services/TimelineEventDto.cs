using InstagramDMs.API.Models.Instagram;
using System.Text.Json.Serialization;

namespace InstagramDMs.API.Services
{
    public class TimelineEventDto
    {

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public InstagramConversationTimeLineEeventType EventType { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public InstagramConversationTimeLineEeventSubType EventSubType { get; set; }
        public DateTime Timestamp { get; set; }
        public string ActorName { get; set; }
        public object Payload { get; set; }
        public int ConversationId { get; set; }
    }
}