using Microsoft.Extensions.Caching.Memory;
using Moq;
using PokemonApi.ExternalApi;
using PokemonApi.Models.Dto;
using PokemonApi.Services;

namespace PokemonApi.Tests.Integration
{
    public class PokemonServiceIntegrationTests
    {
        private readonly PokemonService _pokemonService;
        private readonly Mock<IPokemonApiClient> _mockPokemonApiClient;
        private readonly IMemoryCache _memoryCache;

        public PokemonServiceIntegrationTests()
        {
            _mockPokemonApiClient = new Mock<IPokemonApiClient>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _pokemonService = new PokemonService(_mockPokemonApiClient.Object, _memoryCache);
        }

        [Fact]
        public async Task GetPokemonByNameAsync_ShouldFetchFromApi_AndCacheResult()
        {
            // Arrange
            var pokemonName = "pikachu";
            var pokemonDetail = new PokemonDetail { Name = pokemonName };
            _mockPokemonApiClient
                .Setup(api => api.GetPokemonByNameAsync(pokemonName))
                .ReturnsAsync(pokemonDetail);

            // Act
            var result = await _pokemonService.GetPokemonByNameAsync(pokemonName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pokemonName, result.Name);

            Assert.True(_memoryCache.TryGetValue(pokemonName, out PokemonDetail cachedPokemon));
            Assert.Equal(pokemonDetail.Name, cachedPokemon.Name);
            
            // Verificar se a API foi chamada apenas uma vez ao chamar o serviço novamente
            var cachedResult = await _pokemonService.GetPokemonByNameAsync(pokemonName);
            _mockPokemonApiClient.Verify(api => api.GetPokemonByNameAsync(It.IsAny<string>()), Times.Once);
            Assert.Equal(result.Name, cachedResult.Name);
        }

        [Fact]
        public async Task GetRandomPokemonAsync_ShouldFetchFromApi_AndCacheResult()
        {
            // Arrange
            var randomPokemons = new List<PokemonSummary>
            {
                new PokemonSummary { Name = "bulbasaur" },
                new PokemonSummary { Name = "charmander" }
            };
            _mockPokemonApiClient
                .Setup(api => api.GetRandomPokemonAsync())
                .ReturnsAsync(randomPokemons);

            // Act
            var result = await _pokemonService.GetRandomPokemonAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            Assert.True(_memoryCache.TryGetValue("randomPokemons", out IEnumerable<PokemonSummary> cachedPokemons));
            Assert.Equal(randomPokemons, cachedPokemons);

            // Verificar se a API foi chamada apenas uma vez ao chamar o serviço novamente
            var cachedResult = await _pokemonService.GetRandomPokemonAsync();
            _mockPokemonApiClient.Verify(api => api.GetRandomPokemonAsync(), Times.Once);
            Assert.Equal(result, cachedResult);
        }

        [Fact]
        public async Task DownloadSpriteImageAsBase64_ShouldFetchImageFromApi_AndCacheResult()
        {
            // Arrange
            var imageUrl = "https://example.com/sprite.png";
            var base64Image = "sampleBase64Image";
            _mockPokemonApiClient
                .Setup(api => api.DownloadSpriteImageAsBase64(imageUrl))
                .ReturnsAsync(base64Image);

            // Act
            var result = await _pokemonService.DownloadSpriteImageAsBase64(imageUrl);

            // Assert
            Assert.Equal(base64Image, result);

            // Verify caching
            Assert.True(_memoryCache.TryGetValue(imageUrl, out string cachedImage));
            Assert.Equal(base64Image, cachedImage);

            // Verificar se a API foi chamada apenas uma vez ao chamar o serviço novamente
            var cachedResult = await _pokemonService.DownloadSpriteImageAsBase64(imageUrl);
            _mockPokemonApiClient.Verify(api => api.DownloadSpriteImageAsBase64(It.IsAny<string>()), Times.Once);
            Assert.Equal(result, cachedResult);
        }
    }
}
