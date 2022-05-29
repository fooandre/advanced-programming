using System;
using System.Collections.Generic;
using System.Linq;

public class MenuService
{
    public static MenuService Instance { get; } = new();
    private static PokemonService _Service { get; } = PokemonService.Instance;
    private Dictionary<string, Action> Functions { get; }

    public MenuService()
    {
        Functions = new() {
            { "(1) List pokemon(s) in my pocket", ListPokemons },
            { "(2) Add pokemon to my pocket", AddPokemon },
            { "(3) Remove pokemon from my pocket", RemovePokemon },
            { "(4) Check if I can evolve pokemon", CheckToEvolve },
            { "(5) Evolve pokemon", EvolvePokemons },
            { "(6) Go to Hospital", Hospital },
            { "(7) Go to the Gym", Gym },
            { "(8) Go to the Trade Market", TradeMarket },
        };
    }

    public void Display()
    {
        Console.WriteLine();
        Console.WriteLine("*****************************");
        Console.WriteLine("Welcome to Pokemon Pocket App");
        Console.WriteLine("*****************************");
        foreach (var item in Functions) Console.WriteLine(item.Key);

        char input = GetInput("Please only enter [1,2,3,4,5,6,7,8] or Q to quit:");
        if (char.ToLower(input).Equals('q')) Environment.Exit(0);
        RunFunction((int)input - 48);
    }

    private dynamic? GetInput(string question)
    {
        try
        {
            Console.Write($"{question.Substring(0, 1) + question.Substring(1)} ");
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

    private void RunFunction(int index)
    {
        if (index >= 0 && index <= Functions.Count())
        {
            Console.WriteLine();
            Functions.ElementAt(index - 1).Value();
        }
        else Console.WriteLine("That is not one of the options, please try again or enter Q to quit.");
    }

    private void GetListPokemons()
    {
        while (true)
        {
            char showList = GetInput("Display updated pokemons in pocket?")!;

            switch (char.ToLower(showList))
            {
                case 'y':
                    Console.WriteLine();
                    ListPokemons();
                    return;
                case 'n':
                    Console.WriteLine();
                    return;
                default:
                    Console.WriteLine("Please only enter \"y\" or \"n\".\n");
                    break;
            }
        }
    }

    private void ListPokemons()
    {
        List<Pokemon> pokemons = _Service.GetPokemons();
        if (pokemons.Count() == 0) goto Display;

        GetSort:
        char sortList = GetInput("Would you like the list to be sorted? (y/n)?");
        if (char.ToLower(sortList).Equals('n')) goto Display;

        if (!char.ToLower(sortList).Equals('y'))
        {
            Console.WriteLine("Please only enter \"y\" or \"n\".");
            goto GetSort;
        }

    GetSortAttr:
        char sortAttr = GetInput("\nHow would you like the list to be sorted by?\n(1) Name\n(2) Hp\n(3) Exp\nEnter a number:");
        if (char.ToLower(sortAttr).Equals('n')) goto Display;

        if (!new List<int>() { '1', '2', '3', '4' }.Contains(char.ToLower(sortAttr)))
        {
            Console.WriteLine("Please only enter [1, 2, 3, 4].");
            goto GetSortAttr;
        }

        pokemons = (sortAttr) switch
        {
            '1' => pokemons.OrderBy(pokemon => pokemon.Name).ToList(),
            '2' => pokemons.OrderBy(pokemon => pokemon.Hp).ToList(),
            '3' => pokemons.OrderBy(pokemon => pokemon.Exp).ToList(),
            _ => pokemons
        };

    Display:
        Console.WriteLine("\n=== Pokemons in Pocket ===\n");
        if (pokemons.Count() == 0) Console.WriteLine("Pocket is currently empty, add a pokemon to see it!");
        else foreach (Pokemon pokemon in pokemons) Console.WriteLine(pokemon.ToString());
        Console.WriteLine("\n=== End of List ===\n");
    }

    private void AddPokemon()
    {
    GetName:
        Console.Write("Enter Pokemon's Name: ");
        string? name = Console.ReadLine();

        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Name cannot be empty.\nTry again, or enter \"cancel\" to cancel.\n");
            goto GetName;
        }

        name = name.Trim();
        name = (name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower());
        if (name.Equals("Cancel")) return;

        Type? pokemonClass = Type.GetType(name);

        if (pokemonClass == null)
        {
            Console.WriteLine("Invalid pokemon name.\nTry again, or enter \"cancel\" to cancel.\n");
            goto GetName;
        }

        Pokemon toAdd = (Pokemon)Activator.CreateInstance(pokemonClass)!;

    GetHp:
        Console.Write("Enter Pokemon's HP: ");
        string? hp = Console.ReadLine();

        if (string.IsNullOrEmpty(hp))
        {
            Console.WriteLine("Hp cannot be empty.\nTry again, or enter \"cancel\" to cancel.\n");
            goto GetHp;
        }

        if (hp.Trim().ToLower().Equals("cancel")) return;

        if (!int.TryParse(hp, out int intHp))
        {
            Console.WriteLine("Hp must be a valid number.\nTry again, or enter 0 to cancel.\n");
            goto GetHp;
        }

        if (intHp > toAdd.MaxHp)
        {
            Console.WriteLine($"Hp cannot be higher than {toAdd.MaxHp}.\nTry again, or enter 0 to cancel.\n");
            goto GetHp;
        }

        toAdd.Hp = intHp;

    GetExp:
        Console.Write("Enter Pokemon's Exp: ");
        string? exp = Console.ReadLine();

        if (string.IsNullOrEmpty(exp))
        {
            Console.WriteLine("Name cannot be empty.\nTry again, or enter \"cancel\" to cancel.\n");
            goto GetExp;
        }

        if (exp.Trim().ToLower().Equals("cancel")) return;

        if (!int.TryParse(exp, out int intExp))
        {
            Console.WriteLine("Exp must be a valid number.\nTry again, or enter 0 to cancel.\n");
            goto GetExp;
        }

        toAdd.Exp = intExp;

        if (_Service.AddPokemon(toAdd)) Console.WriteLine($"{toAdd.Name} added successfully!\n");
        else
        {
            Console.WriteLine("Something went wrong, please try again.\n");
            goto GetName;
        }

        GetListPokemons();
    }

