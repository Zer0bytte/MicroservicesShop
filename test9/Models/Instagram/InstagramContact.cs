namespace InstagramDMs.API.Models.Instagram
{
    public class InstagramContact
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string? Name { get; set; }

        public InstagramBusiness Business { get; set; }
        public Guid BusinessId { get; set; }
    }
}
