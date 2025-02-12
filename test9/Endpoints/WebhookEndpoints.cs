using InstagramDMs.API.Data;
using InstagramDMs.API.Services;
using InstagramDMs.API.Vms;
using Newtonsoft.Json;

namespace InstagramDMs.API.Endpoints
{
    public static class WebhookEndpoints
    {
        public static void MapWebhookEndpoints(this WebApplication app)
        {
            app.MapPost("/webhooks/postData", WebhookReceived);
        }

        public static async Task<IResult> WebhookReceived(HttpContext context, InstagramWebhookService service, ApplicationDbContext _context)
        {

            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            var payload = JsonConvert.DeserializeObject<InstagramWebhookPayload>(body);

            if (payload == null || payload.Entry == null || !payload.Entry.Any())
            {
                await Results.BadRequest("Invalid payload.").ExecuteAsync(context);
            }

            foreach (var entry in payload.Entry)
            {
                await service.ProcessEntry(entry);
            }

            await _context.SaveChangesAsync();
            return Results.Ok();


        }
    }


}
