using InstagramDMs.API.Vms;

namespace InstagramDMs.API.Commands
{
    public class CreateGenericTemplateCommand
    {
        public List<Element> Elements { get; set; }
        public string Name { get; set; }
    }
}
