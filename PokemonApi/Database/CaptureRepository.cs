using Microsoft.EntityFrameworkCore;
using PokemonApi.Models.Entity;

namespace PokemonApi.Database
{
    public class CaptureRepository : ICaptureRepository
    {
        private readonly AppDbContext _context;

        public CaptureRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(Capture capture)
        {
            _context.Captures.Add(capture);
            await _context.SaveChangesAsync();
        }

        public async Task<Capture?> GetByMasterIdAndPokemonNameAsync(int pokemonMasterId, string pokemonName)
        {
            return await _context.Captures
                .FirstOrDefaultAsync(c => c.PokemonMasterId == pokemonMasterId && c.PokemonName == pokemonName);
        }

        public async Task<IEnumerable<Capture>> GetByMasterIdAsync(int pokemonMasterId)
        {
            return await _context.Captures
                .Where(c => c.PokemonMasterId == pokemonMasterId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Capture>> GetAllCapturedPokemonsAsync()
        {
            return await _context.Captures
                .Include(c => c.PokemonMaster)
                .ToListAsync();
        }
    }
}
