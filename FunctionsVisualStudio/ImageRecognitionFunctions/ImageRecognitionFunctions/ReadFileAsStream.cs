using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using ImageRecognitionFunctions.Services;
using ImageRecognitionFunctions.Models;
using System;

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

            var client = Authenticate(endpoint, subscriptionKey);

            var result = await RunProcesses(client, file);

            return new OkObjectResult(result);
        }

        private static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        private static async Task<ImageRecognitionViewModel> RunProcesses(ComputerVisionClient client, IFormFile file)
        {
            ReadImageModel readImageModel = null;
            AnalyzeImageModel analyzeImageModel = null;
            ThumbnailModel thumbnailModel = null;

            try
            {
                // Read Image
                var readImageService = new ReadImageService(client);
                readImageModel = await readImageService.ReadImageAsync(file);
            }
            catch { }

            try
            {
                // Analyze Image
                var analyzeImageService = new AnalyzeImageService(client);
                analyzeImageModel = await analyzeImageService.AnalyzeImageAsync(file);
            }
            catch { }

            try
            {
                // Generate Thumbnail Image
                var generateThumbnailService = new GenerateThumbnailService(client);
                thumbnailModel = await generateThumbnailService.GenerateThumbnailAsync(file);
            }
            catch { }

            // combine models into single view model to return in the response
            return new ImageRecognitionViewModel
            {
                Vrm = readImageModel?.Vrm,
                ImageThumbnail = thumbnailModel?.Base64,
                Description = analyzeImageModel?.Description,
                Brand = analyzeImageModel?.Brand
            };
        }
    }
}
