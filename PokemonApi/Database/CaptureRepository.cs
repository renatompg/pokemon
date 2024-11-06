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

        // Salva uma nova captura
        public async Task SaveAsync(Capture capture)
        {
            _context.Captures.Add(capture);
            await _context.SaveChangesAsync();
        }

        // Busca uma captura específica de acordo com o PokemonMasterId e PokemonName
        public async Task<Capture?> GetByMasterIdAndPokemonNameAsync(int pokemonMasterId, string pokemonName)
        {
            return await _context.Captures
                .FirstOrDefaultAsync(c => c.PokemonMasterId == pokemonMasterId && c.PokemonName == pokemonName);
        }

        // Lista todas as capturas de um mestre específico
        public async Task<IEnumerable<Capture>> GetByMasterIdAsync(int pokemonMasterId)
        {
            return await _context.Captures
                .Where(c => c.PokemonMasterId == pokemonMasterId)
                .ToListAsync();
        }
    }
}
