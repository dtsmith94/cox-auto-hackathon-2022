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

namespace ComputerVisionQuickstart
{
    class Program
    {
        // Add your Computer Vision subscription key and endpoint
        static string subscriptionKey = "c6a994cd68c14b1ea68267c25c11c479";
        static string endpoint = "https://hackathon-22.cognitiveservices.azure.com/";

        private const string READ_TEXT_URL_IMAGE = "https://m.atcdn.co.uk/a/media/w800h600/366601a04d4b4c3ea610d8496dc20aa2.jpg";


        [FunctionName("ReadFileAsStream")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var file = req.Form.Files["file"];
        
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);
            var response = await AnalyzeImage(client, file);

            // var simpleResponse

            return new OkObjectResult(response);
        }

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        private static async Task<OcrResult> ReadFileOcrUrl(ComputerVisionClient client, IFormFile file)
        {
            var results = await client.RecognizePrintedTextInStreamAsync(true, file.OpenReadStream());

            return results;
        }

        private static async Task<ReadOperationResult> ReadFileUrl(ComputerVisionClient client, IFormFile file)
        {
            // Read text from URL
            var textHeaders = await client.ReadInStreamAsync(file.OpenReadStream());
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;

            // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));

            return results;
        }

        private static async Task<ImageAnalysis> AnalyzeImage(ComputerVisionClient client, IFormFile file)
        {
            var visualFeatures = new List<VisualFeatureTypes?>
            {
                VisualFeatureTypes.Brands, VisualFeatureTypes.Categories, VisualFeatureTypes.Color, VisualFeatureTypes.Description, VisualFeatureTypes.Objects
            };

            var results = await client.AnalyzeImageInStreamAsync(file.OpenReadStream(), visualFeatures);

            return results;
        }
    }
}