using ImageRecognitionFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Threading.Tasks;

namespace ImageRecognitionFunctions.Services
{
    internal class ReadImageService
    {
        private readonly ComputerVisionClient _client;
        private readonly IFormFile _file;

        internal ReadImageService(ComputerVisionClient client, IFormFile file)
        {
            _client = client;
            _file = file;
        }
        internal async Task<AnalyzeImageModel> ReadImageAsync()
        {
            // Read text from URL
            var textHeaders = await _client.ReadInStreamAsync(_file.OpenReadStream());
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
                results = await _client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));

            // TODO: return the actual model
            return new AnalyzeImageModel();
        }
    }
}
