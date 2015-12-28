using System.Diagnostics;
using sebingel.sharpchievements;

namespace AchievementTest
{
    /// <summary>
    /// Test Class for AchievementCondition
    /// </summary>
    internal static class AchievementConditionTest
    {
        private static bool madeProgress;
        private static bool completed;

        private static string uniqueId = "acUniqueId";
        private static string achievementConditionKey = "acConditionKey";
        private static int countToUnlock = 5;

        /// <summary>
        /// Executes Tests for Creating an AchivementCondition, making some progress and completing an AchievementCondition
        /// </summary>
        internal static void Execute()
        {
            CreateAchievementCondition();
            DoAchievementConditionProgress();
            CompleteAchievementCondition();
        }

        private static void AcConditionCompleted(AchievementCondition achievementCondition)
        {
            completed = true;
        }

        private static void CompleteAchievementCondition()
        {
            AchievementCondition ac = GetNewAchievementCondition();
            ac.ConditionCompleted += AcConditionCompleted;

            ac.MakeProgress();
            ac.MakeProgress();
            ac.MakeProgress();
            ac.MakeProgress();

            Debug.Assert(madeProgress, "madeProgress");
            Debug.Assert(!completed, "!completed");
            Debug.Assert(!ac.Unlocked, "!ac.Unlocked");

            ac.MakeProgress();

            Debug.Assert(completed, "completed");
            Debug.Assert(ac.Unlocked, "ac.Unlocked");
        }

        private static AchievementCondition GetNewAchievementCondition()
        {
            return new AchievementCondition(uniqueId, achievementConditionKey, countToUnlock);
        }

        private static void DoAchievementConditionProgress()
        {
            AchievementCondition ac = GetNewAchievementCondition();
            ac.ProgressChanged += AcProgressChanged;
            ac.MakeProgress();

            Debug.Assert(madeProgress, "madeProgress");
        }

        private static void AcProgressChanged(AchievementCondition sender, AchievementConditionProgressChangedArgs args)
        {
            madeProgress = true;
        }

        private static void CreateAchievementCondition()
        {
            AchievementCondition ac = GetNewAchievementCondition();
            Debug.Assert(ac.UniqueId == uniqueId, "ac.UniqueId==uniqueId");
            Debug.Assert(ac.Unlocked == false, "ac.Unlocked==false");
            Debug.Assert(ac.AchievementConditionKey == achievementConditionKey, "ac.AchievementConditionKey==achievementConditionKey");
        }
    }
}
