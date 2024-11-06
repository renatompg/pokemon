using Moq;
using PokemonApi.Database;
using PokemonApi.Models.Dto;
using PokemonApi.Models.Entity;
using PokemonApi.Services;
using Xunit;
using Capture = PokemonApi.Models.Entity.Capture;


namespace PokemonApi.Tests.Services
{
    public class PokemonMasterServiceTests
    {
        [Fact]
        public async Task CapturePokemonAsync_Success_CapturesPokemon()
        {
            // Arrange
            var pokemonMasterRepositoryMock = new Mock<IPokemonMasterRepository>();
            var captureRepositoryMock = new Mock<ICaptureRepository>();
            var pokemonServiceMock = new Mock<IPokemonService>();

            var captureRequest = new CaptureRequest
            {
                PokemonMasterName = "Ash Ketchum",
                PokemonName = "Pikachu"
            };

            var pokemonMaster = new PokemonMaster { Id = 1, Name = "Ash Ketchum" };
            var pokemon = new PokemonDetail { Name = "Pikachu" };

            pokemonMasterRepositoryMock
                .Setup(repo => repo.GetByName(captureRequest.PokemonMasterName))
                .ReturnsAsync(pokemonMaster);

            pokemonServiceMock
                .Setup(service => service.GetPokemonByNameAsync(captureRequest.PokemonName))
                .ReturnsAsync(pokemon);

            captureRepositoryMock
                .Setup(repo => repo.GetByMasterIdAndPokemonNameAsync(pokemonMaster.Id, pokemon.Name))
                .ReturnsAsync((Capture)null);  // Nenhuma captura existente

            var service = new PokemonMasterService(
                pokemonMasterRepositoryMock.Object,
                captureRepositoryMock.Object,
                pokemonServiceMock.Object
            );

            // Act
            await service.CapturePokemonAsync(captureRequest);

            // Assert
            captureRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<Capture>()), Times.Once);
        }
    }
}
