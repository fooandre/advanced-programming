using System;
using System.Collections.Generic;
using System.Linq;

public class PokemonService
{
    public static PokemonService Instance { get; } = new();
    public List<Pokemon> Pokemons { get; private set; } = new() {
        new Pikachu { Hp = 45, Exp = 22 },
        new Eevee { Hp = 54, Exp = 34 },
        new Charmander { Hp = 34, Exp = 21 },
    };
    //PokemonMaster list for checking pokemon evolution availability.    
    Dictionary<string, PokemonMaster> pokemonMasters = new() {
        { "Pikachu", new PokemonMaster { Name = "Pikachu", NoToEvolve = 2, EvolveTo = "Raichu" } },
        { "Eevee", new PokemonMaster { Name = "Eevee", NoToEvolve = 3, EvolveTo = "Flareon" } },
        { "Charmander", new PokemonMaster { Name = "Charmander", NoToEvolve = 1, EvolveTo = "Charmeleon" } },
    };

    public List<string> ValidPokemons { get; private set; } = new() { "pikachu", "eevee", "charmander" };

    public Boolean AddPokemon(Pokemon pokemon)
    {
        try
        {
            string name = pokemon.Name;
            name = name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
            pokemon.Name = name;

            Pokemons.Add(pokemon);
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    public Boolean RemovePokemon(Pokemon pokemon)
    {
        try
        {
            Pokemons.Remove(pokemon);
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    public List<Pokemon> ToEvolve()
    {
        List<Pokemon> res = new();
        foreach (var pm in pokemonMasters)
        {
            if (Pokemons.Where(p => p.Name.Equals(pm.Key)).Count() < pm.Value.NoToEvolve) continue;

            var toAdd = pm.Key switch
            {
                "Pikachu" => new Pikachu(),
                "Eevee" => new Eevee(),
                "Charmander" => new Charmander(),
                _ => new Pokemon("Invalid")
            };

            res.Add(toAdd);
        }

        return res;
    }
}