// === Author ===
// Name: Andre Foo
// AdminNo: 210119U 

using Microsoft.EntityFrameworkCore;

public class PokemonContext : DbContext {
    public DbSet<Pokemon> Pokemons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite("Filename=./Data/pokemons.db");
}