using InventoryAlertApi.Data;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Org.BouncyCastle.Crypto.Macs;

namespace InventoryAlertApi.Services
{
    public class EmailService
    {
        /*Note - Mail is created for this application
        mail- inventrasend@gmail.com
        password -  InventraSend@12
        app password - wzvugusepqwrjqos*/

        private readonly InventoryDbContext _context;
        public EmailService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task SendAsync(string subject,string body)
        {
            var alertRule = await _context.ALERTRULES.FirstOrDefaultAsync(a => a.RULE_TYPE == subject && a.IS_ACTIVE == true && a.COMMUNICATION_CHANNAL.ToLower() == "email");
            if (alertRule == null)
                return;
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("INVENTRA_Inventory System", "inventrasend@gmail.com"));
            message.To.Add(new MailboxAddress("Manager", alertRule.RECIPIENT_EMAIL_ID));
            message.Subject = subject;
            message.Body=new TextPart("plain") { Text = body};
            using var client=new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587,MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("inventrasend@gmail.com", "wzvugusepqwrjqos");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
