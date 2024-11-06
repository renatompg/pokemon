using Microsoft.EntityFrameworkCore;
using PokemonApi.Models.Entity;

namespace PokemonApi.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<PokemonMaster> PokemonMasters { get; set; }
        public DbSet<Capture> Captures { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=pokemon.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonMaster>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<PokemonMaster>()
                .HasIndex(p => new { p.Name, p.Cpf })
                .IsUnique();

            modelBuilder.Entity<Capture>()
                .HasKey(c => c.Id);  

            modelBuilder.Entity<Capture>()
                .HasIndex(c => new { c.PokemonMasterId, c.PokemonName })
                .IsUnique();  

            modelBuilder.Entity<Capture>()
                .HasOne(c => c.PokemonMaster)
                .WithMany()
                .HasForeignKey(c => c.PokemonMasterId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
