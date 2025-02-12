
using InstagramDMs.API.Commands;
using InstagramDMs.API.Data;
using InstagramDMs.API.Models.Instagram;
using InstagramDMs.API.Vms;
using Newtonsoft.Json;

namespace InstagramDMs.API.Endpoints
{
    public static class TemplateEndpoints
    {
        public static void MapTemplateEndpoints(this WebApplication app)
        {
            app.MapGet("/get-templates", GetTemplates);
            app.MapPost("/create-generic", CreateGenericTemplate);
            app.MapPost("/create-button", CreateButtonTemplate);
        }

        private static async Task<IResult> GetTemplates(ApplicationDbContext context)
        {
            var template = context.InstagramBusinessTemplates.Where(t => t.BusinessId == Guid.NewGuid()).ToList();
            return Results.Ok(template);
        }

        private static async Task<IResult> CreateButtonTemplate(CreateButtonTemplateCommand command, ApplicationDbContext context)
        {
            var buttonTemplate = new InstagramButtonTemplateMessageRequest
            {
                Message = new InstagramButtonTemplateMessageRequest.InstagramButtonTemplateMessage
                {
                    Attachment = new InstagramButtonTemplateMessageRequest.InstagramButtonTemplateAttachment
                    {
                        Payload = new InstagramButtonTemplateMessageRequest.InstagramButtonTemplatePayload
                        {
                            Text = command.BodyText,
                            Buttons = command.Buttons
                        }
                    }
                },
                Recipient = new Recipient
                {
                    Id = "{{RECIPIENT_ID}}"
                }
            };
            var json = JsonConvert.SerializeObject(buttonTemplate);

            var template = new InstagramBusinessTemplate
            {
                Body = json,
                BusinessId = Guid.NewGuid(),
                Name = command.TemplateName,
            };
            context.InstagramBusinessTemplates.Add(template);
            await context.SaveChangesAsync();

            return Results.Ok(json);
        }

        private static async Task<IResult> CreateGenericTemplate(CreateGenericTemplateCommand command, ApplicationDbContext context)
        {
            var genericTemplate = new InstagarmGenericMessageRequest
            {
                Recipient = new Recipient { Id = "{{RECIPIENT_ID}}" },
                Message = new InstagarmGenericMessageRequest.InstagramGenericMessage
                {
                    Attachment = new InstagarmGenericMessageRequest.InstagramGenericMessageAttachment
                    {
                        Type = "template",
                        Payload = new InstagarmGenericMessageRequest.InstagramGenericTemplateMessagePayload
                        {
                            Elements = command.Elements,
                            TemplateType = "generic"
                        }
                    }
                }
            };

            var json = JsonConvert.SerializeObject(genericTemplate);

            var template = new InstagramBusinessTemplate
            {
                Body = json,
                BusinessId = Guid.NewGuid(),
                Name = command.Name,
            };
            context.InstagramBusinessTemplates.Add(template);
            await context.SaveChangesAsync();

            return Results.Ok(json);
        }
    }
}
