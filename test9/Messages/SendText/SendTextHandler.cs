using Azure.Core;
using InstagramDMs.API.Data;
using InstagramDMs.API.Models.Instagram;
using InstagramDMs.API.Services;
using InstagramDMs.API.Vms;
using MediatR;
using Newtonsoft.Json;
using System.Text;

namespace InstagramDMs.API.Messages.SendText;
public record SendTextCommand(string RecipientId, string MessageText) : IRequest<SendTextResult>;
public record SendTextResult(string MessageId);

public class SendTextHandler(ApplicationDbContext context,CurrentUser currentUser) : IRequestHandler<SendTextCommand, SendTextResult>
{
    public async Task<SendTextResult> Handle(SendTextCommand request, CancellationToken cancellationToken)
    {
        var payload = new InstagramSendMessageRequest
        {
            Recipient = new Recipient { Id = request.RecipientId },
            Message = new InstagramMessageRequest
            {
                Text = request.MessageText
            }
        };
        var user = currentUser.BusinessId;
        using var httpClient = new HttpClient();
      //  var url = $"{instagramApiUrl}/{pageId}/messages?access_token={accessToken}";
        var url = $"";

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

            return new SendTextResult(responseData.MessageId);
        }
        catch (HttpRequestException ex)
        {
            return null;
        }
    }
}
