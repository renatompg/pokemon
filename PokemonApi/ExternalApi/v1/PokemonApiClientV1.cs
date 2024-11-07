using Newtonsoft.Json;
using PokemonApi.Models.Dto;

namespace PokemonApi.ExternalApi
{
    public class PokemonApiClientV1 : IPokemonApiClient
    {
        private readonly HttpClient _httpClient;

        public PokemonApiClientV1(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<PokemonDetail> GetPokemonByNameAsync(string name)
        {
            var response = await _httpClient.GetStringAsync($"https://pokeapi.co/api/v2/pokemon/{name}");
            var pokemonDetail = JsonConvert.DeserializeObject<PokemonDetail>(response);

            if (pokemonDetail?.Sprites is { } sprites)
            {
                sprites.BackDefault64 = !string.IsNullOrEmpty(sprites.BackDefault)
                    ? await DownloadSpriteImageAsBase64(sprites.BackDefault)
                    : null;

                sprites.FrontDefault64 = !string.IsNullOrEmpty(sprites.FrontDefault)
                    ? await DownloadSpriteImageAsBase64(sprites.FrontDefault)
                    : null;
            }

            return pokemonDetail ?? new PokemonDetail();
        }

        public async Task<IEnumerable<PokemonSummary>> GetRandomPokemonAsync()
        {
            var response = await _httpClient.GetStringAsync("https://pokeapi.co/api/v2/pokemon?limit=10&offset=0");
            var pagedResponse = JsonConvert.DeserializeObject<PagedPokemonResponse>(response);
            return pagedResponse?.Results ?? Enumerable.Empty<PokemonSummary>();
        }

        public async Task<string> DownloadSpriteImageAsBase64(string uri)
        {
            var imageBytes = await _httpClient.GetByteArrayAsync(uri);
            return Convert.ToBase64String(imageBytes);
        }
    }
}
