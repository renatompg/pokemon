using Microsoft.Extensions.Caching.Memory;
using PokemonApi.ExternalApi;
using PokemonApi.Models.Dto;

namespace PokemonApi.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonApiClient _pokemonApiClient;
        private readonly IMemoryCache _cache;

        public PokemonService(IPokemonApiClient pokemonApiClient, IMemoryCache cache)
        {
            _pokemonApiClient = pokemonApiClient;
            _cache = cache;
        }
        public async Task<PokemonDetail> GetPokemonByNameAsync(string name)
        {
            if (_cache.TryGetValue(name, out PokemonDetail pokemon))
                return pokemon;

            pokemon = await _pokemonApiClient.GetPokemonByNameAsync(name);
            _cache.Set(name, pokemon, TimeSpan.FromMinutes(10));
            return pokemon;
        }

        public async Task<IEnumerable<PokemonSummary>> GetRandomPokemonAsync()
        {
            const string cacheKey = "randomPokemons";
            if (_cache.TryGetValue(cacheKey, out IEnumerable<PokemonSummary> randomPokemons))
                return randomPokemons;

            randomPokemons = await _pokemonApiClient.GetRandomPokemonAsync();
            _cache.Set(cacheKey, randomPokemons, TimeSpan.FromMinutes(10));
            return randomPokemons;
        }

        public async Task<string> DownloadSpriteImageAsBase64(string uri)
        {
            if (_cache.TryGetValue(uri, out string cachedBase64Image))
            {
                return cachedBase64Image;
            }

            string base64Image = await _pokemonApiClient.DownloadSpriteImageAsBase64(uri);

            _cache.Set(uri, base64Image, TimeSpan.FromMinutes(10));

            return base64Image;
        }
    }
}