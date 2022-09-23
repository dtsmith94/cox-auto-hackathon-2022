using ImageRecognitionFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ImageRecognitionFunctions.Services
{
    internal class GenerateThumbnailService
    {
        private readonly ComputerVisionClient _client;

        internal GenerateThumbnailService(ComputerVisionClient client)
        {
            _client = client;
        }

        internal async Task<ThumbnailModel> GenerateThumbnailAsync(IFormFile file)
        {

            var results = await _client.GenerateThumbnailInStreamAsync(300, 200, file.OpenReadStream(), true);

            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                results.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            string base64 = Convert.ToBase64String(bytes);

            // TODO: return the actual model
            return new ThumbnailModel { Base64 = base64 };
        }
    }
}
