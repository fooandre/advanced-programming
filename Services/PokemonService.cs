// === Author ===
// Name: Andre Foo
// AdminNo: 210119U 

using System;
using System.Collections.Generic;
using System.Linq;

public class PokemonService {
    public static PokemonService Instance { get; } = new();
    private List<PokemonMaster> _PokemonMasters { get; } = new() {
        new PokemonMaster { Name = "Pichu", NoToEvolve = 1, EvolveTo = "Pikachu" },
        new PokemonMaster { Name = "Pikachu", NoToEvolve = 2, EvolveTo = "Raichu" },
        new PokemonMaster { Name = "Eevee", NoToEvolve = 3, EvolveTo = "Flareon" },
        new PokemonMaster { Name = "Charmander", NoToEvolve = 1, EvolveTo = "Charmeleon" },
        new PokemonMaster { Name = "Charmeleon", NoToEvolve = 2, EvolveTo = "Charizard" },
    };

    public List<Pokemon> GetPokemons() { using (PokemonContext dbctx = new()) return dbctx.Pokemons.OrderBy(p => p.Id).ToList(); }

    public Boolean AddPokemon(Pokemon pokemon) {
        try {
            using (PokemonContext dbctx = new()) {
                int pokemonCount = GetPokemons().Count();
                pokemon.Id = pokemonCount == 0 ? 1 : GetPokemons().LastOrDefault()!.Id + 1;

                string name = pokemon.Name;
                name = name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
                pokemon.Name = name;

                dbctx.Pokemons.Add(pokemon);
                dbctx.SaveChanges();
                return true;
            }
        } catch (Exception) {
            return false;
        }
    }

    public Boolean ReplacePokemon(int id, Pokemon pokemon) {
        using (PokemonContext dbctx = new()) {
            try {
                var toReplace = dbctx.Pokemons.FirstOrDefault(p => p.Id == id)!;
                toReplace.Name = pokemon.Name;
                toReplace.Hp = pokemon.Hp;
                toReplace.Exp = pokemon.Exp;
                toReplace.Skill = pokemon.Skill;
                dbctx.SaveChanges();
                return true;
            } catch (Exception) {
                return false;
            }
        }
    }

    public Boolean EditPokemon(int id, string attribute, dynamic newValue) {
        using (PokemonContext dbctx = new()) {
            try {
                var toEdit = dbctx.Pokemons.FirstOrDefault(p => p.Id == id)!;

                switch (attribute.Trim().ToLower()) {
                case "name":
                    toEdit.Name = newValue;
                    break;
                case "hp":
                    toEdit.Hp = newValue;
                    break;
                case "exp":
                    toEdit.Exp = newValue;
                    break;
                default:
                    Console.WriteLine($"{attribute} is not a valid attribute");
                    return false;
                }

                dbctx.SaveChanges();
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }

    public Boolean RemovePokemon(Pokemon pokemon) {
        try {
            using (PokemonContext dbctx = new()) {
                dbctx.Pokemons.Remove(pokemon);
                dbctx.SaveChanges();
                return true;
            }
        } catch (Exception) {
            return false;
        }
    }

    public Dictionary<Boolean, List<PokemonMaster>> CheckToEvolve() {
        var groupedByName = GetPokemons().GroupBy(p => p.Name).ToList();
        List<PokemonMaster> canEvolve = new();
        List<PokemonMaster> cannotEvolve = new();

        foreach (var g in groupedByName) {
            PokemonMaster? pokemonMaster = _PokemonMasters.FirstOrDefault(pm => pm.Name.Equals(g.Key));
            if (pokemonMaster == null) continue;

            List<PokemonMaster> addTo = g.Count() < pokemonMaster.NoToEvolve ? cannotEvolve : canEvolve;
            addTo.Add(pokemonMaster);
        }

        return new Dictionary<Boolean, List<PokemonMaster>>()
        {
            { true, canEvolve },
            { false, cannotEvolve }
        };
    }
}