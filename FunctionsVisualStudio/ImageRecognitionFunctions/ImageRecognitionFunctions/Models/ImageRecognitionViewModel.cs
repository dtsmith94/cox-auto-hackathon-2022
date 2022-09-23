using System.Text.Json.Serialization;

namespace ImageRecognitionFunctions.Models
{
    internal class ImageRecognitionViewModel
    {
        [JsonPropertyName("vrm")]
        public string Vrm { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("brand")]
        public string Brand { get; set; }

        [JsonPropertyName("imageProportionOfCar")]
        public int ImageProportionOfCar { get; set; }
    }
}
