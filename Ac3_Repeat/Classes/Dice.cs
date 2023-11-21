using System;

namespace MyDice
{
    public class Dice
    {
    public int[] RollsTotal { get; private set; }

    public Dice()
    {
        RollsTotal = RollDice();
    }

    public int[] RollDice()
    {
        int numRoll1;
        int numRoll2;

        int totalRoll1 = 0; // por si hay dobles
        int totalRoll2 = 0;

        do // bucle que va hasta que NO se saquen dobles.
        {
            numRoll1 = random.Next(1, 7);
            numRoll2 = random.Next(1, 7);

            totalRoll1 += numRoll1; // actualiza
            totalRoll2 += numRoll2;

            if (IsDouble(numRoll1, numRoll2))
            {
                Console.WriteLine($"Dobles {numRoll1}, {numRoll2}\n");
            }

        } while (IsDouble(numRoll1, numRoll2));

        return new int[] { totalRoll1, totalRoll2 };
    }

    private bool IsDouble(int num1, int num2)
    {
        return num1 == num2;
    }

    private static Random random = new Random();

    }
}

