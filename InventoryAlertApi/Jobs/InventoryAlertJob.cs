using Quartz;
using InventoryAlertApi.Data;
using InventoryAlertApi.Models;
using InventoryAlertApi.Services;
using Microsoft.EntityFrameworkCore;

namespace InventoryAlertApi.Jobs
{
    public class InventoryAlertJob : IJob
    {
        private readonly InventoryDbContext _context;
        private readonly EmailService _emailService = new();
        //private readonly SmsService smsService = new();
        public InventoryAlertJob(InventoryDbContext context,EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var inventory = await _context.Products.ToListAsync();

            foreach (var item in inventory)
            {
                string? alert = null;
                if (item.quantity <= 0)
                    alert = $"{item.name} is OUT OF STOCK.";
                else if (item.expiryDate <= DateTime.Today)
                    alert = $"{item.name} has EXPIRED.";
                else if (item.quantity > item.overStockThreshold)
                    alert = $"{item.name} is OVERSTOCKED ({item.quantity} units).";
                if (alert != null)
                {
                    //smsService.Send(alert);
                    await _emailService.SendAsync("Inventory Alert", alert);
                }
            }
        }
    }
}
