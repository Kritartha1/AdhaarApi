using Adhaar.API.Helper;
using Adhaar.API.SMS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;


namespace Adhaar.API.Services
{
    public class SmsService:IIdentityMessageService
    {
        public async Task<bool> SendAsync(MessageRequest message)
        {
            // Twilio Begin
            /*var Twilio = new TwilioRestClient(
             Keys.SMSAccountIdentification,
            Keys.SMSAccountPassword);
            
            var result = Twilio.SendMessage(Keys.SMSAccountFrom,message.Destination, message.Body);
            Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
            Trace.TraceInformation(result.Status);
            Twilio doesn't currently have an async API, so return success.
            return Task.FromResult(0);*/
            //Twilio End


            string accountSid = Keys.SMSAccountIdentification;
            string authToken = Keys.SMSAccountPassword;

            TwilioClient.Init(accountSid, authToken);

            var mssg =await MessageResource.CreateAsync(
                body: "This is the ship that made the Kessel Run in fourteen parsecs?",
                from: new Twilio.Types.PhoneNumber(Keys.SMSAccountFrom),
                to: new Twilio.Types.PhoneNumber("+917008196889")
            );

            Trace.WriteLine( mssg.Status );

            if (mssg == null)
            {
                return false;
            }


            return true;



        }
    }
}
