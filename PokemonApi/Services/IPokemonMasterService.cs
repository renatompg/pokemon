using PokemonApi.Models.Dto;
using PokemonApi.Models.Entity;

namespace PokemonApi.Services
{
    public interface IPokemonMasterService
    {
        Task<PokemonMaster?> GetByName(string name);
        Task Save(PokemonMaster pokemonMaster);
        Task CapturePokemonAsync(CaptureRequest captureRequest);
        Task<IEnumerable<CapturedPokemon>> GetAllCapturedPokemonsAsync();
    }
}