using System;
using System.Threading;
using MyDice;
using Players;
using Enemies;

// clase con toda la logica de combate 1vs1
public class Combat
{
    public bool HasUserWon { get; set; }

    public Combat(Player player, Enemy enemy)
    {
        HasUserWon = WhoWins(player, enemy); // params: objetos jugador y enemigo
    }

    // super funcion bool que usarará otras funciones / helpers para la logica
    public bool WhoWins(Player player, Enemy enemy)
    {   // hay que iniciar todas las variables ANTES del bucle 
        Dice dice = new Dice();
        bool turnPlayer = player.Speed > enemy.Speed; // saber quien empieza simple
        int[] turnWinner = Array.Empty<int>(); // array con datos de dados al final de cada turno
        int turnNumber = 1;
        bool haveBothAttacked = false;

        int[] userDices = { -1, -1 };  // template
        int userDamage = 0;
        int userWinningStreak = 0;

        int[] enemyDices = { -1, -1 };
        int enemyDamage = 0;
        int enemyWinningStreak = 0;

        bool whoStartsHasBeenSelected = false;

        while (player.Health > 0 && enemy.Health > 0) // bucle mientras los 2 vivos
        {
            
            Console.WriteLine($"· · · Turno {turnNumber} · · ·  ");
            
            
            if (player.Speed == enemy.Speed) // quien empieza cuando speed es ==
            {
                if (!whoStartsHasBeenSelected) // antes daba bucle infinito con rl turno seimpre del mismo.
                {
                    if ((player.Attack + player.Skill) >= (enemy.Attack + enemy.Skill)) 
                    {
                        turnPlayer = true;
                    }
                    else
                    {
                        turnPlayer = false;

                    }
                    whoStartsHasBeenSelected = true;
                }
                
            }

            if (turnPlayer) // jugador
            {
                Console.WriteLine("Tirar dados?\n");
                Console.ReadLine();
                Console.WriteLine("");

            
                Console.Write($"{player.Name} Saca "); // animacion
                for (var i = 0; i < 3; i++)
                {
                    Thread.Sleep(250);
                    Console.Write("🎲");
                    Thread.Sleep(250);
                }
                
                Console.WriteLine("");

                userDices = dice.RollDice();
                Console.WriteLine($"{userDices[0]} y {userDices[1]}");

                userDamage = CalculateDamage( turnPlayer, userDices, player, enemy );
                Console.WriteLine($"Daño: {userDamage}💥💥");

                enemy.Health -= userDamage;
                Console.WriteLine($"{enemy.Name} Vida restante: {enemy.Health} 💛\n");
            }
            else // enemigo
            {
                Console.WriteLine("");
                Console.Write($"{enemy.Name} Saca" );
                for (var i = 0; i < 3; i++)
                {
                    Thread.Sleep(250);
                    Console.Write("🎲");
                    Thread.Sleep(250);
                }
                
                Console.WriteLine("");

                enemyDices = dice.RollDice();
                Console.WriteLine($" {enemyDices[0]} y {enemyDices[1]}");

                enemyDamage = CalculateDamage( turnPlayer, enemyDices, player, enemy );
                Console.WriteLine($"Daño: {enemyDamage}💥💥");

                player.Health -= enemyDamage;
                Console.WriteLine($"{player.Name} Vida restante: {player.Health} 💙\n");
            }

            Thread.Sleep(1500);
            // calcula si los 2 han atacado pq son 2 iteraciones del bucle para que sea true
            haveBothAttacked = HaveBothAttacked(userDamage, enemyDamage); 
            if (haveBothAttacked)
            {
                turnNumber++;
                // array con datos del fin de turno. ganador, nums ganadores, perdedoros...
                turnWinner = WinningStreak(userDices, userWinningStreak, enemyDices, enemyWinningStreak);

                if (turnWinner[1] >= 2) // si tiene racha ganadora de 2. Suma 50% del daño que haga
                {
                    
                    if (turnWinner[0] == 0)
                    {
                        enemy.Health = HealingAttack(enemyDamage, enemy.Health, player.Health);
                        Console.WriteLine($"En racha: {enemy.Name} (nuevo hp): {enemy.Health})");
                    }
                    else if (turnWinner[0] == 1)
                    {
                        player.Health = HealingAttack(userDamage, player.Health, enemy.Health);
                        Console.WriteLine($"En racha: {player.Name} (nuevo hp: {player.Health})");
                    }
                }

                // enseñamos los datos del turno   
                if (turnWinner[0] == 0) // Aqui si hay empate a dados. dará como ganador el mismo anterior. Pero no suma.
                {
                    Console.WriteLine($"· · · 🔔 Resultado dados 🔔 · · ·");
                    Console.WriteLine($"{player.Name} - {turnWinner[3]} vs {turnWinner[2]} - {enemy.Name}");
                    Console.WriteLine($"Ganador: {enemy.Name} 🏆");
                    Console.WriteLine($"Racha ganadora: {turnWinner[1]} \n");
                    Thread.Sleep(2000);

                    enemyWinningStreak = turnWinner[1]; // actualizamos rachas
                    userWinningStreak = 0;
                }
                else if (turnWinner[0] == 1)
                {
                    Console.WriteLine($"· · · 🔔 Resultado dados 🔔 · · ·");
                    Console.WriteLine($"{player.Name} - {turnWinner[2]} vs {turnWinner[3]} - {enemy.Name}");
                    Console.WriteLine($"Ganador: {player.Name} 🏆");
                    Console.WriteLine($"Racha ganadora: {turnWinner[1]} \n");
                    Thread.Sleep(2000);

                    userWinningStreak = turnWinner[1];
                    enemyWinningStreak = 0;
                }

                userDamage = enemyDamage = 0; // reset variables
                haveBothAttacked = false;
            }

            // Comprobacion de vidas
            if (player.Health <= 0)
            {
                if (!haveBothAttacked) // si mata sin que el otro haya tirado aun.
                {   // le damos una oprtunidad
                    int enemyNumbers = enemyDices[0] + enemyDices[1];
                    Console.WriteLine($"tirada de supervivencia! Numero a batir: {enemyNumbers}");
                    Thread.Sleep(2000);

                    int[] playerLastChance = dice.RollDice();
                    Console.WriteLine($"Sacaste {playerLastChance[0] + playerLastChance[1]}");

                    if (enemyNumbers > playerLastChance[0] + playerLastChance[1])
                    {
                        Console.WriteLine($"{enemy.Name} gana el combate! 🎉\n");
                        return false;
                    }
                    else // sobrevive
                    {
                        Console.WriteLine("💪");
                        Console.WriteLine($"{player.Name} aguantó el golpe!");
                        player.Health = 1;
                    }    
                }
                else // ya han atacado los 2. 
                {
                    if (turnWinner[0] == 0) // enemigo ha ganado el turno (dados)
                    {
                        Console.WriteLine($"{enemy.Name} gana el combate! 🎉\n");
                        return false;
                    }
                    else // sobreivve
                    {
                        Console.WriteLine("💪");
                        Console.WriteLine($"{player.Name} aguantó el golpe!");
                        player.Health = 1;
                    }
                }
            }

            else if (enemy.Health <= 0) // igual que user
            {
                if (!haveBothAttacked)
                {
                    int playerNumbers = userDices[0] + userDices[1];
                    Console.WriteLine($"🚨 Tirada de supervivencia! Numero a batir: {playerNumbers} 🚨");
                    Thread.Sleep(2000);

                    int[] enemyLastChance = dice.RollDice();
                    Console.WriteLine($"{enemy.Name} sacó {enemyLastChance[0] + enemyLastChance[1]}");

                    if (playerNumbers > enemyLastChance[0] + enemyLastChance[1])
                    {
                        Console.WriteLine($"{player.Name} gana el combate! 🎉\n");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("💪");
                        Console.WriteLine($"{enemy.Name} aguantó el golpe!");
                        enemy.Health = 1;
                    }
                }
                else
                {
                    if (turnWinner[0] == 1) // para ganar debes haber ganado el turno en la tirada de dados
                    {
                        Console.WriteLine($"{player.Name} gana el combate! 🎉\n");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("💪");
                        Console.WriteLine($"{enemy.Name} aguantó el golpe!");
                        enemy.Health = 1;
                    }
                }
                
                
            }

            Thread.Sleep(1000);
            turnPlayer = !turnPlayer; // cambia turno y vuelve a empezar el bucle. Siguiente turno
        }

        return player.Health > enemy.Health; // gana user o gana enemigo (aqui 1 ya no tiene HP)
    }

    
    // Funciones para user en WhoWins()
    private static int CalculateDamage(bool isPlayerTurn, int[] diceRoll, Player player, Enemy enemy)
    { // calcula el daño realizado dependiendo de a quien le toque.
        if (isPlayerTurn) // las funciones que llama hacen lo que dice su nombre
        {
            int damage = (diceRoll[0] + diceRoll[1]) + (int)(player.Attack * 1.5);

            if (HasAdvantage(player.Type, enemy.Type))
            {
                Console.WriteLine("Tipo ventajoso!✅");
                damage = (int)(damage * 1.3);
            }
            else if (HasAdvantage(enemy.Type, player.Type))
            {
                Console.WriteLine("Tipo en desventaja! ❌");
                damage = (int)(damage * 0.7);
            }

            if (CheckArmor(enemy.Armor))
            {
                Console.WriteLine("La armadura redujo el impacto! 🔒🔒");
                damage = (int)(damage / 2);
            }
            else // si tiene armor no puede recibir criticos
            {
                if (IsCritical(player.Skill))
                {
                    damage = (int)(damage * 1.5);
                    Console.WriteLine("Golpe Crítico!🎯");
                }
            }

            return damage;
        }

        else // turno enemigo. Es igual 
        {
            // Enemy Turn
            int damage = (diceRoll[0] + diceRoll[1]) + (int)(enemy.Attack * 1.5);

            if (HasAdvantage(enemy.Type, player.Type))
            {
                Console.WriteLine("Tipo ventajoso para el enemigo!✅");
                damage = (int)(damage * 1.3);
            }
            else if (HasAdvantage(player.Type, enemy.Type))
            {
                Console.WriteLine("Tipo en desventaja para el enemigo!❌");
                damage = (int)(damage * 0.7);
            }

            if (CheckArmor(player.Armor))
            {
                Console.WriteLine("La armadura redujo el impacto! 🔒🔒");
                damage = (int)(damage / 2);
            }
            else
            {
                if (IsCritical(enemy.Skill))
                {
                    damage = (int)(damage * 1.5);
                    Console.WriteLine("Golpe Crítico del enemigo!🎯");
                }
            }

            return damage;
        }
        
        
    }



