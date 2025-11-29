namespace BeerAnalysisApi.DTO
{
    public class ProductPriceDto
    {
        public string ProductName { get; set; } = string.Empty;
        public string ArticleDescription { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal PricePerLitre { get; set; }
    }
}
