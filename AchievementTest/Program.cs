﻿using System;
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

            BasicTest();
            //SaveLoadTest();
        }

        private static void SaveLoadTest()
        {
            Am.LoadAchievements(@"C:\Datasec\achievementTest.bin", true);
            AchievementCondition ac = new AchievementCondition("titel", "key", 5);
            Achievement a = new Achievement("a", "a", "desc", ac);

            Am.RegisterAchievementCondition(ac);
            Am.RegisterAchievement(a);

            Am.ReportProgress("key");
            Am.ReportProgress("key");
            Am.ReportProgress("key");

            Am.SaveAchiements(@"C:\Datasec\achievementTest.bin");

            Console.ReadLine();
        }

        private static void BasicTest()
        {
            AchievementCondition achievementCondition = new AchievementCondition("aCondition", "a", 5);
            AchievementCondition condition = new AchievementCondition("secondACondition", "a", 10);
            AchievementCondition item = new AchievementCondition("sCondition", "s", 5);

            Achievement a = new Achievement("a", "5xA", "You pressed a five times", achievementCondition);
            //a.AchievementCompleted += AchievementCompleted;

            Achievement b = new Achievement("b", "10xA", "You pressed a ten times", condition);
            //b.AchievementCompleted += AchievementCompleted;

            Achievement c = new Achievement("c", "5xS", "You pressed s five times", item);
            //c.AchievementCompleted += AchievementCompleted;

            Achievement d = new Achievement("d", "5xS+10xA", "You pressed s five times and a ten times",
                new List<AchievementCondition> { condition, item });
            //d.AchievementCompleted += AchievementCompleted;

            Am.RegisterAchievement(a);
            Am.RegisterAchievement(b);
            Am.RegisterAchievement(c);
            Am.RegisterAchievement(d);

            Am.RegisterAchievementCondition(achievementCondition);
            Am.RegisterAchievementCondition(condition);
            Am.RegisterAchievementCondition(item);

            Am.DeleteAchievementByUniqueId("d");
            Am.DeleteAchievementByUniqueId("b");

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
