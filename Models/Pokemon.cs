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
    public int MaxHp { get; internal set; }
    public double Exp { get; set; }
    public string Skill { get; internal set; }
    public Pokemon() : base() => Skill = string.Empty;

    public new string ToString() => $"-----------------------\nName: {Name}\nHP: {Hp:0.00}\nExp: {Exp:0.00}\nSkill: {Skill}\n-----------------------";
}

class Pichu : Pokemon
{
    public Pichu() : base()
    {
        Name = "Pichu";
        MaxHp = 244;
        Skill = "Lightning Rod";
    }
}

class Pikachu : Pokemon
{
    public Pikachu() : base()
    {
        Name = "Pikachu";
        MaxHp = 274;
        Skill = "Lightning Bolt";
    }
}

class Raichu : Pokemon
{
    public Raichu() : base()
    {
        Name = "Raichu";
        MaxHp = 324;
        Skill = "Surge Suffer";
    }
}

class Eevee : Pokemon
{
    public Eevee() : base()
    {
        Name = "Eevee";
        MaxHp = 314;
        Skill = "Run Away";
    }
}

class Flareon : Pokemon
{
    public Flareon() : base()
    {
        Name = "Flareon";
        MaxHp = 334;
        Skill = "Flash Fire";
    }
}

class Charmander : Pokemon
{
    public Charmander() : base()
    {
        Name = "Charmander";
        MaxHp = 282;
        Skill = "Solar Power";
    }
}

class Charmeleon : Pokemon
{
    public Charmeleon() : base()
    {
        Name = "Charmeleon";
        MaxHp = 320;
        Skill = "Blaze";
    }
}

class Charizard : Pokemon
{
    public Charizard() : base()
    {
        Name = "Charizard";
        MaxHp = 360;
        Skill = "Blaze";
    }
}