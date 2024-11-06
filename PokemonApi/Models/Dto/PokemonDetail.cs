namespace PokemonApi.Models.Dto
{
    public class PokemonDetail
    {
        public string Name { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public Sprites Sprites { get; set; }
    }
}