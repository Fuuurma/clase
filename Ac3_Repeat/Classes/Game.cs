using Players;
using Enemies;
using System.Diagnostics;

public class Game // clase general para el juego
{
    public bool PlayGame;
    public int BattlesWon;
    public string Scene;
    private Stopwatch timer;

    // Constructor
    public Game()
    {
        Scene = GetSceneForBattle();
        BattlesWon = 0;
        PlayGame = true;
        timer = new Stopwatch();
    }

    public string GetSceneForBattle()
    {
        Random random = new Random();
        string[] scenes = new string[] { "El Camp Nou", "La guarida de los lamentos", "La gruta de los perdedores", "Los anillos de Júpiter",
                                        "El valle perdido", "El mar de la desolación", "Cueva en Afghanistán", "Nido del dragón", "Playa de los cubatas" };
        string scene = scenes[random.Next(scenes.Length)];
        return scene;
    }

    public void Instructions()
    {
        string separator = new string('-', 50);
        Console.WriteLine(separator);
        Console.WriteLine(" · · · 📜 Instrucciones 📜 · · · ");
        Console.WriteLine("Se te preguntará antes de cada batalla si quieres usar una armadura.");
        Console.WriteLine("Acepta o no, dependiendo del enemigo.\nEso es todo en lo que puedes influir.");
        Console.WriteLine("\nComo funciona el juego:");
        Console.WriteLine("Combate por turnos. Donde tiras 2 dados. Esto se suma a tu poder de ataque * 1.5");
        Console.WriteLine("En el daño total tambien influye el tipo, armadura y habilidad.");
        Console.WriteLine("Al ganar 2 turnos seguidos. Curas HP por 50% del daño hecho.");
        Console.WriteLine("La velocidad influye en quien tira antes de los 2.");
        Console.WriteLine("NO puedes ganar o ser derrotado sin haber ganado el turno con los dados.");
        Console.WriteLine("Para más info mira los comentarios en el Program.cs");
        Console.WriteLine("Buena Suerte\n");
        Console.WriteLine(separator);
    }

    public void AnnounceBattle(Player player, Enemy enemy)
    {
        string separator = new string('-', 50);
        Console.WriteLine(separator);
        Console.WriteLine("🔪 Proxima Batalla! 🔪");
        Console.WriteLine($"🌄Escenario: {GetSceneForBattle()}🌅");
        Console.WriteLine($"{player.Name} lvl({player.Level}) vs {enemy.Name} con {enemy.weapon}");
        Console.WriteLine(separator);
        Console.WriteLine("-------------- Estadisticas ----------------------");
        Console.WriteLine($"| Nombre:      {player.Name, -6} vs {enemy.Name, -24}|");
        Console.WriteLine($"| Tipo:        {player.Type, -4} vs {enemy.Type, -22}|");
        Console.WriteLine($"| Vida:        {player.Health, -6} vs {enemy.Health, -24}|");
        Console.WriteLine($"| Ataque:      {player.Attack, -6} vs {enemy.Attack, -24}|");
        Console.WriteLine($"| Velocidad:   {player.Speed, -6} vs {enemy.Speed, -24}|");
        Console.WriteLine($"| Habilidad:   {player.Skill, -6} vs {enemy.Skill, -24}|");
        Console.WriteLine($"| Armadura:    {player.Armor, -6} vs {enemy.Armor, -24}|");
        Console.WriteLine($"| Experiencia: {player.Experience, -6} vs {enemy.Experience, -24}|");
        Console.WriteLine(separator);
        Console.WriteLine("");
    }

    public void YouLose(Enemy enemy)
    {
        Console.WriteLine($"Lo siento perdiste contra {enemy.Name} con {enemy.weapon}");
    }

    public void IncrementBattlesWon()
    {
        BattlesWon++;
        Console.WriteLine($"Batallas Ganadas: {BattlesWon}");
    }

    public void StartTimer() // tiempo jugado
    {
        timer.Start();
    }
    public void StopTimer()
    {
        timer.Stop();
        TimeSpan timePassed = timer.Elapsed;
        Console.WriteLine($"⌛ Tiempo jugado: {timePassed.Minutes} minutos {timePassed.Seconds} segundos⌛");
    }
}
    