using System;
using System.Collections.Generic;
using sebingel.Achievements;

namespace AchievementTest
{
    public class Program
    {
        static void Main()
        {
            List<AchievementCondition> conditions5A = new List<AchievementCondition>();
            conditions5A.Add(new AchievementCondition("a", 5));

            Achievement a = new Achievement("5x a", conditions5A);
            
            AchievementManager.GetInstance().RegisterAchievement(a);

            Run();
        }

        private static void Run()
        {
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();

            do
            {
                keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.A)
                {
                    AchievementManager.GetInstance().ReportProgress("a");
                }
            }
            while (keyInfo.Key != ConsoleKey.Escape);
        }
    }
}
