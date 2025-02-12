using Microsoft.AspNetCore.SignalR;

namespace InstagramDMs.API.Hubs.IGHubs
{
    public class InstagramHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }


    }
}
