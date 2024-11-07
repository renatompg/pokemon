using PokemonApi.Database;
using PokemonApi.Models.Dto;
using PokemonApi.Models.Entity;

namespace PokemonApi.Services
{
    public class PokemonMasterService : IPokemonMasterService
    {
        private readonly IPokemonMasterRepository _pokemonMasterRepository;
        private readonly ICaptureRepository _captureRepository;
        private readonly IPokemonService _pokemonService;

        public PokemonMasterService(IPokemonMasterRepository pokemonMasterRepository, ICaptureRepository captureRepository, IPokemonService pokemonService)
        {
            _pokemonMasterRepository = pokemonMasterRepository;
            _captureRepository = captureRepository;
            _pokemonService = pokemonService;
        }
        public async Task<PokemonMaster?> GetByName(string name)
        {
            return await _pokemonMasterRepository.GetByName(name);
        }

        public async Task Save(PokemonMaster pokemonMaster)
        {
            await _pokemonMasterRepository.Save(pokemonMaster);
        }

        public async Task CapturePokemonAsync(CaptureRequest captureRequest)
        {
            var capture = await ValidateCaptureAsync(captureRequest);

            await _captureRepository.SaveAsync(capture);
        }

        private async Task<Capture> ValidateCaptureAsync(CaptureRequest captureRequest)
        {
            var pokemonMaster = await _pokemonMasterRepository.GetByName(captureRequest.PokemonMasterName);
            if (pokemonMaster == null)
            {
                throw new Exception("Pokemon Master não encontrado.");
            }

            var pokemon = await _pokemonService.GetPokemonByNameAsync(captureRequest.PokemonName);
            if (pokemon == null)
            {
                throw new Exception("Pokémon não encontrado na API externa.");
            }

            var existingCapture = await _captureRepository.GetByMasterIdAndPokemonNameAsync(pokemonMaster.Id, captureRequest.PokemonName);
            if (existingCapture != null)
            {
                throw new Exception("Este Pokémon já foi capturado por este mestre.");
            }

            return new Capture
            {
                PokemonMasterId = pokemonMaster.Id,
                PokemonName = captureRequest.PokemonName
            };
        }

        public async Task<IEnumerable<CapturedPokemon>> GetAllCapturedPokemonsAsync()
        {
            var capturedPokemon = await _captureRepository.GetAllCapturedPokemonsAsync();

            return capturedPokemon?.Where(c => c.PokemonMaster != null)
                                    .Select(c => new CapturedPokemon
                                    {
                                        PokemonMasterName = c.PokemonMaster.Name,
                                        PokemonName = c.PokemonName
                                    })
                                    ?? Enumerable.Empty<CapturedPokemon>();
        }
    }
}