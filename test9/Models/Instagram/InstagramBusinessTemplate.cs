namespace InstagramDMs.API.Models.Instagram
{
    public class InstagramBusinessTemplate
    {
        public int Id { get; set; }
        public Guid BusinessId { get; set; }
        public InstagramBusiness Business { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }

    }
}
