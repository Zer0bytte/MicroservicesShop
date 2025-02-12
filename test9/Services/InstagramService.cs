using InstagramDMs.API.Vms;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace InstagramDMs.API.Services
{
    public class InstagramService(IHttpClientFactory httpClientFactory)
    {

        public async Task<UploadMediaResponse> UploadMedia(string url, string type)
        {
            var payload = new UploadMediaRequest
            {
                Message = new InstagramMessageRequest
                {
                    Attachment = new AttachmentRequest
                    {
                        Type = type,
                        Payload = new Payload
                        {
                            Url = url
                        }
                    }
                }
            };
            var response = await PostPayload("me/message_attachments", payload);
            var responseData = JsonConvert.DeserializeObject<UploadMediaResponse>(await response.Content.ReadAsStringAsync());

            return new UploadMediaResponse() { AttachmentId = responseData.AttachmentId };
        }

        public async Task<HttpResponseMessage> PostPayload(string url, object payload)
        {
            var client = httpClientFactory.CreateClient("Instagram");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer IGAAQGlrzxgUFBZAE9NMDRUR3k1ZADJKZAVJsQkZARczZAMN1o2S3Q5UTdxS3cyOVZATbEdrcEJxbG1WdGwycEN0QkswWU1McHRoam5pWkFDU25HYlVfZAUVsRGYzYk5aaUpVSFhTM19Pc3M1NTZAkdFhTWHZAjNzJxUmpaSjhtNkhNYnk3ZAwZDZD");
            var jsonPayload = JsonConvert.SerializeObject(payload, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            return await client.PostAsync(url, content);
        }
    }

    public class UploadMediaResponse
    {
        [JsonProperty("attachment_id")]
        public required string AttachmentId { get; set; }

    }

    public class UploadMediaRequest
    {
        public InstagramMessageRequest Message { get; set; }
    }

}
