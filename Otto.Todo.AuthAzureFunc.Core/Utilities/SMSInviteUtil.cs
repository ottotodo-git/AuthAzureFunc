using Otto.Todo.AuthAzureFunc.Core.Interfaces;
using Otto.Todo.AuthAzureFunc.Models.DTOs;
using System;
using System.Collections.Generic;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Otto.Todo.AuthAzureFunc.Core.Utilities
{
    public static class SMSInviteUtil
    {
        public static void sendSMSToCustomer(long phone)
        {
            var accountSid = Environment.GetEnvironmentVariable("ACCOUNT_SID");
            var authToken = Environment.GetEnvironmentVariable("AUTH_TOKEN");
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber(phone.ToString()));
            messageOptions.MessagingServiceSid = Environment.GetEnvironmentVariable("MESSAGING_SERVICE_SID");
            messageOptions.Body = "I'm inviting you to use (#name) app ";

            var message = MessageResource.Create(messageOptions);
            //Console.WriteLine(message.Body);
            //return message;
        }

    }
}
