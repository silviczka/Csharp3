using System;

namespace UkolFizzBuzz;

public class FizzBuzz
{
            public static void DisplayNumbersAndFiZZBuzz(int theNumber)
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
                    {
                        Console.WriteLine(i);
                    }
                }
            }
}
