using System;
using Players;
namespace Enemies
{
    public class Enemy : Player // hereda de la clase padre Player
{   // estos no estan del todo bien. Y skill deberia ser heredado. cambie la estructura y se acabo quedando asi
    private static int enemyCount = 0; 
    private bool isBoss;
    public string weapon;
    public int Skill;

    // constructor para cada instancia de Enemy
     public Enemy(string name, int health, int attack, int speed, 
                    string type, int level, int experience, bool isBoss, string weapon, int skill)
            : base(name, health, attack, speed, type, level, experience, false, skill)
    {
        this.isBoss = isBoss;
        this.weapon = weapon;
        Skill = GiveSkill();
        enemyCount++;
        IncreaseBaseStats();

        Armor = isBoss || (random.Next(2) == 0); // si es boss siempre, si no 50% prob
    }


    private static Random random = new Random(); // 1 sola instancia

    // instancia a los enemigos en base al nivel del jugador
    public static Enemy Create(int playerLevel)
    {
        enemyCount++;

        Enemy enemy = new Enemy("", 0, 0, 0, "", 1, 0, false, "", 0); // dummy enemigo

        enemy.Name = enemy.GiveName();
        enemy.Level = 1;
        enemy.Experience = enemy.GiveExperience();
        enemy.weapon = enemy.GetWeapon();
        enemy.Type = enemy.GetType();

        // Atributos base
        const int baseHealth = 100;
        int baseAttack = enemy.GiveAttack();
        int baseSpeed = enemy.GiveSpeed();
        int skill = enemy.GiveSkill();

        // modificador para enemigos. A partir de nivel 3 serán duros.
        int modifier = playerLevel / 2;

        int health = baseHealth + (modifier * 25);
        int attack = baseAttack + (modifier * 2);
        int speed = baseSpeed + (modifier * 2);

        enemy.Health = health;
        enemy.Attack = attack;
        enemy.Speed = speed;
        enemy.Skill = skill;

        if (enemy.GetIsBoss()) // si es boss lo hacemos mas dificil
        {
            enemy.isBoss = true;
            enemy.Health = (int)(health * 1.7); // (int)(operacion) es porque al multiplicar int * double 
            enemy.Attack = (int)(attack * 1.5); // devuelve double pero con (int) lo transforma a int.
            enemy.Speed =  (int)(speed  * 1.2);
            enemy.Skill =  (int)(skill  * 1.3);
            enemy.Experience = (int)(enemy.Experience * 2);
            Console.WriteLine($"\n📛 Cuidado! Ahí viene el jefe {enemy.Name} 📛\n");
        }

        enemy.Armor = ( enemy.isBoss || (random.Next(2) == 0) );

        return enemy;
    }
    
    // No cambia mucho, pero cada enemigo que se crea es un poco mas fuerte.
    private void IncreaseBaseStats() 
    {
        if (!isBoss)
        {
            Attack += (int)(Attack * 0.05);
            Speed += (int)(Speed * 0.05);
            Health += (int)(Health * 0.1);
        }
    }

    protected override string GiveName()
    {
        string[] names;
        if (isBoss)
        {
            names = new string[] { "Terminator", "Jackie-Chan", "Thanos", "Jack Sparrow", "El rey", 
                                    "El fantasma del Papa", "Putin", "Darth Vader", "Voldemort" };
        }
        else
        {
            names = new string[] { "Tu prima", "Tu hermana", "El yonki", "El gitano", "Una persona cualquiera", 
                                    "Rosalía", "Tu Ex", "Un zombie", "Un payaso", "Lacayo", "Troll", "Ratero", "Chalado" };
        }

        return names[random.Next(names.Length)];
    }

    protected override int GiveAttack()
    {
        return random.Next(1, 8); 
    }

    protected override int GiveSpeed()
    {
        return random.Next(1, 10);
    }


    protected override string GetType()
    {
        string[] types = { "Fuego 🔥", "Agua 🌊", "Planta 🌿" };
        return types[random.Next(types.Length)];
    }

    protected override int GiveSkill()
    {
        return random.Next(0,11);
    }

    protected int GiveExperience()
    {
        return random.Next(3,11);
    }

    protected override void InfoPlayer() // no se usa
    {
        string separator = new string('-', 50);
        Console.WriteLine(separator);
        base.InfoPlayer(); // herada
        Console.WriteLine($"Boss: {isBoss}");
        Console.WriteLine($"Weapon: {weapon}");
        Console.WriteLine(separator);
        Console.WriteLine();
    }

    // determina si es boss o no
    public bool GetIsBoss()
    {
        if (isBoss)
        {
            return true;
        }

        const int baseBossProbability = 10; 
        const int additionalProbabilityPerEnemy = 5;
        int bossProbability = baseBossProbability + (enemyCount * additionalProbabilityPerEnemy); // 5% extra por enmeigo

        const int maxProbability = 50;
        if (bossProbability > maxProbability)
        {
            bossProbability = maxProbability;
        }

        if (random.Next(0,101) < bossProbability) // es boss o no
        {
            return true;
        }

        return false;
    }

    public string GetWeapon()
    {
        string[] weapons = new string[] { "una pistola", "un cuchillo de untar", "una piedra", "uranio enriquecido", "C-4", "un palo", 
            "un huevo kinder", "un satisfyer", "una piel de plátano", "pistola de agua", "tira-chinas",
            "AK-47", "Mísil balístico guiado", "Lanzallamas", "Martillo de Thor", "Excalibur", "Espada láser", "Rayo de la muerte" };
        weapon = weapons[random.Next(weapons.Length)];
        return weapon;
    }

    public int GetEnemyCount()
    {
        return enemyCount;
    }
}
}





