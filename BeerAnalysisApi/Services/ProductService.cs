using BeerAnalysisApi.Clients;
using BeerAnalysisApi.DTO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BeerAnalysisApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductClient _client;
        //private static readonly Regex PriceNumberRegex = new Regex(@"(?<val>\d+[.,]\d+)", RegexOptions.Compiled);
        private static readonly Regex PriceNumberRegex = new Regex(@"\((?<val>\d+[.,]\d+)\s*€/Liter\)", RegexOptions.Compiled);

        //private static readonly Regex BottleCountRegex = new Regex(@"(?<count>\d+)\s*[xX×]\s*\d", RegexOptions.Compiled);
        private static readonly Regex BottleCountRegex = new Regex(@"^(?<count>\d+)\s*[xX]\s*\d", RegexOptions.Compiled);


        public ProductService(IProductClient client)
        {
            _client = client;
        }

       
        public async Task<decimal?> GetCheapestPricePerLitreAsync(string url, CancellationToken token = default)
        {
            var products = await _client.GetProductsAsync(url, token);

            return products
                .SelectMany(p => p.Articles)
                .Select(a => ParsePricePerLitre(a.PricePerUnitText))
                .Where(v => v.HasValue)
                .Min();
        }


        public async Task<decimal?> GetMostExpensivePricePerLitreAsync(string url, CancellationToken token = default)
        {
            var products = await _client.GetProductsAsync(url, token);

            return products
                .SelectMany(p => p.Articles)
                .Select(a => ParsePricePerLitre(a.PricePerUnitText))
                .Where(v => v.HasValue)
                .Max();
        }


        public async Task<IReadOnlyList<ProductPriceDto>> GetBeersWithExactPriceAsync(string url, decimal price, CancellationToken token = default)
        {
            var products = await _client.GetProductsAsync(url, token);

            return products
                .SelectMany(p =>
                    p.Articles
                        .Where(a => a.Price == price)
                        .Select(a => new ProductPriceDto
                        {
                            ProductName = p.Name,
                            ArticleDescription = a.ShortDescription,
                            Price = a.Price,
                            PricePerLitre = ParsePricePerLitre(a.PricePerUnitText) ?? 0
                        }))
                .OrderBy(x => x.PricePerLitre)
                .ToList();
        }


        public async Task<ProductBottleInfoDto?> GetProductWithMostBottlesAsync(string url, CancellationToken token = default)
        {
            var products = await _client.GetProductsAsync(url, token);

            // Exmpl 20 x 0,5l
            var parsed = products
                .Select(p => new
                {
                    Product = p,
                    Bottles = ParseBottleCount(p.Articles.FirstOrDefault()?.ShortDescription)
                })
                .Where(x => x.Bottles.HasValue)
                .OrderByDescending(x => x.Bottles)
                .FirstOrDefault();

            if (parsed == null)
                return null;

            return new ProductBottleInfoDto
            {
                ProductName = parsed.Product.Name,
                BottleCount = parsed.Bottles.Value
            };
        }


        
        private static decimal? ParsePricePerLitre(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            var match = PriceNumberRegex.Match(text);

            if (!match.Success)
                return null;

            var numberText = match.Groups["val"].Value.Replace(',', '.');

            if (decimal.TryParse(numberText, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                return value;

            return null;
        }

        private static int? ParseBottleCount(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return null;

            var match = BottleCountRegex.Match(description);

            if (!match.Success)
                return null;

            if (int.TryParse(match.Groups["count"].Value, out var count))
                return count;

            return null;
        }

        public async Task<BeerAnalysisResultDto> AnalyzeAsync(string url, CancellationToken token = default)
        {
            var result = new BeerAnalysisResultDto
            {
                CheapestPricePerLitre = await GetCheapestPricePerLitreAsync(url, token),
                MostExpensivePricePerLitre = await GetMostExpensivePricePerLitreAsync(url, token),
                BeersWithExactPrice = (await GetBeersWithExactPriceAsync(url, 17.99m, token)).ToList(),
                ProductWithMostBottles = await GetProductWithMostBottlesAsync(url, token)
            };

            return result;
        }
    }
}
