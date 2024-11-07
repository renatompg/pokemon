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
        /// <returns>Lista de detalhes de Pokémon.</returns>
        [HttpGet("random")]
        [SwaggerResponse(200, "Lista de Pokémon aleatórios obtida com sucesso.", typeof(IEnumerable<PokemonDetail>))]
        [SwaggerOperation(Summary = "Obter Pokémon aleatórios", Description = "Retorna uma lista de Pokémon com detalhes básicos.")]
        public async Task<ActionResult<IEnumerable<PokemonDetail>>> GetRandomPokemonsAsync()
        {
            var pokemons = await _pokemonService.GetRandomPokemonAsync();
            return Ok(pokemons);
        }

        /// <summary>
        /// Obtém detalhes de um Pokémon específico pelo nome.
        /// </summary>
        /// <param name="name">Nome do Pokémon.</param>
        /// <returns>Detalhes do Pokémon.</returns>
        [HttpGet("{name}")]
        [SwaggerResponse(200, "Detalhes do Pokémon obtidos com sucesso.", typeof(PokemonSummary))]
        [SwaggerResponse(404, "Pokémon não encontrado.")]
        [SwaggerOperation(Summary = "Obter Pokémon por nome", Description = "Retorna detalhes de um Pokémon específico.")]
        public async Task<ActionResult<PokemonSummary>> GetPokemonByNameAsync(string name)
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
        /// <returns>Lista de Pokémon capturados.</returns>
        [HttpGet("master/capture")]
        [SwaggerResponse(200, "Lista de Pokémon capturados obtida com sucesso.", typeof(IEnumerable<PokemonMaster>))]
        [SwaggerResponse(400, "Erro ao obter a lista de Pokémon capturados.")]
        [SwaggerOperation(Summary = "Obter todos os Pokémon capturados", Description = "Retorna uma lista com todos os Pokémon capturados.")]
        public async Task<IActionResult> GetAllCapturedPokemonsAsync()
        {
            try
            {
                return Ok(await _pokemonMasterService.GetAllCapturedPokemonsAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
