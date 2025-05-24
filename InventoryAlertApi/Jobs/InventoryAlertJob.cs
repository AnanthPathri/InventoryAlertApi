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
        private readonly EmailService _emailService = new();
        //private readonly SmsService smsService = new();
        public InventoryAlertJob(InventoryDbContext context,EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var alertMessages=new StringBuilder();
            //var products = await _context.PRODUCTS.ToListAsync();
            var stockBatches = await _context.STOCKBATCHES.Include(static sb => sb.PRODUCTS).ToListAsync();

            foreach (var batch in stockBatches)
            {
                var product = batch.PRODUCTS;
                string? alert = null;
                if (batch.REMAINING_QTY <= 0)
                    alertMessages.AppendLine($"{product.NAME} from Batch Id - {batch.BATCH_ID} is OUT OF STOCK.");
                else if (batch.EXPIRY_DATE <= DateTime.Today)
                    alertMessages.AppendLine($"{product.NAME} from Batch Id - {batch.BATCH_ID} has EXPIRED.");
                else if ((batch.EXPIRY_DATE - DateTime.Today).TotalDays <= 5)
                    alertMessages.AppendLine($"{product.NAME} from Batch Id - {batch.BATCH_ID} will EXPIRE in {(batch.EXPIRY_DATE-DateTime.Today).Days} days.");
                else if (batch.REMAINING_QTY > product.MAX_THRESHOLD)
                    alertMessages.AppendLine($"{product.NAME} from Batch Id - {batch.BATCH_ID} is OVERSTOCKED ({batch.REMAINING_QTY} units).");
                else if (batch.REMAINING_QTY < 10)
                    alertMessages.AppendLine($"{product.NAME} from Batch Id - {batch.BATCH_ID} has LOW STOCK ({batch.REMAINING_QTY} units).");
            }
            if (alertMessages.Length > 0)
            {
                //smsService.Send(alert);
                await _emailService.SendAsync("Inventory Alerts", alertMessages.ToString());
            }
        }
    }
}