    private void RemovePokemon()
    {
        List<Pokemon> pokemons = _Service.GetPokemons();

        if (pokemons.Count() == 0)
        {
            Console.WriteLine("No pokemons in pocket to remove, collect more first!");
            return;
        }

        int removed = 0;

        foreach (var pokemon in pokemons) Console.WriteLine($"({pokemons.ToList().IndexOf(pokemon) + 1})\n{pokemon.ToString()}\n");

        while (true)
        {
            if (removed == pokemons.Count()) break;

            char input = GetInput("\nChoose pokemon to remove or enter \"q\" to exit:");
            if (char.ToLower(input).Equals('q')) break;

            int index = (int)input - (48 + 1);

            if (index < 0 || index >= pokemons.Count())
            {
                Console.WriteLine("That is not a valid pokemons\nPlease choose again, or enter \"q\" to exit");
                continue;
            }

            Pokemon toRemove = pokemons.ElementAt(index);
            _Service.RemovePokemon(toRemove);
            removed++;

            Console.WriteLine($"{toRemove.Name} removed successfully!");
        }

        Console.WriteLine($"\n{removed} pokemon(s) removed");
        GetListPokemons();
    }

    private void CheckToEvolve()
    {
        Dictionary<Boolean, List<PokemonMaster>> toEvolve = _Service.CheckToEvolve();

        if (toEvolve[true].Count == 0 && toEvolve[false].Count == 0)
        {
            Console.WriteLine("No pokemon to evolve, collect more first!");
            return;
        }

        foreach (var pokemon in toEvolve[true]) Console.WriteLine($"{pokemon.Name} --> {pokemon.EvolveTo}");
        foreach (var pokemon in toEvolve[false])
        {
            var group = _Service.GetPokemons().Where(p => p.Name.Equals(pokemon.Name));
            Console.WriteLine($"{pokemon.NoToEvolve - group.Count()} more {pokemon.Name} needed to evolve to {pokemon.EvolveTo}");
        }
    }

