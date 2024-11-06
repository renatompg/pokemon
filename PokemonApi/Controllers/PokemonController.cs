using Microsoft.AspNetCore.Mvc;
using PokemonApi.Models.Dto;
using PokemonApi.Models.Entity;
using PokemonApi.Services;

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

        [HttpGet("random")]
        public async Task<ActionResult<IEnumerable<PokemonDetail>>> GetRandomPokemonsAsync()
        {
            var pokemons = await _pokemonService.GetRandomPokemonAsync();
            return Ok(pokemons);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<PokemonSummary>> GetPokemonByNameAsync(string name)
        {
            var pokemon = await _pokemonService.GetPokemonByNameAsync(name);
            if (pokemon == null)
                return NotFound();

            return Ok(pokemon);
        }

        [HttpGet("master/{name}")]
        public async Task<ActionResult<PokemonMaster>> GetPokemonMasterByNameAsync(string name)
        {
            var pokemon = await _pokemonMasterService.GetByName(name);
            if (pokemon == null)
                return NotFound();

            return Ok(pokemon);
        }

        [HttpPost("master")]
        public async Task<IActionResult> CreatePokemonMasterAsync([FromBody] PokemonMaster pokemonMaster)
        {
            await _pokemonMasterService.Save(pokemonMaster);
            return Ok();
        }

        [HttpPost("master/capture")]
        public async Task<IActionResult> CapturePokemon([FromBody] CaptureRequest captureRequest)
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

         [HttpGet("sprites/base64/{uri}")]
         public async Task<IActionResult> GetImageBase64(string uri)
         {
             return Ok(new { ImageBase64 = await _pokemonService.DownloadSpriteImageAsBase64(uri) });
         }
    }
}
