namespace PokemonApi.Models.Entity
{
    public class Capture
    {
        public int Id { get; set; }
        public int PokemonMasterId { get; set; }
        public string PokemonName { get; set; }
        public PokemonMaster PokemonMaster { get; set; }
    }
}
