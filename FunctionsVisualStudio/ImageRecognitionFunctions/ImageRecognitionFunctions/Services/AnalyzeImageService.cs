using ImageRecognitionFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageRecognitionFunctions.Services
{
    internal class AnalyzeImageService
    {
        private readonly ComputerVisionClient _client;

        internal AnalyzeImageService(ComputerVisionClient client)
        {
            _client = client;
        }
        internal async Task<AnalyzeImageModel> AnalyzeImageAsync(IFormFile file)
        {
            var visualFeatures = new List<VisualFeatureTypes?>
            {
                VisualFeatureTypes.Brands, VisualFeatureTypes.Categories, VisualFeatureTypes.Color, VisualFeatureTypes.Description, VisualFeatureTypes.Objects
            };

            var results = await _client.AnalyzeImageInStreamAsync(file.OpenReadStream(), visualFeatures);

            // TODO: return the actual model
            return new AnalyzeImageModel();
        }
    }
}
