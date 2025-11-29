using BeerAnalysisApi.Models;

namespace BeerAnalysisApi.Clients
{
    public interface IProductClient
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(string url, CancellationToken cancellationToken = default);
    }
}
