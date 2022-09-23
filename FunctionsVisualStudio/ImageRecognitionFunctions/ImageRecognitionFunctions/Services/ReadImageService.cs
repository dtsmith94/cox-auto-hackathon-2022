using ImageRecognitionFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageRecognitionFunctions.Services
{
    internal class ReadImageService
    {
        private readonly ComputerVisionClient _client;

        internal ReadImageService(ComputerVisionClient client)
        {
            _client = client;
        }
        internal async Task<ReadImageModel> ReadImageAsync(IFormFile file)
        {
            // Read text from URL
            var textHeaders = await _client.ReadInStreamAsync(file.OpenReadStream());
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

            var vrm = ExtractVrm(results);

            return new ReadImageModel { Vrm = vrm };
        }

        private string ExtractVrm(ReadOperationResult results)
        {
            var lines = results.AnalyzeResult.ReadResults.SelectMany(readResult => readResult.Lines);

            foreach (var line in lines)
            {
                var text = line.Text.Replace(" ", "").ToUpper();

                var regex = new Regex(@"(^[A-Z]{2}[0-9]{2}\s?[A-Z]{3}$)|(^[A-Z][0-9]{1,3}[A-Z]{3}$)|(^[A-Z]{3}[0-9]{1,3}[A-Z]$)|(^[0-9]{1,4}[A-Z]{1,2}$)|(^[0-9]{1,3}[A-Z]{1,3}$)|(^[A-Z]{1,2}[0-9]{1,4}$)|(^[A-Z]{1,3}[0-9]{1,3}$)|(^[A-Z]{1,3}[0-9]{1,4}$)|(^[0-9]{3}[DX]{1}[0-9]{3}$)");
                var match = regex.Match(text);

                if (match.Success)
                    return text;
            }

            return string.Empty;
        }
    }
}
