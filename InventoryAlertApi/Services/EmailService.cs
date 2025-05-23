using MailKit.Net.Smtp;
using MimeKit;

namespace InventoryAlertApi.Services
{
    public class EmailService
    {
        public async Task SendAsync(string subject,string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Inventory System", "pathriananthakrishna@gmail.com"));
            message.To.Add(new MailboxAddress("Manager", "pathriananthakrishna@gmail.com"));
            message.Subject = subject;
            message.Body=new TextPart("plain") { Text = body};
            using var client=new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587,MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("pathriananthakrishna@gmail.com", "vmywogeozvkmngqt");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
