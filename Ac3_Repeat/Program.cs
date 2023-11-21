using Players;
using Enemies;
class Program
{   // iTriedToFollowC#NamingConventionsThisTime 
    public static void Main(string[] args)
    {
        Console.Clear();
        Player player = Player.Create();
        Game game = new Game();

        game.StartTimer();
        while (game.PlayGame)
        {
            Enemy enemy = Enemy.Create(player.Level);
            
            if ( game.BattlesWon == 0 )
            {
                game.Instructions();
            }

            game.AnnounceBattle(player, enemy);

            player.AskForArmor();


            // Combat
            Combat combat = new Combat(player, enemy);

            bool hasUserWon = combat.WhoWins(player, enemy);

            if (hasUserWon)
            {
                game.IncrementBattlesWon();
                player.Experience += enemy.Experience;

                if (player.Armor == true)
                {
                    player.Armor = false;
                }

                if (player.Experience >= 10)
                {
                    player.LevelUp();
                }
            }
            else
            {
                game.YouLose(enemy);
                game.PlayGame = false;
            }
            Thread.Sleep(2000);
            // Console.Clear();
            
        }
        game.StopTimer();
    }
}



/*
Funcionalidad tecnica:
clase padre Player con atributos vida, ataque, velocidad, armadura, habilidad, nivel, tipo, experiencia..
Tiene metodos para: crear un personaje, subir de nivel y subir stats, y para mostrar la info.

Clase hija Enemies que hereda sus mismos metodos pero usa como parametro el nivel del jugador. 
Eso permite crear enemigos mas debiles al principio y mas duros mientras progresas.
Metodos para: subir sus stats base, saber si es jefe, contador de enemigos, dar arma y tipo aleatorios.

Clase Dice que retorna un array de 2 ints. En bucle hasta que no saque dobles.

Clase Game que tiene los datos del juego. Escenario, tiempo, batallas jugadas...

Clase Combat con toda la logica del 1vs1 en un bool
Bucle hasta que alguno tenga 0 de vida. Combate por turnos.
Tira los dados y calcula el daño para los 2 personajes.
Calcula quien ataca primero dependiendo de la velocidad con un bool que va alternando acda vez.
Aplica el daño y comprueba si le queda vida o no. Y siguiente turno.
Para calcular el daño:
Ataque Base = NumDados * (PoderAtaque)
Tipo Favorable = Ataque Base * 1.3
Tipo Desfavorable = Ataque Base * 0.7
Ataque Crítico = Ataque Final * 1.5 (crítico es aleatorio. depende de Skill, más es mejor.)
Armadura: Si tiene, el daño le afecta la mitad. Además no puede recibir críticos. Los jefes siempre tienen armadura.
Tu tienes 3 armaduras. Puedes decidir usarlas antes de cada combate.
Racha ganadora: Se va actualizando. Si la racha es 2 o más curas 50% del daño que hagas.
NO puedes ganar o perder sin ganar la tirada de dados vs el contrario.
*/