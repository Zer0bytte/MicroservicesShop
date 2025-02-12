using InstagramDMs.API.Vms;

namespace InstagramDMs.API.Commands
{
    public class CreateButtonTemplateCommand
    {
        public string BodyText { get; set; }
        public List<Button> Buttons { get; set; }
        public string TemplateName { get; set; }

    }
}
