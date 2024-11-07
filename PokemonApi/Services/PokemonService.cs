using Microsoft.Extensions.Caching.Memory;
using PokemonApi.ExternalApi;
using PokemonApi.Models.Dto;

namespace PokemonApi.Services
{
    /// <summary>
    /// Serviço responsável por interagir com a API externa de Pokémon e fornecer detalhes
    /// dos Pokémon de forma eficiente, utilizando cache em memória para armazenar os resultados
    /// das requisições e evitar chamadas repetitivas à API externa.
    /// </summary>
    /// <remarks>
    /// A utilização de cache em memória visa melhorar a performance das requisições, evitando
    /// chamadas desnecessárias à API externa para dados que são frequentemente requisitados.
    /// Os dados são armazenados no cache por 10 minutos.
    /// 
    /// <note type="note">
    /// Caso seja necessário escalar o sistema para múltiplas instâncias ou ambientes distribuídos,
    /// deve-se considerar o uso de uma solução de cache distribuído, como o Redis, 
    /// para garantir que o cache seja compartilhado entre todas as instâncias da aplicação.
    /// </note>
    /// </remarks>
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