    private static Random random = new Random();

    private static bool HasAdvantage(string attackerType, string defenderType)
    {
        if ((attackerType == "Fuego 🔥" && defenderType == "Planta 🌿") ||
            (attackerType == "Agua 🌊" && defenderType == "Fuego 🔥") ||
            (attackerType == "Planta 🌿" && defenderType == "Agua 🌊"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

     private static bool IsCritical(int attackerSkill)
    { // para calcular criticos se usa la habilidad. Si tiene 10 es siempre critico
        const int maxSkill = 10;

        if (attackerSkill > maxSkill)
        {
            attackerSkill = maxSkill;
            return true;
        }

        int numberToGuess = maxSkill; // 10

        for (var i = 0; i < attackerSkill; i++)
        {
            int randomNumber = random.Next(i, maxSkill);
            if (randomNumber == numberToGuess)
            {
                return true;
            }
        }
        return false;
    }

    private static bool CheckArmor(bool defenderArmor) // atr de la clase
    {
        if (defenderArmor)
        {
            return true;
        }
        return false;
    }

    private static int[] WinningStreak(int[] playerDices, int playerWinningStreak, 
                                        int[] enemyDices, int enemyWinningStreak)
    {
        int[] defaultArray = { -1, -1 }; // se usa para comprobar el 1r turno de combates
        if (playerDices == defaultArray ||
            enemyDices == defaultArray )
            {
                return defaultArray;
            }
        
        int winningStreak = 0; // racha
        bool isUserWinning = false; // de quien

        int numberPlayerDices =  (playerDices[0] + playerDices[1]); // nums       
        int numberEnemyDices =  (enemyDices[0] + enemyDices[1]);

        int winnerDices = 0;
        int loserDices = 0;

        int whoWon = numberPlayerDices.CompareTo(numberEnemyDices); //nose usa

        if (numberPlayerDices > numberEnemyDices) // quien gana
        {
            if (!isUserWinning) // el turno anterior no habia ganado
            {
                isUserWinning = true;
                winningStreak = 0;
                enemyWinningStreak = 0;
            }
            winningStreak ++; // actualizamos
            playerWinningStreak++;
            winnerDices = numberPlayerDices;
            loserDices = numberEnemyDices;

            winningStreak = playerWinningStreak; // este es atr de la clase, que se actualizara luegp.

        }
        else if (numberPlayerDices < numberEnemyDices)
        {
            if (isUserWinning)
            {
                isUserWinning = false;
                winningStreak = 0;
                playerWinningStreak = 0;

            }
            winningStreak ++;
            enemyWinningStreak++;
            winnerDices = numberEnemyDices;
            loserDices = numberPlayerDices;
            
            winningStreak = enemyWinningStreak;
        }

        if (numberPlayerDices == numberEnemyDices)
        {
            winnerDices = loserDices = numberPlayerDices;
            Console.WriteLine($"Empate a dados {winnerDices}");
        }

        // El else es un empate. No hay que hacer nada. la racha se mantiene
                        // bool a int es 0 o 1. Luego se recogen datos en array[0,1,2,3]...
        int[] result = { Convert.ToByte(isUserWinning), winningStreak, winnerDices, loserDices };
        return result ;
    }


    private static bool HaveBothAttacked( int playerDamage, int enemyDamage )
    {
        return playerDamage > 0 && enemyDamage > 0; // simple pero muy util en el bucle
    }


    private static int HealingAttack(int attackDamage, int attackerHealth, int defendeerHealth)
    { // cuando estas en racha curas
        int totalDamageDone;
        int healedHp;
        int updatedHealth;
        const int minHealthHealed = 15;

        if (attackDamage <= defendeerHealth) // aun le queda vida
        {
            totalDamageDone = attackDamage;
        }
        else
        {
            totalDamageDone = defendeerHealth; // no curar mas de la cuenta
        }
        
        healedHp = totalDamageDone / 2;

        if (healedHp < minHealthHealed)
        {
            healedHp = minHealthHealed;
        }

        updatedHealth = attackerHealth + healedHp;

        return updatedHealth;
    }

}

