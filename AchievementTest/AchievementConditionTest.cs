using System.Diagnostics;

namespace sebingel.sharpchievements.Tests
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
        /// Executes Tests for creating an AchievementCondition, making some progress and completing an AchievementCondition
        /// </summary>
        internal static void Execute()
        {
            CreateAchievementCondition();
            DoAchievementConditionProgress();
            CompleteAchievementCondition();
        }

        /// <summary>
        /// sets completed variable true, when an AchievementCondition is completed
        /// </summary>
        /// <param name="achievementCondition"></param>
        private static void AcConditionCompleted(AchievementCondition achievementCondition)
        {
            completed = true;
        }

        /// <summary>
        /// Tests if the completion of an AchievementCondition works as designed
        /// </summary>
        private static void CompleteAchievementCondition()
        {
            // create the AchievementCondition and wire event
            AchievementCondition ac = GetNewAchievementCondition();
            ac.ConditionCompleted += AcConditionCompleted;

            // MakeProgress
            ac.MakeProgress();
            ac.MakeProgress();
            ac.MakeProgress();
            ac.MakeProgress();

            // check if AchievementCondition is not completed/unlocked
            Debug.Assert(!completed, "!completed");
            Debug.Assert(!ac.Unlocked, "!ac.Unlocked");

            // make final prograss that completes/unlocks the AchievementCondition
            ac.MakeProgress();

            // check if AchievementCondition is completed/unlocked
            Debug.Assert(completed, "completed");
            Debug.Assert(ac.Unlocked, "ac.Unlocked");
        }

        /// <summary>
        /// Creates an AchievementCondition
        /// </summary>
        /// <returns></returns>
        private static AchievementCondition GetNewAchievementCondition()
        {
            return new AchievementCondition(uniqueId, achievementConditionKey, countToUnlock);
        }

        /// <summary>
        /// Checks of progression of an AchievementCondition works as designed
        /// </summary>
        private static void DoAchievementConditionProgress()
        {
            // Create AchievementCondition and wire events
            AchievementCondition ac = GetNewAchievementCondition();
            ac.ProgressChanged += AcProgressChanged;
            // MakeProgress
            ac.MakeProgress();

            // Check of progress was made
            Debug.Assert(madeProgress, "madeProgress");
        }

        /// <summary>
        /// Set the madeProgress variable is event is called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void AcProgressChanged(AchievementCondition sender, AchievementConditionProgressChangedArgs args)
        {
            madeProgress = true;
        }

        /// <summary>
        /// Creates an AchievementCondition
        /// </summary>
        private static void CreateAchievementCondition()
        {
            AchievementCondition ac = GetNewAchievementCondition();
            Debug.Assert(ac.UniqueId == uniqueId, "ac.UniqueId==uniqueId");
            Debug.Assert(ac.Unlocked == false, "ac.Unlocked==false");
            Debug.Assert(ac.AchievementConditionKey == achievementConditionKey, "ac.AchievementConditionKey==achievementConditionKey");
        }
    }
}
