using Quartz;
using InventoryAlertApi.Models;
using InventoryAlertApi.Services;

namespace InventoryAlertApi.Jobs;

public class InventoryAlertJob : IJob
{
    private readonly EmailService emailService = new();
    //private readonly SmsService smsService = new();
    public Task Execute(IJobExecutionContext context)
    {
        var inventory = new List<InventoryItem>
        {
            new InventoryItem{name = "Milk",quantity=0,expiryDate=DateTime.Today},
            new InventoryItem{name = "Bread",quantity=5,expiryDate=DateTime.Today.AddDays(-1)},
            new InventoryItem{name = "Cookies",quantity=100,expiryDate=DateTime.Today.AddMonths(6)}
        };
        foreach (var item in inventory)
        {
            string? alert = null;
            if (item.quantity <= 0)
                alert = $"{item.name} is OUT OF STOCK.";
            else if(item.expiryDate<=DateTime.Today)
                alert = $"{item.name} has EXPIRED.";
            else if(item.quantity>item.overStockThreshold) 
                alert = $"{item.name} is OVERSTOCKED ({item.quantity} units).";
            if (alert != null)
            {
                //smsService.Send(alert);
                emailService.SendAsync("Inventory Alert", alert).Wait();
            }
        }
        return Task.CompletedTask;
    }
}
