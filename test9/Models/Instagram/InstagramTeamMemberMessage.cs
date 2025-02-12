namespace InstagramDMs.API.Models.Instagram
{
    public class InstagramTeamMemberMessage
    {
        public int Id { get; set; }
        public int TeamMemberId { get; set; }
        public InstagramTeamMember TeamMember { get; set; }
        public string MessageId { get; set; }
    }
}
