using Microsoft.Extensions.Caching.Memory;
using Moq;
using PokemonApi.ExternalApi;
using PokemonApi.Models.Dto;
using PokemonApi.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PokemonApi.Tests
{
    public class PokemonServiceTests
    {
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly Mock<IPokemonApiClient> _pokemonApiClientMock;
        private readonly PokemonService _pokemonService;

        public PokemonServiceTests()
        {
            _memoryCacheMock = new Mock<IMemoryCache>();
            _pokemonApiClientMock = new Mock<IPokemonApiClient>();
            _pokemonService = new PokemonService(_pokemonApiClientMock.Object, _memoryCacheMock.Object);
        }

        [Fact]
        public async Task GetPokemonByNameAsync_ShouldReturnFromCache_WhenPokemonIsCached()
        {
            // Arrange
            var cachedPokemon = new PokemonDetail { Name = "Pikachu" };
            object cachedObject = cachedPokemon;

            _memoryCacheMock.Setup(c => c.TryGetValue("Pikachu", out cachedObject))
                            .Returns(true);

            // Act
            var result = await _pokemonService.GetPokemonByNameAsync("Pikachu");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Pikachu", result.Name);
        }

        [Fact]
        public async Task GetRandomPokemonAsync_ShouldReturnFromCache_WhenPokemonsAreCached()
        {
            // Arrange
            var cachedPokemons = new List<PokemonSummary> { new PokemonSummary { Name = "Pikachu", Url = "http://example.com" } };
            object cachedObject = cachedPokemons;

            _memoryCacheMock.Setup(c => c.TryGetValue("randomPokemons", out cachedObject))
                            .Returns(true);

            // Act
            var result = await _pokemonService.GetRandomPokemonAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Pikachu", result.First().Name);
        }

        [Fact]
        public async Task DownloadSpriteImageAsBase64_ShouldReturnFromCache_WhenImageIsCached()
        {
            // Arrange
            var cachedImage = "base64imagecontent";
            object cachedObject = cachedImage;

            _memoryCacheMock.Setup(c => c.TryGetValue("http://example.com/sprite.png", out cachedObject))
                            .Returns(true);

            // Act
            var result = await _pokemonService.DownloadSpriteImageAsBase64("http://example.com/sprite.png");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("base64imagecontent", result);
        }
    }
}
