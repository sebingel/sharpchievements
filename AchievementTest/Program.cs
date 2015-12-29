using System;
using System.Diagnostics;

namespace sebingel.sharpchievements.Tests
{
    public static class Program
    {
        static void Main()
        {
            Stopwatch sw = Stopwatch.StartNew();
            Console.WriteLine(DateTime.Now.ToLongTimeString() + " Starting Tests");

            AchievementConditionTest.Execute();
            AchievementTest.Execute();
            AchievementManagerTest.Execute();

            Console.WriteLine(DateTime.Now.ToLongTimeString() + " All Tests completed in " + sw.ElapsedMilliseconds +
                              " ms\nPress Enter to close");
            Console.ReadLine();
        }
    }
}
