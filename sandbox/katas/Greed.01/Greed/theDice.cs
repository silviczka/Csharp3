using System;

namespace UkolGreed;

public class TheDice
{
  public static int sumOfPoints;
            public static List<int> DieNumbers = new List<int>();

            public static void PointCounter()
            {
                int[] counts = new int[7];
                foreach (var item in DieNumbers)
                {
                    counts[item]++;
                }
                //quintets
                if (counts[1] == 5)
                {
                    sumOfPoints += 1200;
                }
                if (counts[5] == 5 || counts[6] == 3)
                {
                    sumOfPoints += 600;
                }
                //quintets
                if (counts[1] == 4)
                {
                    sumOfPoints += 1100;
                }
                if (counts[5] == 4)
                {
                    sumOfPoints += 550;
                }
                //triplets
                if (counts[1] == 3)
                {
                    sumOfPoints += 1000;
                }
                if (counts[2] == 3 || counts[1] == 2)
                {
                    sumOfPoints += 200;
                }
                if (counts[3] == 3)
                {
                    sumOfPoints += 300;
                }
                if (counts[4] == 3)
                {
                    sumOfPoints += 400;
                }
                if (counts[5] == 3)
                {
                    sumOfPoints += 500;
                }
                //number 5 and 1 counting up to 2
                if (counts[1] == 1 || counts[5] == 2)
                {
                    sumOfPoints += 100;
                }
                if (counts[5] == 1)
                {
                    sumOfPoints += 50;
                }
            }
}
