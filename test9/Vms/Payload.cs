using Newtonsoft.Json;

namespace InstagramDMs.API.Vms
{
    public class Payload
    {
        public string Url { get; set; }
        public Generic Generic { get; set; }

        [JsonProperty("attachment_id")]
        public string AttachmentId { get; set; }

    }
}
