using BeerAnalysisApi.Clients;
using BeerAnalysisApi.Services;
using BeerAnalysisApi.Models;
using Moq;
using Xunit;

namespace BeerAnalysisApi.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductClient> _clientMock;
        private readonly ProductService _service;
        private readonly string _url;


        public ProductServiceTests()
        {
            _clientMock = new Mock<IProductClient>();

            _service = new ProductService(_clientMock.Object);

            _url = "http://dummy";
        }

        private static IReadOnlyList<Product> CreateTestProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Standard Beer",
                    Articles = new List<Article>
                    {
                        new Article
                        {
                            Id = 10,
                            ShortDescription = "20 x 0,5l",
                            Price = 17.99m,
                            PricePerUnitText = "(2,30 €/Liter)"
                        }
                    }
                },
                new Product
                {
                    Id = 2,
                    Name = "Premium Beer",
                    Articles = new List<Article>
                    {
                        new Article
                        {
                            Id = 11,
                            ShortDescription = "6 x 0,33l",
                            Price = 17.99m,
                            PricePerUnitText = "(3,50 €/Liter)"
                        }
                    }
                },
                new Product
                {
                    Id = 3,
                    Name = "Big Pack Beer",
                    Articles = new List<Article>
                    {
                        new Article
                        {
                            Id = 12,
                            ShortDescription = "24 x 0,5l",
                            Price = 19.99m,
                            PricePerUnitText = "(2,50 €/Liter)"
                        }
                    }
                }
            };
        }





        [Fact]
        public async Task GetCheapestPricePerLitreAsync_Returns_MinValue()
        {
            // Arrange
            var products = CreateTestProducts();
            _clientMock.Setup(c => c.GetProductsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(products);

            // Act
            var result = await _service.GetCheapestPricePerLitreAsync(_url);

            // Assert
            Assert.Equal(2.30m, result);
        }

        [Fact]
        public async Task GetMostExpensivePricePerLitreAsync_Returns_MaxValue()
        {
            // Arrange
            var products = CreateTestProducts();
            _clientMock.Setup(c => c.GetProductsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(products);

            // Act
            var result = await _service.GetMostExpensivePricePerLitreAsync(_url);

            // Assert
            Assert.Equal(3.50m, result);
        }

        [Fact]
        public async Task GetBeersWithExactPriceAsync_Filters_And_Sorts_By_PricePerLitre()
        {
            // Arrange
            var products = CreateTestProducts();
            _clientMock.Setup(c => c.GetProductsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(products);

            // Act
            var result = await _service.GetBeersWithExactPriceAsync(_url, 17.99m);

            // Assert
            Assert.NotEmpty(result);

            for (int i = 1; i < result.Count; i++)
            {
                // next is bigger
                Assert.True(result[i].PricePerLitre > result[i - 1].PricePerLitre);
            }
        }

        [Fact]
        public async Task GetProductWithMostBottlesAsync_Returns_Product_With_Max_Bottles()
        {
            // Arrange
            var products = CreateTestProducts();
            _clientMock
                .Setup(c => c.GetProductsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            // Act
            var result = await _service.GetProductWithMostBottlesAsync(_url);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Big Pack Beer", result!.ProductName);
            Assert.Equal(24, result.BottleCount);
        }
    }
}