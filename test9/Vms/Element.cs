using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InstagramDMs.API.Vms
{
    public class Element
    {
        [Required]
        public required string Title { get; set; }

        [JsonProperty("image_url")]
        [JsonPropertyName("image_url")]
        public string? Image_Url { get; set; }
        public string? Subtitle { get; set; }

        [JsonProperty("default_action")]
        [JsonPropertyName("default_action")]
        public DefaultAction? Default_Action { get; set; }
        public List<Button>? Buttons { get; set; } = null;
    }
}
