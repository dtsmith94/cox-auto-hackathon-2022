using System.Collections.Generic;
using System.Globalization;
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

            var ti = CultureInfo.CurrentCulture.TextInfo;
            return new AnalyzeImageModel
            {
                Brand = ti.ToTitleCase(results.Brands.FirstOrDefault()?.Name ?? string.Empty),
                Description = results.Description.Captions.FirstOrDefault()?.Text,
                IsVehicle = IsImageOfVehicle(results.Objects),
                UnitType = ti.ToTitleCase(GetUnitType(results.Objects) ?? string.Empty)
            };
        }

        private static bool IsImageOfVehicle(IEnumerable<DetectedObject> objects)
        {
            var detectedObjects = objects as DetectedObject[] ?? objects.ToArray();

            // A hotwheels car just returned with objects "var"
            // A toy motorbike just returned with objects "var"
            var hasInvalidObject = detectedObjects.Any(x =>
                (x.ObjectProperty.ToLower() == "bicycle"));

            var hasVehicleObject = detectedObjects.Any(x =>
                (x.ObjectProperty.ToLower() == "land vehicle"
                 || x.ObjectProperty.ToLower() == "car"
                 || x.ObjectProperty.ToLower() == "van"
                 || x.ObjectProperty.ToLower() == "hatchback"
                 || x.ObjectProperty.ToLower() == "motorcycle"
                 || x.ObjectProperty.ToLower() == "vehicle"));

            return !hasInvalidObject && hasVehicleObject;
        }

        private static string GetUnitType(IEnumerable<DetectedObject> objects)
        {
            // A caravan just returned with objects "land Vehicle"
            // A hgv just returned with objects "land Vehicle" though the description sees it as truck
            // A quad just returned with objects "land Vehicle"
            // A trailer just returned with objects "land Vehicle" though the description sees it as a trailer
            var unitTypeObject = objects
                .OrderByDescending(x => x.Confidence)
                .FirstOrDefault(x =>
                    x.ObjectProperty.ToLower() == "car"
                    || x.ObjectProperty.ToLower() == "van"
                    || x.ObjectProperty.ToLower() == "hatchback"
                    || x.ObjectProperty.ToLower() == "motorcycle"
                    || x.ObjectProperty.ToLower() == "bicycle");

            return unitTypeObject?.ObjectProperty.ToLower() == "hatchback"
                ? unitTypeObject.Parent.ObjectProperty
                : unitTypeObject?.ObjectProperty;
        }
    }
}