    private void EvolvePokemons()
    {
        List<PokemonMaster> canEvolve = _Service.CheckToEvolve()[true];

        if (canEvolve.Count() == 0)
        {
            Console.WriteLine("No pokemon to evolve, collect more first!");
            return;
        }

        foreach (var toEvolve in canEvolve)
        {
            List<Pokemon> toRemove = new();

            Console.WriteLine($"{toEvolve.Name} --> {toEvolve.EvolveTo}\n");

            IGrouping<string, Pokemon> group = _Service.GetPokemons().GroupBy(p => p.Name).ToList().Find(g => g.Key.Equals(toEvolve.Name))!;

            if (group.Count() == toEvolve.NoToEvolve)
            {
                group.ToList().ForEach(p => toRemove.Add(p));
                goto UpdateDB;
            }

            foreach (var pokemon in group) Console.WriteLine($"({group.ToList().IndexOf(pokemon) + 1})\n{pokemon.ToString()}\n");

            while (toRemove.Count() < toEvolve.NoToEvolve)
            {
                int index = toRemove.Count() + 1;
                string ordinalIndicator = index == 1 ? "st" : index == 2 ? "nd" : "rd";

                dynamic input = (char)GetInput($"Choose the {index}{ordinalIndicator} {toEvolve.Name} to evolve:");
                if (char.ToLower(input).Equals('q')) return;
                input = (int)input - (48 + 1);

                if (input < 0 || input >= group.Count())
                {
                    Console.WriteLine("That is not one of the options, please select again or enter Q to quit.");
                    continue;
                }

                Pokemon pokemon = group.ElementAt((int)input);

                if (toRemove.Contains(pokemon))
                {
                    Console.WriteLine("Pokemon already selected, select another or enter Q to quit.");
                    continue;
                }

                toRemove.Add(group.ElementAt((int)input));
            }

        UpdateDB:
            toRemove.ForEach(pokemon => _Service.RemovePokemon(pokemon));

            Type pokemonClass = Type.GetType(toEvolve.EvolveTo)!;
            Pokemon instance = (Pokemon)Activator.CreateInstance(pokemonClass)!;
            _Service.AddPokemon(instance);

        }

        GetListPokemons();
    }

