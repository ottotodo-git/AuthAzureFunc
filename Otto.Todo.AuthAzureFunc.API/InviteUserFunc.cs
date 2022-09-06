using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Otto.Todo.AuthAzureFunc.Core.Interfaces;
using Otto.Todo.AuthAzureFunc.Models.DTOs;
using Otto.Todo.AuthAzureFunc.Models.Models;

namespace Otto.Todo.AuthAzureFunc.API
{
    public class InviteUserFunc
    {
        private readonly IAuthCoreService _authService;

        public InviteUserFunc(IAuthCoreService authService)
        {
            _authService = authService;
        }

        [FunctionName("InviteUserFunc")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/invite/{phone_number}")] HttpRequest req,
            ILogger log,
            long phone_number)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                _authService.inviteUserAsync(phone_number);
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ErrorDetails()
                {
                    StatusCode = 500,
                    ErrorMessage = ex.Message
                })
                {
                    StatusCode = 500
                };
            }

            return new OkObjectResult(new ErrorDetails()
            {
                StatusCode = 200
            })
            {
                StatusCode = 200
            };

        }
    }
}
