using System.Text.Json.Serialization;

namespace BeerAnalysisApi.Models
{
    public class Article
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("shortDescription")]
        public string ShortDescription { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; } = string.Empty;

        [JsonPropertyName("pricePerUnitText")]
        public string PricePerUnitText { get; set; } = string.Empty;

        [JsonPropertyName("image")]
        public string Image { get; set; } = string.Empty;
    }
}
