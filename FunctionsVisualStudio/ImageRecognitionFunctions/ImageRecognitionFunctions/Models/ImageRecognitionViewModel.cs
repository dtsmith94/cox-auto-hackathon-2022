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

        [JsonPropertyName("imageProportionOfVehicle")]
        public int? ImageProportionOfVehicle { get; set; }

        [JsonPropertyName("imageThumbnail")]
        public string ImageThumbnail { get; set; }

        [JsonPropertyName("isVehicle")]
        public bool? IsVehicle { get; set; }

        [JsonPropertyName("unitType")]
        public string UnitType { get; set; }
    }
}
