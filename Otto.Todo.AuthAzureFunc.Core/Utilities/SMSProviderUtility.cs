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
            var accountSid = Environment.GetEnvironmentVariable("ACCOUNT_SID");
            var authToken = Environment.GetEnvironmentVariable("AUTH_TOKEN");
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber(phoneNumber));
            messageOptions.MessagingServiceSid = Environment.GetEnvironmentVariable("MESSAGING_SERVICE_SID");
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
