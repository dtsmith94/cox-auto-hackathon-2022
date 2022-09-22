using ImageRecognitionFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageRecognitionFunctions.Services
{
    internal class AnalyzeImageService
    {
        private readonly ComputerVisionClient _client;
        private readonly IFormFile _file;

        internal AnalyzeImageService(ComputerVisionClient client, IFormFile file)
        {
            _client = client;
            _file = file;
        }
        internal async Task<AnalyzeImageModel> AnalyzeImageAsync()
        {
            var visualFeatures = new List<VisualFeatureTypes?>
            {
                VisualFeatureTypes.Brands, VisualFeatureTypes.Categories, VisualFeatureTypes.Color, VisualFeatureTypes.Description, VisualFeatureTypes.Objects
            };

            var results = await _client.AnalyzeImageInStreamAsync(_file.OpenReadStream(), visualFeatures);

            // TODO: return the actual model
            return new AnalyzeImageModel();
        }
    }
}
