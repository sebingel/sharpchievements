using System.Diagnostics;
using System.IO;

namespace sebingel.sharpchievements.Tests
{
    /// <summary>
    /// Test for AchievementManager
    /// </summary>
    internal static class AchievementManagerTest
    {
        private static Achievement completed;

        /// <summary>
        /// Executes the Test
        /// </summary>
        internal static void Execute()
        {
            // ---- ATTENTION ----
            // Since AchievementManager is a Singleton testing is a bit tricky.
            // We can not just create a new instance for each test so we have to test everything in regard
            // of the current state of AchievementManager

            // path for the test of the Save() and Load() methods
            string saveFilePath = "./achievementSaveTest.bin";

            // clear possible remains
            if (File.Exists(saveFilePath))
                File.Delete(saveFilePath);

            AchievementManager am = AchievementManager.Instance;
            am.AchievementCompleted += AmAchievementCompleted;

            // create and register first Achievement/AchievementCondition
            AchievementCondition achievementCondition1 = new AchievementCondition("aCUniqueId1", "aCConditionKey1", 1);
            Achievement achievement1 = new Achievement("aUniqueId1", "aTitel", "aDescription", achievementCondition1);
            am.RegisterAchievementCondition(achievementCondition1);
            am.RegisterAchievement(achievement1);

            // Check if first Achievement is in place
            Debug.Assert(am.AchievementList.Count == 1, "am.AchievementList().Count == 1");
            Debug.Assert(am.AchievementList.Contains(achievement1), "am.AchievementList().Contains(achievement1)");

            // create and register second Achievement/AchievementCondition
            AchievementCondition achievementCondition5 = new AchievementCondition("aCUniqueId5", "aCConditionKey5", 5);
            Achievement achievement5 = new Achievement("aUniqueId2", "aTitel", "aDescription", achievementCondition5);
            am.RegisterAchievementCondition(achievementCondition5);
            am.RegisterAchievement(achievement5);

            // check if second Achievement is in place
            Debug.Assert(am.AchievementList.Count == 2, "am.AchievementList().Count == 2");
            Debug.Assert(am.AchievementList.Contains(achievement5), "am.AchievementList().Contains(achievement5)");

            // check progression
            Debug.Assert(completed == null, "completed==null");
            am.ReportProgress("aCConditionKey1");
            Debug.Assert(completed == achievement1, "completed==achievement1");
            Debug.Assert(achievement1.Unlocked, "achievement1.Unlocked");

            // check deletion
            am.DeleteAchievementByUniqueId("aUniqueId1");
            Debug.Assert(am.AchievementList.Count == 1, "am.AchievementList().Count==1");
            Debug.Assert(!am.AchievementList.Contains(achievement1));

            // check progression again
            am.ReportProgress("aCConditionKey5");
            am.ReportProgress("aCConditionKey5");
            am.ReportProgress("aCConditionKey5");
            am.ReportProgress("aCConditionKey5");
            am.ReportProgress("aCConditionKey5");
            Debug.Assert(completed == achievement5, "completed==achievement5");
            Debug.Assert(achievement5.Unlocked, "achievement5.Unlocked");

            // check saving
            am.SaveAchiements(saveFilePath);
            Debug.Assert(File.Exists(saveFilePath), "File.Exists('achievementSaveTest.bin')");

            // check resetting
            am.Reset();
            Debug.Assert(am.AchievementList.Count == 0, "am.AchievementList().Count==0");

            // check loading
            am.LoadAchievements(saveFilePath, true);
            Debug.Assert(am.AchievementList.Count == 1, "am.AchievementList().Count==1");
            Debug.Assert(am.AchievementList[0].Unlocked, "am.AchievementList()[0].Unlocked");

            // clear remains
            File.Delete(saveFilePath);
        }

        static void AmAchievementCompleted(Achievement achievement)
        {
            completed = achievement;
        }
    }
}
