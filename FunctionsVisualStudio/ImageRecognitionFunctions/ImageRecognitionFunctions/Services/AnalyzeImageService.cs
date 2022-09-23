using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageRecognitionFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

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
                VisualFeatureTypes.Brands, VisualFeatureTypes.Categories, VisualFeatureTypes.Color,
                VisualFeatureTypes.Description, VisualFeatureTypes.Objects
            };

            var results = await _client.AnalyzeImageInStreamAsync(file.OpenReadStream(), visualFeatures);

            return new AnalyzeImageModel
            {
                Brand = results.Brands.FirstOrDefault()?.Name,
                Description = results.Description.Captions.FirstOrDefault()?.Text,
                IsImageOfVehicle = results.Objects.Any(x =>
                    (x.ObjectProperty.ToLower() == "van"
                     || x.ObjectProperty.ToLower() == "car")
                    && x.ObjectProperty.ToLower() == "land vehicle")
            };
        }
    }
}