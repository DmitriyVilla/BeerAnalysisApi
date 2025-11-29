namespace BeerAnalysisApi.DTO
{
    public class BeerAnalysisResultDto
    {
        public decimal? CheapestPricePerLitre { get; set; }
        public decimal? MostExpensivePricePerLitre { get; set; }
        public List<ProductPriceDto> BeersWithExactPrice { get; set; } = new();
        public ProductBottleInfoDto? ProductWithMostBottles { get; set; }
    }
}
