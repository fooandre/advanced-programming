public class PokemonMaster
{
    public string Name { get; set; }
    public int NoToEvolve { get; set; }
    public string EvolveTo { get; set; }

    public PokemonMaster() => Name = EvolveTo = string.Empty;

    public PokemonMaster(string name)
    {
        Name = name;
        EvolveTo = string.Empty;
    }

    public PokemonMaster(string name, int noToEvolve, string evolveTo)
    {
        Name = name;
        NoToEvolve = noToEvolve;
        EvolveTo = evolveTo;
    }
}

public class Pokemon : PokemonMaster
{
    public double Hp { get; set; }
    public double Exp { get; set; }
    public string Skill { get; set; }
    public Pokemon(string name) : base(name) => Skill = string.Empty;

    public new string ToString() => $"-----------------------\nName: {Name}\nHP: {Hp}\nExp: {Exp}\nSkill: {Skill}\n-----------------------";
}

class Pikachu : Pokemon { public Pikachu() : base("Pikachu") => Skill = "Lightning Bolt"; }
class Eevee : Pokemon { public Eevee() : base("Eevee") => Skill = "Run Away"; }
class Charmander : Pokemon { public Charmander() : base("Charmander") => Skill = "Solar Power"; }
