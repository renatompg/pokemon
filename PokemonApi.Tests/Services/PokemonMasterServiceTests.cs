using Moq;
using PokemonApi.Database;
using PokemonApi.Models.Dto;
using PokemonApi.Models.Entity;
using PokemonApi.Services;
using Capture = PokemonApi.Models.Entity.Capture;


namespace PokemonApi.Tests.Services
{
    public class PokemonMasterServiceTests
    {

        private readonly Mock<IPokemonMasterRepository> _pokemonMasterRepositoryMock;
        private readonly Mock<ICaptureRepository> _captureRepositoryMock;
        private readonly Mock<IPokemonService> _pokemonServiceMock;
        private readonly PokemonMasterService _pokemonMasterService;

        public PokemonMasterServiceTests()
        {
            _pokemonMasterRepositoryMock = new Mock<IPokemonMasterRepository>();
            _captureRepositoryMock = new Mock<ICaptureRepository>();
            _pokemonServiceMock = new Mock<IPokemonService>();

            _pokemonMasterService = new PokemonMasterService(
                _pokemonMasterRepositoryMock.Object,
                _captureRepositoryMock.Object,
                _pokemonServiceMock.Object);
        }

        [Fact]
        public async Task GetByName_ReturnsPokemonMaster_WhenFound()
        {
            // Arrange
            var pokemonMaster = new PokemonMaster { Id = 1, Name = "Ash Ketchum" };
            _pokemonMasterRepositoryMock.Setup(repo => repo.GetByName(It.IsAny<string>()))
                .ReturnsAsync(pokemonMaster);

            // Act
            var result = await _pokemonMasterService.GetByName("Ash Ketchum");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Ash Ketchum", result.Name);
        }

        [Fact]
        public async Task GetByName_ReturnsNull_WhenNotFound()
        {
            // Arrange
            _pokemonMasterRepositoryMock.Setup(repo => repo.GetByName(It.IsAny<string>()))
                .ReturnsAsync((PokemonMaster)null);

            // Act
            var result = await _pokemonMasterService.GetByName("Nonexistent Name");

            // Assert
            Assert.Null(result);
        }

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
                .ReturnsAsync((Capture)null); 

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
