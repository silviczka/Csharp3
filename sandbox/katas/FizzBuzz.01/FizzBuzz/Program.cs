// See https://aka.ms/new-console-template for more information
using System.Numerics;
using System.Runtime.Intrinsics.Arm;
namespace UkolFizzBuzz
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, please enter number greater than 0, which will be the upper limit for FizzBuzz game.");
            int theNumber;
            bool isNum = int.TryParse(Console.ReadLine(), out theNumber);
            if (theNumber > 0 && isNum)
            {
                Console.WriteLine($"Numbers from 1 to {theNumber} go like this:");
                FizzBuzz.DisplayNumbersAndFiZZBuzz(theNumber);
                Console.WriteLine("Press any key to leave");
                Console.ReadLine();
            }

            else
            {
                Console.WriteLine("Something went wrong");
                Console.ReadLine();
            }
        }
    }
}