    private void Hospital()
    {
        Console.Clear();

        void NursePokemons()
        {
            List<Pokemon> toNurseList = new();

        GetToNurse:
            List<Pokemon> pokemons = _Service.GetPokemons().Where(p => p.Hp < p.MaxHp).ToList();

            if (pokemons.Count() == 0)
            {
                Console.WriteLine("No pokemons to nurse :Dc");
                return;
            }

            foreach (var pokemon in pokemons) Console.WriteLine($"({pokemons.IndexOf(pokemon) + 1})\n{pokemon.ToString()}\n");

            dynamic toNurse = (char)GetInput($"Choose pokemon(s) to nurse:");
            if (char.ToLower(toNurse).Equals('q')) return;
            toNurse = (int)toNurse - (48 + 1);

            if (toNurse < 0 || toNurse >= pokemons.Count())
            {
                Console.WriteLine("That is not one of the options, please select again or enter Q to quit.\n");
                goto GetToNurse;
            }

            toNurse = pokemons.ElementAt((int)toNurse);
            if (toNurse.Hp == toNurse.MaxHp)
            {
                Console.WriteLine($"{toNurse.Name} is already at max hp, please select another or enter Q to quit.\n");
                goto GetToNurse;
            }

            toNurseList.Add(toNurse);
            Console.WriteLine($"{toNurse.Name} selected\n");

            if (toNurseList.Count() == pokemons.Count())
            {
                Console.WriteLine("All pokemons in pocket selected!");
                goto NursePokemons;
            }

        GetContinueChoosing:
            if (toNurseList.Count() == pokemons.Count()) goto NursePokemons;

            char continueChoosing = GetInput("\nChoose more pokemons?")!;

            switch (char.ToLower(continueChoosing))
            {
                case 'y':
                    Console.WriteLine();
                    goto GetToNurse;
                case 'n':
                    Console.WriteLine();
                    break;
                default:
                    Console.WriteLine("Please only enter \"y\" or \"n\".\n");
                    goto GetContinueChoosing;
            }

        NursePokemons:
            Random random = new();

            foreach (var pokemon in toNurseList)
            {
                if (_Service.EditPokemon(pokemon.Id, "Hp", pokemon.MaxHp))
                {
                    Console.WriteLine($"{pokemon.Name} healed to {pokemon.MaxHp:0.00}");
                    pokemons.Remove(pokemon);
                }
                else Console.WriteLine($"{pokemon.Name} could not be nursed properly, please try again.");
            }

            toNurseList.Clear();

        GetContinueNursing:
            if (pokemons.Count() == 0)
            {
                Console.WriteLine("All pokemons at max health!");
                GetListPokemons();
            }

            char continueNursing = GetInput("\nContinue nursing?")!;

            switch (char.ToLower(continueNursing))
            {
                case 'y':
                    Console.WriteLine();
                    goto GetToNurse;
                case 'n':
                    Console.WriteLine();
                    GetListPokemons();
                    break;
                default:
                    Console.WriteLine("Please only enter \"y\" or \"n\".\n");
                    goto GetContinueNursing;
            }
        }

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("***********************");
            Console.WriteLine("Welcome to the Hospital");
            Console.WriteLine("***********************");
            Console.WriteLine("(1) List pokemons in my pocket");
            Console.WriteLine("(2) Nurse selected pokemon(s)");
            Console.WriteLine("(3) Exit hospital");

            dynamic input = (char)GetInput("Please only enter [1,2,3] or Q to quit:")!;

            if (char.ToLower(input).Equals('q'))
            {
                Console.Clear();
                return;
            }

            input = (int)input - 48;
            switch (input)
            {
                case 1:
                    Console.WriteLine();
                    ListPokemons();
                    break;
                case 2:
                    Console.WriteLine();
                    NursePokemons();
                    break;
                case 3:
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine("\nThat is not one of the options, please try again or enter Q to quit.");
                    break;
            }
        }
    }

    private void Gym()
    {
        Console.Clear();

        void TrainPokemons()
        {
            List<Pokemon> toTrainList = new();

        GetToTrain:
            List<Pokemon> pokemons = _Service.GetPokemons();

            if (pokemons.Count() == 0)
            {
                Console.WriteLine("No pokemons to train :cc");
                return;
            }

            foreach (var pokemon in pokemons) Console.WriteLine($"({pokemons.IndexOf(pokemon) + 1})\n{pokemon.ToString()}\n");

            dynamic toTrain = (char)GetInput($"Choose pokemon(s) to Train:");
            if (char.ToLower(toTrain).Equals('q')) return;
            toTrain = (int)toTrain - (48 + 1);

            if (toTrain < 0 || toTrain >= pokemons.Count())
            {
                Console.WriteLine("That is not one of the options, please select again or enter Q to quit.\n");
                goto GetToTrain;
            }

            toTrain = pokemons.ElementAt((int)toTrain);
            toTrainList.Add(toTrain);
            Console.WriteLine($"{toTrain.Name} selected\n");

            if (toTrainList.Count() == pokemons.Count())
            {
                Console.WriteLine("All pokemons in pocket selected!");
                goto TrainPokemons;
            }

        GetContinueChoosing:
            char continueChoosing = GetInput("\nChoose more pokemons?")!;

            switch (char.ToLower(continueChoosing))
            {
                case 'y':
                    Console.WriteLine();
                    goto GetToTrain;
                case 'n':
                    Console.WriteLine();
                    break;
                default:
                    Console.WriteLine("Please only enter \"y\" or \"n\".\n");
                    goto GetContinueChoosing;
            }

        TrainPokemons:
            Random random = new();

            foreach (var pokemon in toTrainList)
            {
                double newExp = pokemon.Exp + (random.Next(300) + random.NextDouble());

                if (_Service.EditPokemon(pokemon.Id, "Exp", newExp)) Console.WriteLine($"{pokemon.Name} trained to {newExp:0.00}");
                else Console.WriteLine($"{pokemon.Name} could not be trained properly, please try again.");
            }

            toTrainList.Clear();

        GetContinueTraining:
            char continueTraining = GetInput("\nContinue Training?")!;

            switch (char.ToLower(continueTraining))
            {
                case 'y':
                    Console.WriteLine();
                    goto GetToTrain;
                case 'n':
                    Console.WriteLine();
                    GetListPokemons();
                    break;
                default:
                    Console.WriteLine("Please only enter \"y\" or \"n\".\n");
                    goto GetContinueTraining;
            }
        }

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("******************");
            Console.WriteLine("Welcome to the Gym");
            Console.WriteLine("******************");
            Console.WriteLine("(1) List pokemons in my pocket");
            Console.WriteLine("(2) Train selected pokemon(s)");
            Console.WriteLine("(3) Exit gym");

            dynamic input = (char)GetInput("Please only enter [1,2,3] or Q to quit:")!;

            if (char.ToLower(input).Equals('q'))
            {
                Console.Clear();
                return;
            }

            input = (int)input - 48;
            switch (input)
            {
                case 1:
                    Console.WriteLine();
                    ListPokemons();
                    break;
                case 2:
                    Console.WriteLine();
                    TrainPokemons();
                    break;
                case 3:
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine("\nThat is not one of the options, please try again or enter Q to quit.");
                    break;
            }
        }
    }

    enum Trade
    {
        Pichu,
        Pikachu,
        Raichu,
        Eevee,
        Flareon,
        Charmander,
        Charmeleon,
        Charizard
    }
    private void TradeMarket()
    {
        Console.Clear();

        Random random = new();
        List<Pokemon> trades;

        void RefreshTrades()
        {
            trades = new();
            while (true)
            {
                int randomNum = random.Next(8);

                Type pokemonClass = Type.GetType(Enum.GetName(typeof(Trade), randomNum)!)!;
                Pokemon newTrade = (Pokemon)Activator.CreateInstance(pokemonClass)!;
                newTrade.Hp = random.Next(newTrade.MaxHp);
                newTrade.Exp = random.Next(500);

                trades.Add(newTrade);

                if (trades.Count() == 4) break;
            }
        }

        void DisplayTrades()
        {
            Console.WriteLine("=== Trade Market ===");
            foreach (var trade in trades) Console.WriteLine(trade.ToString());
        }

        void TradeSession()
        {
        GetTradeFor:
            foreach (var trade in trades) Console.WriteLine($"({trades.IndexOf(trade) + 1})\n{trade.ToString()}\n");
            dynamic tradeFor = (char)GetInput($"Choose pokemon to trade for:");
            if (char.ToLower(tradeFor).Equals('q')) return;
            tradeFor = (int)tradeFor - (48 + 1);

            if (tradeFor < 0 || tradeFor >= trades.Count())
            {
                Console.WriteLine("That is not one of the options, please select again or enter Q to quit.\n");
                goto GetTradeFor;
            }

            tradeFor = trades.ElementAt((int)tradeFor);
            Console.WriteLine($"{tradeFor.Name} selected\n");

        GetTradeWith:
            List<Pokemon> pokemons = _Service.GetPokemons();
            foreach (var pokemon in pokemons) Console.WriteLine($"({pokemons.IndexOf(pokemon) + 1})\n{pokemon.ToString()}\n");

            dynamic tradeWith = (char)GetInput($"Choose pokemon to trade with:");
            if (char.ToLower(tradeWith).Equals('q')) return;
            tradeWith = (int)tradeWith - (48 + 1);

            if (tradeWith < 0 || tradeWith >= pokemons.Count())
            {
                Console.WriteLine("That is not one of the options, please select again or enter Q to quit.\n");
                goto GetTradeWith;
            }

            tradeWith = pokemons.ElementAt((int)tradeWith);
            Console.WriteLine($"{tradeWith.Name} selected\n");

            if (!_Service.ReplacePokemon(tradeWith.Id, tradeFor))
            {
                Console.WriteLine("Something went wrong, please try again");
                goto GetTradeFor;
            }
            else
            {
                trades[trades.IndexOf(tradeFor)] = tradeWith;
                Console.WriteLine($"{tradeWith.Name} traded for {tradeFor.Name}");
            }

        GetContinueTrading:
            char continueTrading = GetInput("\nContinue trading?")!;

            switch (char.ToLower(continueTrading))
            {
                case 'y':
                    Console.WriteLine();
                    goto GetTradeFor;
                case 'n':
                    Console.WriteLine();
                    break;
                default:
                    Console.WriteLine("Please only enter \"y\" or \"n\".\n");
                    goto GetContinueTrading;
            }

            GetListPokemons();
        }

        RefreshTrades();
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("***************************");
            Console.WriteLine("Welcome to the Trade Market");
            Console.WriteLine("***************************");
            Console.WriteLine("(1) Display trades");
            Console.WriteLine("(2) Refresh trades");
            Console.WriteLine("(3) List pokemons in my pocket");
            Console.WriteLine("(4) Start trade session");
            Console.WriteLine("(5) Exit market");

            dynamic input = (char)GetInput("Please only enter [1,2,3,4,5] or Q to quit:")!;

            if (char.ToLower(input).Equals('q'))
            {
                Console.Clear();
                return;
            }

            input = (int)input - 48;
            switch (input)
            {
                case 1:
                    Console.WriteLine();
                    DisplayTrades();
                    break;
                case 2:
                    Console.WriteLine();
                    RefreshTrades();
                    DisplayTrades();
                    break;
                case 3:
                    Console.WriteLine();
                    ListPokemons();
                    break;
                case 4:
                    if (_Service.GetPokemons().Count() == 0)
                    {
                        Console.WriteLine("\nNo pokemons to trade with");
                        break;
                    }
                    else if (trades.Count > 0)
                    {
                        Console.WriteLine();
                        TradeSession();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\nNo more pokemons in trade market");
                        break;
                    }
                case 5:
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine("\nThat is not one of the options, please try again or enter Q to quit.");
                    break;
            }
        }
    }
}
