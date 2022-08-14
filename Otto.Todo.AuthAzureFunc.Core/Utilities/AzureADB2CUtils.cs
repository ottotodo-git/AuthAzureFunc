using Azure.Identity;
using Microsoft.Graph;
using Otto.Todo.AuthAzureFunc.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otto.Todo.AuthAzureFunc.Core.Utilities
{
    public static class AzureADB2CUtils
    {
        public static GraphServiceClient getGraphClient()
        {
            // The client credentials flow requires that you request the
            // /.default scope, and preconfigure your permissions on the
            // app registration in Azure. An administrator must grant consent
            // to those permissions beforehand.
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            // Multi-tenant apps can use "common",
            // single-tenant apps must use the tenant ID from the Azure portal
            var tenantId = Environment.GetEnvironmentVariable("AZURE_TENANT_ID");

            // Values from app registration
            var clientId = Environment.GetEnvironmentVariable("FRONTEND_CLIENT_ID");
            var clientSecret = Environment.GetEnvironmentVariable("FRONTEND_CLIENT_SECRET");

            // using Azure.Identity;
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            // https://docs.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret, options);

            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);
            return graphClient;

        }

        public async static Task<AuthRequestDTO> createUserAsync(AuthRequestDTO auth)
        {
            GraphServiceClient graphClient = getGraphClient();
            var phone_trim = auth.PhoneNumber.Substring(auth.PhoneNumber.Length - 5, 5);
            var username = auth.Name + "_" + phone_trim + "@neelnagahotmail.onmicrosoft.com";
            var user = new User
            {
                AccountEnabled = true,
                DisplayName = auth.Name,
                MailNickname = auth.Name,
                MobilePhone = auth.PhoneNumber,
                UserPrincipalName = username,
                PasswordProfile = new PasswordProfile
                {
                    ForceChangePasswordNextSignIn = true,
                    Password = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username)),
                }
            };

            var user_out = await graphClient.Users
                .Request()
                .AddAsync(user);

            if(user_out.Id != null)
            {
                auth.ExternalUserId = user_out.Id;
            }
            return auth;

        }

        public async static Task<User> getUserAsync(string extid)
        {
            var graphClient = getGraphClient();

            var user = await graphClient.Users[extid]
                            .Request()
                            .GetAsync();

            return user;

        }

        public async static Task<ICollection<User>> getUserByPhoneAsync(string phone)
        {
            var graphClient = getGraphClient();

            var queryOptions = new List<QueryOption>()
            {
                new QueryOption("$count", "true")
            };

            //strip out + from the phone
            if (phone.Contains("+"))
            {
                phone = phone.Replace("+","").Trim();
            }
            var user = await graphClient.Users
                            .Request(queryOptions)
                            .Header("ConsistencyLevel", "eventual")
                            .Filter($"MobilePhone eq '{phone}'")
                            .GetAsync();

            return user;

        }
    }
}
