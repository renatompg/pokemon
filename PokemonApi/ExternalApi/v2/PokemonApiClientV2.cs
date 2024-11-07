using Newtonsoft.Json;
using PokemonApi.Models.Dto;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

namespace PokemonApi.ExternalApi
{
    /// <summary>
    /// Versão 2 do cliente da API Pokémon, projetada para garantir resiliência
    /// e lidar com falhas de rede de forma robusta.
    /// </summary>
    /// <remarks>
    /// A implementação desta classe utiliza o pacote Polly para aplicar políticas de resiliência,
    /// incluindo retry, circuit breaker e timeout. Essas políticas ajudam a prevenir falhas
    /// temporárias e a manter a estabilidade do sistema.
    /// </remarks>
    public class PokemonApiClientV2 : IPokemonApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        private readonly AsyncTimeoutPolicy _timeoutPolicy;

        public PokemonApiClientV2(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // Retry Policy: Tenta novamente até 3 vezes em caso de falha, com um tempo de espera
            // exponencial entre as tentativas (2, 4 e 8 segundos).
            _retryPolicy = Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            // Circuit Breaker Policy: Interrompe as chamadas para a API por 1 minuto após 3 falhas consecutivas,
            // permitindo que o sistema se recupere.
            _circuitBreakerPolicy = Policy.Handle<HttpRequestException>()
                .CircuitBreakerAsync(3, TimeSpan.FromMinutes(1));

            // Timeout Policy: Define um tempo limite de 5 segundos para cada chamada à API.
            _timeoutPolicy = Policy.TimeoutAsync(5);
        }

        public async Task<PokemonDetail> GetPokemonByNameAsync(string name)
        {
            return await _retryPolicy
                .WrapAsync(_circuitBreakerPolicy)
                .WrapAsync(_timeoutPolicy)
                .ExecuteAsync(async () =>
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
                });
        }

        public async Task<IEnumerable<PokemonSummary>> GetRandomPokemonAsync()
        {
            return await _retryPolicy
                .WrapAsync(_circuitBreakerPolicy)
                .WrapAsync(_timeoutPolicy)
                .ExecuteAsync(async () =>
                {
                    var response = await _httpClient.GetStringAsync("https://pokeapi.co/api/v2/pokemon?limit=10&offset=0");
                    var pagedResponse = JsonConvert.DeserializeObject<PagedPokemonResponse>(response);
                    return pagedResponse?.Results ?? Enumerable.Empty<PokemonSummary>();
                });
        }

        public async Task<string> DownloadSpriteImageAsBase64(string uri)
        {
            return await _retryPolicy
                .WrapAsync(_circuitBreakerPolicy)
                .WrapAsync(_timeoutPolicy)
                .ExecuteAsync(async () =>
                {
                    var imageBytes = await _httpClient.GetByteArrayAsync(uri);
                    return Convert.ToBase64String(imageBytes);
                });
        }
    }
}
