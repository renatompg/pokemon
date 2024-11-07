using PokemonApi.Models.Entity;

namespace PokemonApi.Database
{
    public interface ICaptureRepository
    {
        Task SaveAsync(Capture capture);
        Task<Capture?> GetByMasterIdAndPokemonNameAsync(int pokemonMasterId, string pokemonName);
        Task<IEnumerable<Capture>> GetByMasterIdAsync(int pokemonMasterId);
        Task<IEnumerable<Capture>>  GetAllCapturedPokemonsAsync();
    }
}