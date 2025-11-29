using BeerAnalysisApi.DTO;
using BeerAnalysisApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeerAnalysisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // URL
        private const string DefaultProductDataUrl = "https://flapotest.blob.core.windows.net/test/ProductData.json";

        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("cheapest-per-litre")]
        public async Task<ActionResult<decimal?>> GetCheapestPerLitre([FromQuery] string? url)
        {
            var sourceUrl = string.IsNullOrWhiteSpace(url) ? DefaultProductDataUrl : url;
            _logger.LogInformation("GetCheapestPerLitre called with url={Url}", sourceUrl);

            var result = await _productService.GetCheapestPricePerLitreAsync(sourceUrl);

            return Ok(result);
        }

        [HttpGet("most-expensive-per-litre")]
        public async Task<ActionResult<decimal?>> GetMostExpensivePerLitre([FromQuery] string? url)
        {
            var sourceUrl = string.IsNullOrWhiteSpace(url) ? DefaultProductDataUrl : url;
            _logger.LogInformation("GetMostExpensivePerLitre called with url={Url}", sourceUrl);

            var result = await _productService.GetMostExpensivePricePerLitreAsync(sourceUrl);

            return Ok(result);
        }

        [HttpGet("price-exact")]
        public async Task<ActionResult<IReadOnlyList<ProductPriceDto>>> GetBeersWithExactPrice(
            [FromQuery] decimal price = 17.99m, // Default Price
            [FromQuery] string? url = null)
        {
            var sourceUrl = string.IsNullOrWhiteSpace(url) ? DefaultProductDataUrl : url; // Default URL
            _logger.LogInformation("GetBeersWithExactPrice called with url={Url}, price={Price}", sourceUrl, price);

            var result = await _productService.GetBeersWithExactPriceAsync(sourceUrl, price);

            return Ok(result);
        }


        [HttpGet("most-bottles")]
        public async Task<ActionResult<ProductBottleInfoDto?>> GetProductWithMostBottles(
            [FromQuery] string? url = null)
        {
            var sourceUrl = string.IsNullOrWhiteSpace(url) ? DefaultProductDataUrl : url;
            _logger.LogInformation("GetProductWithMostBottles called with url={Url}", sourceUrl);

            var result = await _productService.GetProductWithMostBottlesAsync(sourceUrl);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("analysis")]
        public async Task<ActionResult<BeerAnalysisResultDto>> GetFullAnalysis([FromQuery] string? url = null)
        {
            var sourceUrl = string.IsNullOrWhiteSpace(url) ? DefaultProductDataUrl : url;
            _logger.LogInformation("GetFullAnalysis called with url={Url}", sourceUrl);

            var result = await _productService.AnalyzeAsync(sourceUrl);

            return Ok(result);
        }


    }
}
