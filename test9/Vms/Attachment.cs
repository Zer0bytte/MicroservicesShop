namespace InstagramDMs.API.Vms
{
    public class Attachment
    {
        public string Type { get; set; }
        public Payload Payload { get; set; }
    }

    public class AttachmentRequest
    {
        public string Type { get; set; }
        public Payload Payload { get; set; }
    }
}
