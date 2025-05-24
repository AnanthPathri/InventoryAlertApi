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
            var alertMessages = new StringBuilder();
            var grouped = stockBatches.GroupBy(b => b.PRODUCT_ID);
            foreach (var group in grouped)
            {
                var product = group.First().PRODUCTS;
                var totalQuantity = group.Sum(g => g.REMAINING_QTY);
                if (totalQuantity <= 0)
                {
                    alertMessages.AppendLine($"{product.NAME} is OUT of STOCK across all Warehouses");
                }
            }
            if (alertMessages.Length > 0)
            {
               
                //smsService.Send(alert);
                await _emailService.SendAsync("OUTOFSTOCK", alertMessages.ToString());
            }
        }
        private async Task GetExpiredAlerts(List<STOCKBATCHES> stockBatches)
        {
            var alertMessages = new StringBuilder();
            //var grouped = stockBatches.GroupBy(b => b.PRODUCT_ID);
            foreach (var batch in stockBatches.Where(b=>b.EXPIRY_DATE<=DateTime.Today))
            {
                var product = batch.PRODUCTS;
                alertMessages.AppendLine($"{product.NAME} from BATCH {batch.BATCH_ID} has EXPIRED");
            }
            if (alertMessages.Length > 0)
            {
                
                //smsService.Send(alert);
                await _emailService.SendAsync("EXPIRED", alertMessages.ToString());
            }
        }
        private async Task GetExpiringSoonAlerts(List<STOCKBATCHES> stockBatches)
        {
            var alertMessages = new StringBuilder();
            foreach (var batch in stockBatches.Where(b=>b.EXPIRY_DATE>DateTime.Today&&(b.EXPIRY_DATE-DateTime.Today).TotalDays<=10))
            {
                var product = batch.PRODUCTS;
                var days = (batch.EXPIRY_DATE - DateTime.Today).Days;
                alertMessages.AppendLine($"{product.NAME} from BATCH {batch.BATCH_ID} will EXPIRE in {days} days");
            }
            if (alertMessages.Length > 0)
            {
                
                //smsService.Send(alert);
                await _emailService.SendAsync("EXPIRYALERT", alertMessages.ToString());
            }
        }
        private async Task GetOverStockAlerts(List<STOCKBATCHES> stockBatches)
        {
            var alertMessages = new StringBuilder();
            var grouped = stockBatches.GroupBy(b => b.PRODUCT_ID);
            foreach (var group in grouped)
            {
                var product = group.First().PRODUCTS;
                var totalQuantity = group.Sum(g=>g.REMAINING_QTY);
                if (totalQuantity > product.MAX_THRESHOLD)
                {
                    alertMessages.AppendLine($"{product.NAME} is OVERSTOCKED across all warehouses of total ({totalQuantity})");
                } 
            }
            if (alertMessages.Length > 0)
            {
                
                //smsService.Send(alert);
                await _emailService.SendAsync("OVERSTOCK", alertMessages.ToString());
            }
        }
        private async Task GetLowStockAlerts(List<STOCKBATCHES> stockBatches)
        {
            var alertMessages = new StringBuilder();
            var grouped = stockBatches.GroupBy(b => b.PRODUCT_ID);
            foreach (var group in grouped)
            {
                var product = group.First().PRODUCTS;
                var totalQuantity = group.Sum(g => g.REMAINING_QTY);
                if (totalQuantity < product.MIN_THRESHOLD)
                {
                    alertMessages.AppendLine($"{product.NAME} has LOWSTOCK across all warehouses ({totalQuantity}) days");
                }
            }
            if (alertMessages.Length > 0)
            {
                
                //smsService.Send(alert);
                await _emailService.SendAsync("LOWSTOCK", alertMessages.ToString());
            }
        }
    }
}
