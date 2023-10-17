using Microsoft.AspNetCore.SignalR;

namespace webapi.Models
{
    public class CommunicationHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
