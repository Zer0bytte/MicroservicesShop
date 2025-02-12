namespace InstagramDMs.API.Commands
{
    public class SendTemplateCommand
    {
        public string RecipientId { get; set; }
        public int TemplateId { get; set; }
    }
}
