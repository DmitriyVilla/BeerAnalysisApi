using BeerAnalysisApi.DTO;

namespace BeerAnalysisApi.Services
{
    public interface IProductService
    {
        Task<BeerAnalysisResultDto> AnalyzeAsync(string url, CancellationToken token = default);

        Task<decimal?> GetCheapestPricePerLitreAsync(string url, CancellationToken token = default);

        Task<decimal?> GetMostExpensivePricePerLitreAsync(string url, CancellationToken token = default);

        Task<IReadOnlyList<ProductPriceDto>> GetBeersWithExactPriceAsync(string url, decimal price, CancellationToken token = default);

        Task<ProductBottleInfoDto?> GetProductWithMostBottlesAsync(string url, CancellationToken token = default);
    }
}
