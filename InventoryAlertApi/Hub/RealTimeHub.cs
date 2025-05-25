
using InventoryAlertApi.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace InventoryAlertApi.RealHub
{
    public class RealTimeHub : Hub
    {
        public async Task SendMessage(DashboardDTO dashboardData)
        {
            await Clients.All.SendAsync("ReceiveMessage", dashboardData);
        }
    }
}
