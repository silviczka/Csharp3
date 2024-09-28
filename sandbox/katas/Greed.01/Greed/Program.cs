// See https://aka.ms/new-console-template for more information
namespace UkolGreed
{
    internal class Program
    {
        class theDies
        {
            public static int countOf1s;
            public static int countOf2s;
            public static int countOf3s;
            public static int countOf4s;
            public static int countOf5s;
            public static int countOf6s;
            public static int sumOfPoints;
            public static List<int> DiesNumbers = new List<int>();

            public static void pointCounter()
            {
                foreach (var item in DiesNumbers)
                {
                    if (item == 1)
                        countOf1s++;

                    if (item == 2)
                        countOf2s++;

                    if (item == 3)
                        countOf3s++;

                    if (item == 4)
                        countOf4s++;

                    if (item == 5)
                        countOf5s++;

                    if (item == 6)
                        countOf6s++;

                }
                //quintets
                if (countOf1s == 5)
                {
                    sumOfPoints += 1200;
                }
                if (countOf5s == 5)
                {
                    sumOfPoints += 600;
                }
                //quintets
                if (countOf1s == 4)
                {
                    sumOfPoints += 1100;
                }
                if (countOf5s == 4)
                {
                    sumOfPoints += 550;
                }
                //triplets
                if (countOf1s == 3)
                {
                    sumOfPoints += 1000;
                }
                if (countOf2s == 3)
                {
                    sumOfPoints += 200;
                }
                if (countOf3s == 3)
                {
                    sumOfPoints += 300;
                }
                if (countOf4s == 3)
                {
                    sumOfPoints += 400;
                }
                if (countOf5s == 3)
                {
                    sumOfPoints += 500;
                }
                if (countOf6s == 3)
                {
                    sumOfPoints += 600;
                }
                //number 5 and 1 counting up to 2
                if (countOf1s == 2)
                {
                    sumOfPoints += 200;
                }
                if (countOf1s == 1)
                {
                    sumOfPoints += 100;
                }
                if (countOf5s == 2)
                {
                    sumOfPoints += 100;
                }
                if (countOf5s == 1)
                {
                    sumOfPoints += 50;
                }
            }
        }
        static void Main(string[] args)
        {
            System.Console.WriteLine("Welcome to the Dice game, press any key to throw the dies");
            Console.ReadLine();
            for (int i = 1; i < 6; i++)
            {
                Random rnd = new Random();
                int dice = rnd.Next(1, 7);
                theDies.DiesNumbers.Add(dice);
            }
            string dieNumbersString = string.Join(",", theDies.DiesNumbers.ToArray());
            Console.WriteLine($"The dies were thrown and dies numbers are: {dieNumbersString}\n");
            theDies.pointCounter();
            Console.WriteLine($"Your score is: {theDies.sumOfPoints}");
            //Console.ReadLine();
        }
    }
}
