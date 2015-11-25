using System;
using System.Collections.Generic;
using sebingel.sharpchievements;

namespace AchievementTest
{
    public class Program
    {
        static void Main()
        {
            AchievementManager achievementManager = AchievementManager.GetInstance();

            AchievementCondition achievementCondition = new AchievementCondition("a", 5);
            AchievementCondition condition = new AchievementCondition("a", 10);
            AchievementCondition item = new AchievementCondition("s", 5);

            Achievement a = new Achievement("5xA", "You pressed a five times", new List<AchievementCondition> { achievementCondition });
            //a.AchievementCompleted += AchievementCompleted;

            Achievement b = new Achievement("10xA", "You pressed a ten times", new List<AchievementCondition> { condition });
            //b.AchievementCompleted += AchievementCompleted;

            Achievement c = new Achievement("5xS", "You pressed s five times", new List<AchievementCondition> { item });
            //c.AchievementCompleted += AchievementCompleted;

            Achievement d = new Achievement("5xS+10xA", "You pressed s five times and a ten times", new List<AchievementCondition> { condition, item });
            //d.AchievementCompleted += AchievementCompleted;

            achievementManager.RegisterAchievement(a);
            achievementManager.RegisterAchievement(b);
            achievementManager.RegisterAchievement(c);
            achievementManager.RegisterAchievement(d);

            achievementManager.RegisterAchievementCondition(achievementCondition);
            achievementManager.RegisterAchievementCondition(condition);
            achievementManager.RegisterAchievementCondition(item);

            achievementManager.AchievementCompleted += AchievementCompleted;

            Run();
        }

        static void AchievementCompleted(Achievement achievement)
        {
            Console.WriteLine();
            Console.WriteLine("***** " + achievement.Titel + " *****");
            Console.WriteLine("ACHIEVEMENT UNLOCKED: " + achievement.Description);
        }

        private static void Run()
        {
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey();
                Console.WriteLine();

                if (keyInfo.Key == ConsoleKey.A)
                {
                    AchievementManager.GetInstance().ReportProgress("a");
                }
                else if (keyInfo.Key == ConsoleKey.S)
                {
                    AchievementManager.GetInstance().ReportProgress("s");
                }
            }
            while (keyInfo.Key != ConsoleKey.Escape);
        }
    }
}
