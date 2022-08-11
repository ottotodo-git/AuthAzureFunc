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
    public class ValidateTokenFunc
    {
        private readonly IAuthCoreService _authService;
        private object responseMessage;

        public ValidateTokenFunc(IAuthCoreService authService)
        {
            _authService = authService;
        }
        [FunctionName("ValidateTokenFunc")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "v1/validate")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            string reqBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<AuthRequestDTO>(reqBody);
            try
            {
                responseMessage = await _authService.validateTokenAsync(data);
                if (responseMessage == null)
                    return new ObjectResult(new ErrorDetails()
                    {
                        StatusCode = 500,
                        ErrorMessage = "User Authentication Failed"
                    });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ErrorDetails()
                {
                    StatusCode = 500,
                    ErrorMessage = ex.Message
                });
            }
            //Console.WriteLine(responseMessage);

            //var responseMessage = "testAPI";

            return new OkObjectResult(responseMessage);
        }
    }
}
