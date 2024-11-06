using Microsoft.EntityFrameworkCore;
using PokemonApi.Models.Entity;

namespace PokemonApi.Database
{
    public class PokemonMasterRepository : IPokemonMasterRepository
    {
        private readonly AppDbContext _context;

        public PokemonMasterRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PokemonMaster?> GetByName(string name)
        {
            return await _context.PokemonMasters.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task Save(PokemonMaster pokemonMaster)
        {
            _context.PokemonMasters.Add(pokemonMaster);
            await _context.SaveChangesAsync();
        }
    }
}