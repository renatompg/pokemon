using PokemonApi.Models.Dto;

namespace PokemonApi.ExternalApi
{
    public interface IPokemonApiClient
    {
        Task<PokemonDetail> GetPokemonByNameAsync(string name);
        Task<IEnumerable<PokemonSummary>> GetRandomPokemonAsync();
        Task<string> DownloadSpriteImageAsBase64(string uri);
    }
}