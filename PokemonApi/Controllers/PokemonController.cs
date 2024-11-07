using Microsoft.AspNetCore.Mvc;
using PokemonApi.Models.Dto;
using PokemonApi.Models.Entity;
using PokemonApi.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace PokemonApi.Controllers
{
    [ApiController]
    [Route("api/pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;
        private readonly IPokemonMasterService _pokemonMasterService;

        public PokemonController(IPokemonService pokemonService, IPokemonMasterService pokemonMasterService)
        {
            _pokemonService = pokemonService;
            _pokemonMasterService = pokemonMasterService;
        }

        /// <summary>
        /// Obtém uma lista de Pokémon aleatórios.
        /// </summary>
        /// <returns>Lista de Pokémons com links para detalhes adicionais de cada Pokémon.</returns>
        /// <remarks>
        /// Este endpoint implementa o padrão HATEOAS ao adicionar links para os detalhes de cada Pokémon.
        /// Para cada Pokémon na lista retornada, a URL do recurso detalhado do Pokémon (detalhes por nome) é incluída
        /// na propriedade 'Url'. Isso permite que o cliente navegue facilmente para o recurso de detalhes de cada Pokémon
        /// diretamente a partir da resposta.
        ///
        /// Como no projeto há a definição de retornar dados detalhados do Pokémon, como imagem em base64 e evoluções,
        /// e devido ao custo de gerar esses dados para uma lista inteira, optou-se por aplicar o padrão HATEOAS.
        /// Dessa forma, garantimos que o cliente possa acessar os detalhes do Pokémon quando necessário, sem gerar sobrecarga
        /// </remarks>
        [HttpGet("random")]
        [SwaggerResponse(200, "Lista de Pokémon aleatórios obtida com sucesso.", typeof(IEnumerable<PokemonSummary>))]
        [SwaggerOperation(Summary = "Obter Pokémon aleatórios", Description = "Retorna uma lista de Pokémon com detalhes básicos.")]
        public async Task<ActionResult<IEnumerable<PokemonSummary>>> GetRandomPokemonsAsync()
        {
            var pokemons = await _pokemonService.GetRandomPokemonAsync();

            foreach (var pokemon in pokemons)
            {
                // O HATEOAS é implementado ao adicionar a URL do recurso de detalhes para cada Pokémon
                pokemon.Url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/api/pokemon/{pokemon.Name}";
            }

            return Ok(pokemons);
        }


        /// <summary>
        /// Obtém detalhes de um Pokémon específico pelo nome.
        /// </summary>
        /// <param name="name">Nome do Pokémon.</param>
        /// <returns>Detalhes do Pokémon.</returns>
        [HttpGet("{name}")]
        [SwaggerResponse(200, "Detalhes do Pokémon obtidos com sucesso.", typeof(PokemonDetail))]
        [SwaggerResponse(404, "Pokémon não encontrado.")]
        [SwaggerOperation(Summary = "Obter Pokémon por nome", Description = "Retorna detalhes de um Pokémon específico.")]
        public async Task<ActionResult<PokemonDetail>> GetPokemonByNameAsync(string name)
        {
            var pokemon = await _pokemonService.GetPokemonByNameAsync(name);
            if (pokemon == null)
                return NotFound();

            return Ok(pokemon);
        }

        /// <summary>
        /// Obtém o registro mestre de um Pokémon específico pelo nome.
        /// </summary>
        /// <param name="name">Nome do Pokémon.</param>
        /// <returns>Registro mestre do Pokémon.</returns>
        [HttpGet("master/{name}")]
        [SwaggerResponse(200, "Registro mestre do Pokémon obtido com sucesso.", typeof(PokemonMaster))]
        [SwaggerResponse(404, "Registro mestre do Pokémon não encontrado.")]
        [SwaggerOperation(Summary = "Obter registro mestre do Pokémon", Description = "Retorna o registro mestre de um Pokémon específico.")]
        public async Task<ActionResult<PokemonMaster>> GetPokemonMasterByNameAsync(string name)
        {
            var pokemon = await _pokemonMasterService.GetByName(name);
            if (pokemon == null)
                return NotFound();

            return Ok(pokemon);
        }

        /// <summary>
        /// Cria um novo registro mestre para um Pokémon.
        /// </summary>
        /// <param name="pokemonMaster">Objeto contendo os dados do registro mestre do Pokémon.</param>
        [HttpPost("master")]
        [SwaggerResponse(200, "Registro mestre do Pokémon criado com sucesso.")]
        [SwaggerOperation(Summary = "Criar registro mestre do Pokémon", Description = "Cria um novo registro mestre para um Pokémon.")]
        public async Task<IActionResult> CreatePokemonMasterAsync([FromBody] PokemonMaster pokemonMaster)
        {
            await _pokemonMasterService.Save(pokemonMaster);
            return Ok();
        }

        /// <summary>
        /// Captura um Pokémon.
        /// </summary>
        /// <param name="captureRequest">Objeto contendo os dados da captura do Pokémon.</param>
        [HttpPost("master/capture")]
        [SwaggerResponse(200, "Pokémon capturado com sucesso.")]
        [SwaggerResponse(400, "Erro ao capturar o Pokémon.")]
        [SwaggerOperation(Summary = "Capturar Pokémon", Description = "Registra a captura de um Pokémon específico.")]
        public async Task<IActionResult> CapturePokemonAsync([FromBody] CaptureRequest captureRequest)
        {
            try
            {
                await _pokemonMasterService.CapturePokemonAsync(captureRequest);
                return Ok("Pokémon capturado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtém todos os Pokémon capturados.
        /// </summary>
        /// <returns>Lista de Pokémon capturados com links para detalhes.</returns>
        /// <remarks>
        /// Este endpoint utiliza o padrão HATEOAS ao adicionar links para os detalhes de cada Pokémon e Pokémon Master.
        /// Cada Pokémon capturado retorna a URL do recurso detalhado, tanto para o Pokémon quanto para o Pokémon Master.
        /// O uso do padrão HATEOAS proporciona melhor navegabilidade na API.
        /// </remarks>
        [HttpGet("master/capture")]
        [SwaggerResponse(200, "Lista de Pokémon capturados obtida com sucesso.", typeof(IEnumerable<PokemonMaster>))]
        [SwaggerResponse(400, "Erro ao obter a lista de Pokémon capturados.")]
        [SwaggerOperation(Summary = "Obter todos os Pokémon capturados", Description = "Retorna uma lista com todos os Pokémon capturados.")]
        public async Task<IActionResult> GetAllCapturedPokemonsAsync()
        {
            try
            {
                // Obtendo todos os Pokémon capturados
                var capturedPokemons = await _pokemonMasterService.GetAllCapturedPokemonsAsync();

                // Verificando se há Pokémon capturados e adicionando os links HATEOAS
                if (capturedPokemons == null || !capturedPokemons.Any())
                {
                    return Ok(Enumerable.Empty<CapturedPokemon>());
                }

                // Adicionando os links HATEOAS para cada Pokémon capturado
                var result = capturedPokemons.Select(capturedPokemon =>
                {
                    capturedPokemon.PokemonMasterUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/api/pokemon/master/{capturedPokemon.PokemonMasterName}";
                    capturedPokemon.PokemonUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/api/pokemon/{capturedPokemon.PokemonName}";
                    return capturedPokemon;
                }).ToList();

                // Retornando a lista de Pokémon capturados
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Tratando exceções e retornando erro de requisição
                // Considerar logar o erro para facilitar o diagnóstico
                return BadRequest(ex.Message);
            }
        }
    }
}
