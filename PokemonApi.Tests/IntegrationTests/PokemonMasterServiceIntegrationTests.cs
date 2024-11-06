using PokemonApi.Database;
using PokemonApi.Models.Entity;
using PokemonApi.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using System.Net.Http;
using PokemonApi.ExternalApi;
using PokemonApi.Models.Dto;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Moq.Protected;

namespace PokemonApi.Tests.IntegrationTests
{
    public class PokemonMasterServiceIntegrationTests
    {
        private DbContextOptions<AppDbContext> GetInMemoryDatabaseOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
        }

        private Mock<HttpMessageHandler> GetMockHttpMessageHandler(string jsonResponse)
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });
            return mockHandler;
        }

        [Fact]
        public async Task CapturePokemonAsync_IntegrationTest_SavesCaptureInDatabase()
        {
            // Arrange: Configuração do banco de dados em memória e criação de dependências
            var options = GetInMemoryDatabaseOptions();
            var context = new AppDbContext(options);
            var pokemonMasterRepository = new PokemonMasterRepository(context);
            var captureRepository = new CaptureRepository(context);

            // Configurando o mock da API externa para retornar um Pokémon
            var mockHttpClient = GetMockHttpMessageHandler("{\"name\":\"pikachu\",\"id\":25}");
            var httpClient = new HttpClient(mockHttpClient.Object);

            var pokemonService = new PokemonService(new PokemonApiClient(httpClient), new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions()));

            var service = new PokemonMasterService(
                pokemonMasterRepository, captureRepository, pokemonService
            );

            // Criando um PokemonMaster no banco de dados
            var pokemonMaster = new PokemonMaster { Id = 1, Name = "Ash Ketchum", Cpf = "12345678900", Age = 10 };
            await pokemonMasterRepository.Save(pokemonMaster);

            // Definindo os dados de captura
            var captureRequest = new CaptureRequest
            {
                PokemonMasterName = "Ash Ketchum",
                PokemonName = "Pikachu"
            };

            // Act: Chamando o método que realiza a captura
            await service.CapturePokemonAsync(captureRequest);

            // Assert: Verificando se a captura foi salva no banco de dados
            var capturedPokemon = await captureRepository.GetByMasterIdAndPokemonNameAsync(1, "Pikachu");

            Assert.NotNull(capturedPokemon); // Verifica se a captura foi realizada
            Assert.Equal("Pikachu", capturedPokemon.PokemonName); // Verifica o nome do Pokémon
        }
    }
}