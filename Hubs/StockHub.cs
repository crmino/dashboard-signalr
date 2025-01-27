using Microsoft.AspNetCore.SignalR;

namespace DashboardSignalR.Hubs
{
    public class StockHub : Hub
    {

        public async Task Send(DateTime dateTime, string stockName, string price)
        {
            await Clients.All.SendAsync("Receive", dateTime, stockName, price);
        }
    }
}
