namespace InstagramDMs.API.Models.Instagram
{
    public class InstagramBusinessMedia
    {
        public int Id { get; set; }
        public Guid BusinessId { get; set; }
        public InstagramBusiness Business { get; set; }
        public required string AttachmentId { get; set; }
        public required string NameOnServer { get; set; }
    }
}
