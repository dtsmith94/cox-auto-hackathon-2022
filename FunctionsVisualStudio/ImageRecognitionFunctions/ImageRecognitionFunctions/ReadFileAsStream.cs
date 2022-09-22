using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using System.Threading;
using Newtonsoft.Json;
using ImageRecognitionFunctions.Services;
using ImageRecognitionFunctions.Models;

namespace ComputerVisionQuickstart
{
    class Program
    {
        // Add your Computer Vision subscription key and endpoint
        static string subscriptionKey = "c6a994cd68c14b1ea68267c25c11c479";
        static string endpoint = "https://hackathon-22.cognitiveservices.azure.com/";

        [FunctionName("ReadFileAsStream")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var file = req.Form.Files["file"];
        
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

            // Read Image
            var readImageService = new ReadImageService(client, file);
            var readImageModel = await readImageService.ReadImageAsync();

            // Analyze Image
            var analyzeImageService = new AnalyzeImageService(client, file);
            var analyzeImageModel = await analyzeImageService.AnalyzeImageAsync();

            // combine models into single view model to return in the response
            var viewModel = new ImageRecognitionViewModel
            {
                Vrm = readImageModel.Vrm
            };

            return new OkObjectResult(viewModel);
        }

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        //private static async Task<OcrResult> ReadFileOcrUrl(ComputerVisionClient client, IFormFile file)
        //{
        //    var results = await client.RecognizePrintedTextInStreamAsync(true, file.OpenReadStream());

        //    return results;
        //}
    }
}