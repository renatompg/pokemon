using PokemonApi.Models.Entity;

namespace PokemonApi.Database
{
    public interface IPokemonMasterRepository
    {
        Task<PokemonMaster?> GetByName(string name);
        Task Save(PokemonMaster pokemonMaster);
    }
}