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
            // Validar a captura do Pokémon e obter o objeto Capture
            var capture = await ValidateCaptureAsync(captureRequest);

            // 5. Salvar no repositório de Capture
            await _captureRepository.SaveAsync(capture);
        }

        private async Task<Capture> ValidateCaptureAsync(CaptureRequest captureRequest)
        {
            // 1. Buscar o PokemonMaster pelo nome
            var pokemonMaster = await _pokemonMasterRepository.GetByName(captureRequest.PokemonMasterName);
            if (pokemonMaster == null)
            {
                throw new Exception("Pokemon Master não encontrado.");
            }

            // 2. Buscar o Pokémon na API externa
            var pokemon = await _pokemonService.GetPokemonByNameAsync(captureRequest.PokemonName);
            if (pokemon == null)
            {
                throw new Exception("Pokémon não encontrado na API externa.");
            }

            // 3. Verificar se o Pokémon já foi capturado por esse mestre
            var existingCapture = await _captureRepository.GetByMasterIdAndPokemonNameAsync(pokemonMaster.Id, captureRequest.PokemonName);
            if (existingCapture != null)
            {
                throw new Exception("Este Pokémon já foi capturado por este mestre.");
            }

            // 4. Criar e retornar a entidade Capture
            return new Capture
            {
                PokemonMasterId = pokemonMaster.Id,
                PokemonName = captureRequest.PokemonName
            };
        }
    }
}