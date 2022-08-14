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
using System.Linq;
using Otto.Todo.AuthAzureFunc.Models.Models;

namespace Otto.Todo.AuthAzureFunc.API
{
    public class GetUsersFunc
    {
        private readonly IAuthCoreService _authService;

        public GetUsersFunc(IAuthCoreService authService)
        {
            _authService = authService;
        }

        [FunctionName("GetUsersFunc")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "v1/users")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var responseMessage = await _authService.getUsersAsync();
                if (responseMessage.ToList().Count == 0)
                {
                    return new NotFoundObjectResult(new ErrorDetails()
                    {
                        StatusCode = 404,
                        ErrorMessage = "Record not found"
                    });
                }
                //Console.WriteLine(responseMessage);

                //var responseMessage = "testAPI";

                return new OkObjectResult(responseMessage);
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
        }
    }
}
