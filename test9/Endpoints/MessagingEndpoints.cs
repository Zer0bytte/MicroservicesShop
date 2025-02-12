using InstagramDMs.API.Commands;
using InstagramDMs.API.Data;
using InstagramDMs.API.Models.Instagram;
using InstagramDMs.API.Services;
using InstagramDMs.API.Vms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace InstagramDMs.API.Endpoints
{
    public static class MessagingEndpoints
    {

        static string instagramApiUrl = "https://graph.instagram.com/v21.0";
        static string pageId = "me";
        static string accessToken = "IGAAQGlrzxgUFBZAE9NMDRUR3k1ZADJKZAVJsQkZARczZAMN1o2S3Q5UTdxS3cyOVZATbEdrcEJxbG1WdGwycEN0QkswWU1McHRoam5pWkFDU25HYlVfZAUVsRGYzYk5aaUpVSFhTM19Pc3M1NTZAkdFhTWHZAjNzJxUmpaSjhtNkhNYnk3ZAwZDZD"; // Replace with your access token

        public static void MapMessagingEndpoints(this WebApplication app)
        {
            app.MapPost("/send-text", SendMessage);
            app.MapPost("/send-media", SendMedia).DisableAntiforgery();
            app.MapPost("/send-template", SendTemplate);
            app.MapPost("/send-seen/{id}", SendSeen).RequireAuthorization();
        }

        private static async Task SendSeen(int id, ApplicationDbContext context)
        {
            context.InstagramMessages.Where(x => x.ConversationId == id).ExecuteUpdate(x => x.SetProperty(p => p.ReadOn, DateTime.UtcNow));
        }

        private static async Task<IResult> SendTemplate(SendTemplateCommand command, ApplicationDbContext context, InstagramService instagramService)
        {
            var template = context.InstagramBusinessTemplates.SingleOrDefault(t => t.Id == command.TemplateId);
            if (template is null) return Results.BadRequest();
            var payloadJson = template.Body;
            payloadJson = payloadJson.Replace("{{RECIPIENT_ID}}", command.RecipientId);
            var response = await instagramService.PostPayload("me/messages", JsonConvert.DeserializeObject(payloadJson));

            if (response.IsSuccessStatusCode)
                return Results.Ok();
            else
                return Results.BadRequest(response.Content.ReadAsStringAsync());
        }

        public static async Task<IResult> SendMedia([FromForm] SendMediaMessageCommand request, InstagramService instagramService, ApplicationDbContext context, MediaSecurityService mediaSecurity,
        IHostEnvironment hostingEnvironment)
        {
            if (string.IsNullOrEmpty(request.RecipientId)) return Results.BadRequest();

            string attachmentId = request.AttachmentId;
            string mediaType = request.MediaType;
            if (request.Media is not null)
            {
                var isMediaValid = mediaSecurity.IsMediaValid(request.Media);
                if (!isMediaValid) return Results.BadRequest("Invalid media");

                string uploads = Path.Combine(hostingEnvironment.ContentRootPath, "uploads");

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.Media.FileName);
                string filePath = Path.Combine(uploads, fileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Media.CopyToAsync(fileStream);
                }

                string mediaUrl = $"https://zerobyte.localto.net/media/{fileName}";

                mediaType = mediaSecurity.GetMediaType(request.Media);
                if (mediaType is null) return Results.BadRequest();

                var mediaUpload = await instagramService.UploadMedia(mediaUrl, mediaType);
                if (mediaUpload is null) return Results.BadRequest();
                var businessMedia = new InstagramBusinessMedia
                {
                    AttachmentId = mediaUpload.AttachmentId,
                    BusinessId = Guid.NewGuid(),
                    NameOnServer = fileName,
                };
                context.InstagramBusinessMedia.Add(businessMedia);
                await context.SaveChangesAsync();
                attachmentId = mediaUpload.AttachmentId;
            }
            else
            {
                if (string.IsNullOrEmpty(request.AttachmentId) || string.IsNullOrEmpty(request.MediaType))
                    return Results.BadRequest();
            }


            var payload = new InstagramSendMessageRequest
            {
                Recipient = new Vms.Recipient
                {
                    Id = request.RecipientId
                },
                Message = new Vms.InstagramMessageRequest
                {
                    Attachment = new Vms.AttachmentRequest
                    {
                        Type = mediaType,
                        Payload = new Vms.Payload
                        {
                            AttachmentId = attachmentId
                        }
                    }
                }
            };
            var response = await instagramService.PostPayload("me/messages", payload);

            return Results.Ok();
        }
        public static async Task<IResult> SendMessage(SendTextMessageCommand request, ApplicationDbContext context)
        {
            if (string.IsNullOrEmpty(request.RecipientId) || string.IsNullOrEmpty(request.MessageText))
            {
                return Results.BadRequest("Recipient ID and either Message Text or Media URL are required.");
            }
            var payload = new InstagramSendMessageRequest
            {
                Recipient = new Recipient { Id = request.RecipientId },
                Message = new InstagramMessageRequest
                {
                    Text = request.MessageText
                }
            };
            using var httpClient = new HttpClient();
            var url = $"{instagramApiUrl}/{pageId}/messages?access_token={accessToken}";

            try
            {
                var jsonPayload = JsonConvert.SerializeObject(payload);

                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);

                response.EnsureSuccessStatusCode();

                var responseData = JsonConvert.DeserializeObject<InstagramSendMessageResponse>(await response.Content.ReadAsStringAsync());
                var temMemberMessage = new InstagramTeamMemberMessage
                {
                    MessageId = responseData.MessageId,
                    TeamMemberId = 1
                };
                context.InstagramTeamMemberMessages.Add(temMemberMessage);
                await context.SaveChangesAsync();

                return Results.Ok(responseData);
            }
            catch (HttpRequestException ex)
            {
                return Results.Problem($"Failed to send message: {ex.Message}");
            }
        }

    }
}
