using System;
using System.Collections.Generic;
using System.Linq;

public class MenuService
{
    public static MenuService Instance { get; } = new();
    private static PokemonService _Service { get; } = PokemonService.Instance;
    private List<Pokemon> _Pokemons { get; } = _Service == null ? new() : _Service.Pokemons;
    private Dictionary<string, Action> Functions { get; }

    public MenuService()
    {
        Functions = new() {
            { "(1) Add pokemon to my pocket", () => AddPokemon() },
            { "(2) List pokemon(s) in my pocket", () => ShowPokemons() },
            { "(3) Check if I can evolve pokemon", () => Console.WriteLine("Option 3") },
            { "(4) Evolve pokemon", () => Console.WriteLine("Option 4") },
        };
    }

    public void Display()
    {
        Console.WriteLine();
        Console.WriteLine("*****************************");
        Console.WriteLine("Welcome to Pokemon Pocket App");
        Console.WriteLine("*****************************");
        foreach (var item in Functions) Console.WriteLine(item.Key);

        char input = GetInput("Please only enter [1,2,3,4] or Q to quit: ");
        if (char.ToLower(input).Equals('q')) return;
        RunFunction((int)input - 48);
    }

    public dynamic? GetInput(string question)
    {
        try
        {
            Console.Write($"{question.Substring(0, 1) + question.Substring(1)}: ");
            dynamic input = Console.ReadKey().KeyChar;
            Console.WriteLine();
            return input;
        }
        catch (Exception error)
        {
            Console.WriteLine(error.Message);
            return null;
        }
    }

    public void RunFunction(int index)
    {
        if (index >= 0 && index <= Functions.Count()) Functions.ElementAt(index - 1).Value();
        Console.WriteLine("That is not one of the options, please try again or enter Q to quit.");
    }

    public void ShowPokemons()
    {
        List<Pokemon> pokemons = _Service.Pokemons;

        char sortList = GetInput("Would you like the list to be sorted? (y/n): ");
        if (char.ToLower(sortList).Equals('n')) goto Display;

        char sortAttr = GetInput("How would you like the list to be sorted by:\n(1) Name\n(2) Hp\n(3) Exp\nEnter a number: ");
        if (char.ToLower(sortAttr).Equals('n')) goto Display;

        pokemons = (sortAttr) switch
        {
            '1' => pokemons.OrderBy(pokemon => pokemon.Name).ToList(),
            '2' => pokemons.OrderBy(pokemon => pokemon.Hp).ToList(),
            '3' => pokemons.OrderBy(pokemon => pokemon.Exp).ToList(),
            _ => pokemons
        };

    Display:
        Console.WriteLine("\n=== Pokemons in Pocket ===");
        if (_Pokemons.Count() == 0) Console.WriteLine("Pocket is currently empty, add a pokemon to see it!");
        else foreach (Pokemon pokemon in pokemons) Console.WriteLine(pokemon.ToString());
    }

    private void AddPokemon()
    {
        Pokemon? toAdd = null;

        while (true)
        {
            if (toAdd != null) goto NameEntered;

            Console.Write("Enter Pokemon's Name: ");
            string? name = Console.ReadLine();

            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Name cannot be empty.\nTry again, or enter 0 to cancel.\n");
                continue;
            }

            if (name.Contains("cancel")) break;

            name = name.ToLower();

            if (!_Service.ValidPokemons.Contains(name))
            {
                Console.WriteLine("Invalid pokemon name.\nTry again, or enter 0 to cancel.\n");
                continue;
            }

            toAdd = name switch
            {
                "pikachu" => new Pikachu(),
                "eevee" => new Eevee(),
                "charmander" => new Charmander(),
                _ => new Pokemon("invalid")
            };

        NameEntered:
            if (toAdd.Hp != 0) goto HpEntered;

            Console.Write("Enter Pokemon's HP: ");
            string? hp = Console.ReadLine();

            if (hp!.Equals("cancel")) break;

            if (!int.TryParse(hp, out int intHp))
            {
                Console.WriteLine("Hp must be a valid number.\nTry again, or enter 0 to cancel.\n");
                continue;
            }

            toAdd.Hp = intHp;

        HpEntered:
            Console.Write("Enter Pokemon's Exp: ");
            string? exp = Console.ReadLine();

            if (exp!.Equals("cancel")) break;

            if (!int.TryParse(exp, out int intExp))
            {
                Console.WriteLine("Exp must be a valid number.\nTry again, or enter 0 to cancel.\n");
                continue;
            }

            toAdd.Exp = intExp;

            if (!_Service.AddPokemon(toAdd)) Console.WriteLine("Something went wrong, please try again.\n");
            else Console.WriteLine($"{toAdd.Name} added successfully!\n");

            ShowPokemons();
            break;
        }
    }
}