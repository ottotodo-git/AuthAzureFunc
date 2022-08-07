using System;
using System.Collections.Generic;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Otto.Todo.AuthAzureFunc.Core.Utilities
{
    public static class SMSProviderUtility
    {
        public static MessageResource sendSMSToCustomer(String phoneNumber)
        {
            var accountSid = "AC298c6b87b8d597fd33b7e2756aef7723";
            var authToken = "d65990693270a7c3f5ed579fda497cfc";
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber(phoneNumber));
            messageOptions.MessagingServiceSid = "MG1b99eb35569c3c0381e155cf7dca1210";
            messageOptions.Body = "Your OttoTodo verification code is: "+VerifyCode();

            var message = MessageResource.Create(messageOptions);
            //Console.WriteLine(message.Body);
            return message;
        }

        public static string VerifyCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
     
    }
}
