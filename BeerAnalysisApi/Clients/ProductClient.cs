using BeerAnalysisApi.Models;
using System.Text.Json;

namespace BeerAnalysisApi.Clients
{
    public class ProductClient : IProductClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductClient> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public ProductClient(HttpClient httpClient, ILogger<ProductClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(string url, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Url must not be empty.", nameof(url));

            _logger.LogInformation("Requesting product data from {Url}", url);

            using var response = await _httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Received non-success status code {StatusCode} from {Url}", (int)response.StatusCode, url);

                response.EnsureSuccessStatusCode();
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);

            var products = JsonSerializer.Deserialize<List<Product>>(json, _jsonOptions) ?? new List<Product>();

            _logger.LogInformation("Successfully deserialized {Count} products", products.Count);

            return products;
        }
    }
}
