using Otto.Todo.AuthAzureFunc.Core.Interfaces;
using Otto.Todo.AuthAzureFunc.Models.DTOs;
using System;
using System.Collections.Generic;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Otto.Todo.AuthAzureFunc.Core.Utilities
{
    public static class SMSProviderUtils
    {
        public static void sendSMSToCustomer(AuthRequestDTO auth)
        {
            var accountSid = Environment.GetEnvironmentVariable("ACCOUNT_SID");
            var authToken = Environment.GetEnvironmentVariable("AUTH_TOKEN");
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber(auth.PhoneNumber));
            auth.VerificationCode = VerifyCode();
            messageOptions.MessagingServiceSid = Environment.GetEnvironmentVariable("MESSAGING_SERVICE_SID");
            messageOptions.Body = "Your OttoTodo verification code is: "+ auth.VerificationCode;

            var message = MessageResource.Create(messageOptions);
            //Console.WriteLine(message.Body);
            //return message;
        }

        public static string VerifyCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
     
    }
}
