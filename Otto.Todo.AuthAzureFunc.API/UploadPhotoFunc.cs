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
using System.Collections.Generic;
using System.Collections;

namespace Otto.Todo.AuthAzureFunc.API
{
    public class UploadPhotoFunc
    {
        private readonly IAuthCoreService _authService;
        private object responseMessage;

        public UploadPhotoFunc(IAuthCoreService authService)
        {
            _authService = authService;
        }

        [FunctionName("UploadPhotoFunc")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "v1/upload")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            //var testValue = req.Form.Files[0].FileName;
            if(req.Form.Files.Count > 0)
            {
                IFormFile uploadfile = req.Form.Files[0];
                Hashtable uploadKeys = new Hashtable();
                uploadKeys.Add("userid", req.Form["userid"]);
                responseMessage = await _authService.uploadPhotoAsync(uploadfile, uploadKeys);
            }
            

            return new OkObjectResult(responseMessage);
        }
    }
}
