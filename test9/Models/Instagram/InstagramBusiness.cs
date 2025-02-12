namespace InstagramDMs.API.Models.Instagram
{
    public class InstagramBusiness
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string InstagramPageId { get; set; }
        public string AccessToken { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public ICollection<InstagramTeamMember> TeamMembers { get; set; }
        public ICollection<InstagramConversation> Conversations { get; set; }
        public ICollection<InstagramContact> Contacts { get; set; }


    }
}
