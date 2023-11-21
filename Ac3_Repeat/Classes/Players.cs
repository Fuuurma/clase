using System;
using System.Diagnostics.Metrics;
namespace Players
{
    public class Player
{
    public string? Name;
    public int Health;
    public int Attack;
    public int Speed;
    public bool Armor;
    public int Skill;
    public string Type;
    public int Level;
    public int Experience;
    public int BattlesWon;
    public int armorCounter;

    // Constructor del jugador
    public Player(string name, int health, int attack, int speed, 
                    string type, int level, int experience, bool armor, int skill)
    {
        Name = name;
        Health = health;
        Attack = attack;
        Armor = armor;
        Skill = skill;
        Level = level;
        Speed = speed;
        Type = type;
        Experience = experience;
        BattlesWon = 0; 
        armorCounter = 0;
    }

    private static Random random = new Random(); // para no crear una instancia cada vez. con 1 es suficiente

    // Instancia al jugador
    public static Player Create() 
    {
        Player player = new Player ("", 250, 0, 0, "", 1, 0, false, 0); // stats base
        player.Name = player.GiveName();
        player.Attack = player.GiveAttack();
        player.Speed = player.GiveSpeed();
        player.Skill = player.GiveSkill();
        player.Type = player.GetType();
        return player;
    }

    /* seguridad Protected se puede usar en clase padre y heredadas.
       virtual para sobreescribir esa funcion en la clase Enemies
       Las que tengo que usar en program o combat están en public */

    protected virtual string GiveName()
    {
        string name;
        do
        {
            Console.WriteLine("Nombre del jugador: ");
            name = Console.ReadLine();
            Console.WriteLine($"Bienvenido {name}!");
        } 
        while (string.IsNullOrWhiteSpace(name)); // que devuelva lagun valor
        return name;
    }

    protected virtual int GiveAttack()
    {
        return random.Next(10, 20); 
    }

    protected virtual int GiveSpeed()
    {
        return random.Next(1, 10); 
    }

    protected virtual int GiveSkill()
    {
        return random.Next(1, 9);
    }


    public void  AskForArmor()
    {
        Console.WriteLine($"Te quedan { 3 - armorCounter } armaduras.");
        Console.WriteLine("Quieres usar una? ('s' para acceptar.)");
        string? choice = Console.ReadLine();
        if (choice.ToLower() == "s")
        {
            GiveArmor();
        }
    }

    // Cambia atr Armor a true
    public virtual bool GiveArmor()
    {
        
        if (armorCounter >= 3)
        {
            Console.WriteLine("No te quedan más armaduras.\n");
            return false;
        }
        Armor = true;
        Console.WriteLine("Te pones la armadura para el combate");
        Console.WriteLine($"Armadura: {Armor} 🔒\n");
        armorCounter++;
        
        return true; 
    }

    protected virtual string GetType()
    {
        string[] types = { "Fuego 🔥", "Agua 🌊", "Planta 🌿" };
        string[] myOpinion = {
                "Fuego... Que raro. El tipo de los niño rata.",
                "Mira el socorrista este, que se pilla el tipo agua...",
                "Planta, eh? ¿Estás seguro de que no quieres ser un jardinero en lugar de un entrenador Pokémon?"
        };
        
        while (true)
        {
            Console.WriteLine("Escoge tu tipo:");
            for (int i = 0; i < types.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {types[i]}");
            }

            int select = Convert.ToByte(Console.ReadLine());

            if (select >= 1 && select <= types.Length) // 1,2,3
            {
                string type = types[select - 1];
                Console.WriteLine(myOpinion[ select -1 ]);
                Thread.Sleep(3000);
                Console.Clear();
                return type; // index 0
            }
            else
            {
                Console.WriteLine("Tipo incorrecto. Selecciona un número válido.");
            }
        }
    } 

    // Sube de nivel al jugador. Incrementa sus stats.
    public void LevelUp()
    {
        const int maxExp = 10;
        if (this.Experience >= maxExp)  
        {
            this.Level++; // this. para actuar sobre la instancia de la clase
            this.Experience = this.Experience - maxExp;
            this.Health += (int)( this.Health * 0.4 ); 
            this.Attack += (int)( this.Attack * 0.2 );
            this.Speed += 1;
            this.Skill += 1;

            Console.WriteLine($"🔼🔼 {this.Name} subió al nivel {this.Level}! 🔼🔼\n");
        }
    }


    // al final no lo uso.
    protected virtual void InfoPlayer()
    {
        string separator = new string('-', 50);
        Console.WriteLine(separator);
        Console.WriteLine($"{Name} Tipo ({Type}) lvl({Level}) Exp({Experience})");
        Console.WriteLine($"\tVida: {Health}");
        Console.WriteLine($"\tAtaque: {Attack}");
        Console.WriteLine($"\tVelocidad: {Speed}");
        Console.WriteLine($"\tArmadura: {Armor}");
        Console.WriteLine(separator);
        Console.WriteLine();
    }
}
}


