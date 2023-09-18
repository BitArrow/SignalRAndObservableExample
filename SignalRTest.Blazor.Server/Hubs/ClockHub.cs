using Microsoft.AspNetCore.SignalR;

namespace SignalRTest.Blazor.Server.Hubs
{
    public class ClockHub : Hub
    {
        public async Task TimeChange()
        {
            var dateTime = DateTime.Now;
            await Clients.All.SendAsync("ReceiveTime", dateTime.ToString("u"));
        }
    }
}
