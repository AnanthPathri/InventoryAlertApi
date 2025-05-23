using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace InventoryAlertApi.Services
{
    public class SmsService
    {
        private const string accountSid = "AC18d8a6fc52a012f5edaf743bf93fa80a";
        private const string authToken = "734d761ddb8101e9f24afb26f24290d6";
        private const string fromPhone = "+18449803617";
        private const string toPhone = "+917680899689";

        public void Send(string message)
        {
            TwilioClient.Init(accountSid, authToken);
            var msg=MessageResource.Create(
                body: message,
                from: new PhoneNumber(fromPhone),
                to: new PhoneNumber(toPhone));
        }
    }
}
