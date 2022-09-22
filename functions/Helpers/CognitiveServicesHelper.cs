using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Linq;

namespace functions
{
    public class CognitiveServicesHelper : ICognitiveServicesHelper
    {
        private readonly ComputerVisionClient _client;

        public CognitiveServicesHelper(IComputerVisionClient computerVisionClient)
        {
            this._client = (ComputerVisionClient)computerVisionClient;
        }
        private const string READ_TEXT_URL_IMAGE = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/printed_text.jpg";

        public async Task<ReadOperationResult> CallReadFileUrl()
        {
            // Extract text (OCR) from a URL image using the Read API
            return await ReadFileUrl(_client, READ_TEXT_URL_IMAGE);
        }

        private static async Task<ReadOperationResult> ReadFileUrl(ComputerVisionClient client, string urlFile)
        {
            // Read text from URL
            var textHeaders = await client.ReadAsync(urlFile);
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
    }
}