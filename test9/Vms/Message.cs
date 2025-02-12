using Newtonsoft.Json;

namespace InstagramDMs.API.Vms
{
    public class Message
    {
        public string Mid { get; set; }
        public string? Text { get; set; }
        public List<Attachment>? Attachments { get; set; }
        public bool Is_Echo { get; set; }
    }

    public class InstagramMessageRequest
    {
        public string? Text { get; set; }
        public AttachmentRequest Attachment { get; set; }
    }


    public class InstagarmGenericMessageRequest
    {
        public Recipient Recipient { get; set; }
        public InstagramGenericMessage Message { get; set; }

        public class InstagramGenericMessage
        {
            public InstagramGenericMessageAttachment Attachment { get; set; }
        }

        public class InstagramGenericMessageAttachment
        {
            public string Type { get; set; }
            public InstagramGenericTemplateMessagePayload Payload { get; set; }

        }
        public class InstagramGenericTemplateMessagePayload
        {
            [JsonProperty("template_type")]
            public string TemplateType { get; set; }
            public List<Element> Elements { get; set; }
        }
    }

    public class InstagramButtonTemplateMessageRequest
    {
        public required Recipient Recipient { get; set; }
        public required InstagramButtonTemplateMessage Message { get; set; }


        public class InstagramButtonTemplateMessage
        {
            public InstagramButtonTemplateAttachment Attachment { get; set; }
        }

        public class InstagramButtonTemplateAttachment
        {
            public string type { get; } = "template";
            public InstagramButtonTemplatePayload Payload { get; set; }


        }

        public class InstagramButtonTemplatePayload
        {
            public string template_type { get; } = "button";
            public string Text { get; set; }
            public List<Button> Buttons { get; set; }
        }
    }

}
