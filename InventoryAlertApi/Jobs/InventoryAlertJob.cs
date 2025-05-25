using Quartz;
using InventoryAlertApi.Data;
using InventoryAlertApi.Models;
using InventoryAlertApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace InventoryAlertApi.Jobs
{
    public class InventoryAlertJob : IJob
    {
        private readonly InventoryDbContext _context;
        private readonly EmailService _emailService;
        private string message;
        //private readonly SmsService smsService = new();
        public InventoryAlertJob(InventoryDbContext context,EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //var products = await _context.PRODUCTS.ToListAsync();
            var stockBatches = await _context.STOCKBATCHES.Include(sb => sb.PRODUCTS).Include(sb => sb.WAREHOUSES).ToListAsync();
            await GetOutOfStockAlerts(stockBatches);
            await GetExpiredAlerts(stockBatches);
            await GetExpiringSoonAlerts(stockBatches);
            await GetOverStockAlerts(stockBatches);
            await GetLowStockAlerts(stockBatches);
        }
        private async Task GetOutOfStockAlerts(List<STOCKBATCHES> stockBatches)
        {
            var alertMessages=new List<string>();
            var grouped = stockBatches.GroupBy(b => b.PRODUCT_ID);
            foreach (var group in grouped)
            {
                var product = group.First().PRODUCTS;
                var totalQuantity = group.Sum(g => g.REMAINING_QTY);
                if (totalQuantity <= 0)
                {
                    alertMessages.Add($"{product.NAME} is OUT of STOCK across all Warehouses");
                }
            }
            await SendGroupedAlerts("OUTOFSTOCK", alertMessages);
        }
        private async Task GetExpiredAlerts(List<STOCKBATCHES> stockBatches)
        {
            var alertMessages = new List<string>();
            foreach (var batch in stockBatches.Where(b=>b.EXPIRY_DATE<=DateTime.Today))
            {
                var product = batch.PRODUCTS;
                alertMessages.Add($"{product.NAME} from BATCH {batch.BATCH_ID} has EXPIRED");
            }
            await SendGroupedAlerts("EXPIRED", alertMessages);
        }
        private async Task GetExpiringSoonAlerts(List<STOCKBATCHES> stockBatches)
        {
            var alertMessages = new List<string>();
            foreach (var batch in stockBatches.Where(b=>b.EXPIRY_DATE>DateTime.Today&&(b.EXPIRY_DATE-DateTime.Today).TotalDays<=10))
            {
                var product = batch.PRODUCTS;
                var days = (batch.EXPIRY_DATE - DateTime.Today).Days;
                alertMessages.Add($"{product.NAME} from BATCH {batch.BATCH_ID} willl EXPIRE in {days} days");
            }
            await SendGroupedAlerts("EXPIRYALERT", alertMessages);
        }
        private async Task GetOverStockAlerts(List<STOCKBATCHES> stockBatches)
        {
            var alertMessages = new List<string>();
            var grouped = stockBatches.GroupBy(b => b.PRODUCT_ID);
            foreach (var group in grouped)
            {
                var product = group.First().PRODUCTS;
                var totalQuantity = group.Sum(g=>g.REMAINING_QTY);
                if (totalQuantity > product.MAX_THRESHOLD)
                {
                    alertMessages.Add($"{product.NAME} is OVERSTOCKED across all warehouses of total ({totalQuantity})");
                } 
            }
            await SendGroupedAlerts("OVERSTOCK", alertMessages);
        }
        private async Task GetLowStockAlerts(List<STOCKBATCHES> stockBatches)
        {
            var alertMessages = new List<string>();
            var grouped = stockBatches.GroupBy(b => b.PRODUCT_ID);
            foreach (var group in grouped)
            {
                var product = group.First().PRODUCTS;
                var totalQuantity = group.Sum(g => g.REMAINING_QTY);
                if (totalQuantity < product.MIN_THRESHOLD)
                {
                    alertMessages.Add($"{product.NAME} has LOWSTOCK across all warehouses (Qty: {totalQuantity})");
                }
            }
            await SendGroupedAlerts("LOWSTOCK", alertMessages);
        }

        private async Task SendGroupedAlerts(string ruleType, List<string> messages)
        {
            var newMessage = new List<string>();
            foreach (var message in messages)
            {
                bool alreadySent = await _context.NOTIFICATIONS.AnyAsync(n => n.ALERTRULETYPE == ruleType &&
                                                                      n.MESSAGE == message &&
                                                                      n.CHANNEL == "email" &&
                                                                      n.SENTAT > DateTime.Now.AddHours(-24));
                if (!alreadySent)
                {
                    newMessage.Add(message);
                    _context.NOTIFICATIONS.Add(new NOTIFICATIONS
                    {
                        ALERTRULETYPE = ruleType,
                        MESSAGE = message,
                        CHANNEL = "EMAIL",
                        SENTAT = DateTime.Now,
                        STATUS = "SENT"
                    });
                }
            }
            if (newMessage.Any())
            {
                string emailBody = string.Join(Environment.NewLine, newMessage);
                await _emailService.SendAsync(ruleType, emailBody);
                await _context.SaveChangesAsync();
            }
        }
    }
}
