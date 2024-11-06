
using PokemonApi.Models.Dto;

namespace PokemonApi.Services
{
    public interface IPokemonService
    {
        Task<PokemonDetail> GetPokemonByNameAsync(string name);
        Task<IEnumerable<PokemonSummary>> GetRandomPokemonAsync();
        Task<string> DownloadSpriteImageAsBase64(string uri);
    }
}