namespace PokemonApi.Models.Dto
{
    public class PagedPokemonResponse
    {
        public int Count { get; set; }
        public string? Next { get; set; }
        public string? Previous { get; set; }
        public List<PokemonSummary> Results { get; set; }
    }
}