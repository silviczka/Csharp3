// See https://aka.ms/new-console-template for more information
namespace UkolGreed
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Dice game, press any key to roll the dice");
            Console.ReadLine();
            for (int i = 1; i < 6; i++)
            {
                Random rnd = new Random();
                int die = rnd.Next(1, 7);
                TheDice.DieNumbers.Add(die);
            }
            string dieNumbersString = string.Join(",", TheDice.DieNumbers.ToArray());
            Console.WriteLine($"The dice are thrown and dice numbers are: {dieNumbersString}\n");
            TheDice.PointCounter();
            Console.WriteLine($"Your score is: {TheDice.sumOfPoints}");
            //Console.ReadLine();
        }
    }
}
