using System.ComponentModel.DataAnnotations;

public class PokemonMaster
{
    public string Name { get; set; } = string.Empty;
    public int NoToEvolve { get; set; }
    public string EvolveTo { get; set; } = string.Empty;

    public PokemonMaster() { }
    public PokemonMaster(string name, int noToEvolve, string evolveTo)
    {
        Name = name;
        NoToEvolve = noToEvolve;
        EvolveTo = evolveTo;
    }

    public new string ToString() => $"{NoToEvolve} {Name} evolves to {EvolveTo}";
}

public class Pokemon
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Hp { get; set; }
    public double Exp { get; set; }
    public string Skill { get; internal set; }
    public Pokemon() : base() => Skill = string.Empty;

    public new string ToString() => $"-----------------------\nName: {Name}\nHP: {Hp}\nExp: {Exp}\nSkill: {Skill}\n-----------------------";
}

class Pichu : Pokemon
{
    public Pichu() : base()
    {
        Name = "Pichu";
        Skill = "Lightning Rod";
    }
}

class Pikachu : Pokemon
{
    public Pikachu() : base()
    {
        Name = "Pikachu";
        Skill = "Lightning Bolt";
    }
}

class Raichu : Pokemon
{
    public Raichu() : base()
    {
        Name = "Raichu";
        Skill = "Surge Suffer";
    }
}

class Eevee : Pokemon
{
    public Eevee() : base()
    {
        Name = "Eevee";
        Skill = "Run Away";
    }
}

class Flareon : Pokemon
{
    public Flareon() : base()
    {
        Name = "Flareon";
        Skill = "Flash Fire";
    }
}

class Charmander : Pokemon
{
    public Charmander() : base()
    {
        Name = "Charmander";
        Skill = "Solar Power";
    }
}

class Charmeleon : Pokemon
{
    public Charmeleon() : base()
    {
        Name = "Charmeleon";
        Skill = "Blaze";
    }
}

class Charizard : Pokemon
{
    public Charizard() : base()
    {
        Name = "Charizard";
        Skill = "Blaze";
    }
}