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
using Otto.Todo.AuthAzureFunc.Models.Models;

namespace Otto.Todo.AuthAzureFunc.API
{
    public  class GetUserFunc
    {
        private readonly IAuthCoreService _authService;
        private object responseMessage;
        public GetUserFunc(IAuthCoreService authService)
        {
            _authService = authService;
        }
        [FunctionName("GetUserFunc")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "v1/users/{id}")] HttpRequest req,
            long id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
               responseMessage = await _authService.getUserAsync(id);
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

            if (responseMessage == null)
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
    }
}
