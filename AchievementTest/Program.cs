using System;
using System.Collections.Generic;
using sebingel.sharpchievements;

namespace AchievementTest
{
    public class Program
    {
        private static readonly AchievementManager Am = AchievementManager.GetInstance();

        static void Main()
        {
            Am.AchievementCompleted += AchievementCompleted;

            //BasicTest();
            //SaveTest();
            LoadTest();
        }

        private static void SaveTest()
        {
            AchievementManager am = AchievementManager.GetInstance();

            AchievementCondition ac = new AchievementCondition("key", 10);
            Achievement a = new Achievement("a", "desc", ac);

            am.RegisterAchievementCondition(ac);
            am.RegisterAchievement(a);

            am.SaveAchiements(@"C:\Datasec\AnwAdmin\achievements.bin");

            Console.ReadLine();
        }

        private static void LoadTest()
        {
            Am.LoadAchievements(@"C:\Datasec\AnwAdmin\achievements.bin");

            Am.ReportProgress("key");
            Am.ReportProgress("key");
            Am.ReportProgress("key");

            Am.SaveAchiements(@"C:\Datasec\AnwAdmin\achievements.bin");

            Console.ReadLine();
        }

        private static void BasicTest()
        {
            AchievementCondition achievementCondition = new AchievementCondition("a", 5);
            AchievementCondition condition = new AchievementCondition("a", 10);
            AchievementCondition item = new AchievementCondition("s", 5);

            Achievement a = new Achievement("5xA", "You pressed a five times", achievementCondition);
            //a.AchievementCompleted += AchievementCompleted;

            Achievement b = new Achievement("10xA", "You pressed a ten times", condition);
            //b.AchievementCompleted += AchievementCompleted;

            Achievement c = new Achievement("5xS", "You pressed s five times", item);
            //c.AchievementCompleted += AchievementCompleted;

            Achievement d = new Achievement("5xS+10xA", "You pressed s five times and a ten times",
                new List<AchievementCondition> { condition, item });
            //d.AchievementCompleted += AchievementCompleted;

            Am.RegisterAchievement(a);
            Am.RegisterAchievement(b);
            Am.RegisterAchievement(c);
            Am.RegisterAchievement(d);

            Am.RegisterAchievementCondition(achievementCondition);
            Am.RegisterAchievementCondition(condition);
            Am.RegisterAchievementCondition(item);

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
