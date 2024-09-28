// See https://aka.ms/new-console-template for more information
using System.Numerics;
using System.Runtime.Intrinsics.Arm;
namespace UkolFizzBuzz
{
    internal class Program
    {
        class FizzBuzz
        {
            public static int theNumber;
            public static void DisplayNumbersAndFiZZBuzz()
            {
                for (int i = 1; i <= theNumber; i++)
                {
                    if (i % 3 == 0 && i % 5 == 0)
                    {
                        Console.WriteLine("FizzBuzz");
                    }

                    else if (i % 5 == 0)
                    {
                        Console.WriteLine("Buzz");
                    }
                    else if (i % 3 == 0)
                    {
                        Console.WriteLine("Fizz");
                    }
                    else
                        Console.WriteLine(i);
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, please enter number greater than 0, which will be the upper limit for FizzBuzz game.");
            bool isNum = int.TryParse(Console.ReadLine(), out FizzBuzz.theNumber);
            if (FizzBuzz.theNumber > 0 && isNum)
            {
                System.Console.WriteLine($"Numbers from 1 to {FizzBuzz.theNumber} go like this:");
                FizzBuzz.DisplayNumbersAndFiZZBuzz();
                Console.WriteLine("Press any key to leave");
                Console.ReadLine();
            }

            else
                Console.WriteLine("Something went wrong");
            Console.ReadLine();
        }
    }
}

