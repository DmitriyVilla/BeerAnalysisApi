using System.Text.Json.Serialization;

namespace BeerAnalysisApi.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("brandName")]
        public string BrandName { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("descriptionText")]
        public string? DescriptionText { get; set; }

        [JsonPropertyName("articles")]
        public List<Article> Articles { get; set; } = new();
    }
}
