using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace sebingel.sharpchievements.Tests
{
    /// <summary>
    /// Test Class for Achievement
    /// </summary>
    internal static class AchievementTest
    {
        private static bool completed;
        private static bool progress;

        /// <summary>
        /// Executes Tests for creating an Achievement, clearing an Achievement and completing an Achievement
        /// </summary>
        internal static void Execute()
        {
            CreateAchievementTest();
            ClearAchievementTest();
            CompleteAchievementTest();
            AchievementProgressTest();
        }

        private static void AchievementProgressTest()
        {
            // create AcheivementCondition
            AchievementCondition achievementCondition = new AchievementCondition("acUniqueId", "acKey", 3);
            string uniqueId = "aUniqueId";
            string atitel = "aTitel";
            string adescription = "aDescription";

            // create Achievement and wire event
            Achievement a = new Achievement(uniqueId, atitel, adescription, achievementCondition);
            a.ProgressChanged += AProgressChanged;

            // check if no progress is made yet
            Debug.Assert(!progress, "!progress");

            // MakeProgress
            achievementCondition.MakeProgress();

            // check if progress is made
            Debug.Assert(progress, "progress");

            // achievementCondition has a count of 3. so making one step should raise the progress of the achievement to 33%
            Debug.Assert(a.Progress == 33, "a.Progress==33");
        }

        /// <summary>
        /// Sets the progress variable when the event is fired
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void AProgressChanged(Achievement sender, AchievementProgressChangedArgs args)
        {
            progress = true;
        }

        /// <summary>
        /// Tests if the completion of an Achievement works as designed
        /// </summary>
        private static void CompleteAchievementTest()
        {
            // create AcheivementCondition
            AchievementCondition achievementCondition = new AchievementCondition("acUniqueId", "acKey", 1);
            string uniqueId = "aUniqueId";
            string atitel = "aTitel";
            string adescription = "aDescription";

            // create Achievement and wire event
            Achievement a = new Achievement(uniqueId, atitel, adescription, achievementCondition);
            a.AchievementCompleted += AAchievementCompleted;

            // check if not completed yet
            Debug.Assert(!completed, "!completed");

            // MakeProgress
            achievementCondition.MakeProgress();

            // check if completed/unlocked
            Debug.Assert(completed, "completed");
            Debug.Assert(a.Unlocked, "a.Unlocked");
        }

        /// <summary>
        /// Sets the completed variable to true if event is fired
        /// </summary>
        /// <param name="achievement"></param>
        static void AAchievementCompleted(Achievement achievement)
        {
            completed = true;
        }

        /// <summary>
        /// Checks if clearing of an Achievement works as designed
        /// </summary>
        private static void ClearAchievementTest()
        {
            // create AchievementConditions
            AchievementCondition achievementCondition = new AchievementCondition("acUniqueId", "acKey", 5);
            AchievementCondition achievementCondition2 = new AchievementCondition("acUniqueId2", "acKey", 5);
            string uniqueId = "aUniqueId";
            string atitel = "aTitel";
            string adescription = "aDescription";

            // create Achievement
            Achievement a = new Achievement(uniqueId, atitel, adescription, (IEnumerable<AchievementCondition>)new List<AchievementCondition> { achievementCondition, achievementCondition2 });
            // check of Achievement has both AchievementConditions
            Debug.Assert(a.Conditions.Count() == 2, "a.Conditions.Count == 2");

            // clear Achievement
            a.Clear();
            // Check if Conditions are gone
            Debug.Assert(!a.Conditions.Any(), "!a.Conditions.Any()");
        }

        /// <summary>
        /// Checks the Constructors of Achievement
        /// </summary>
        private static void CreateAchievementTest()
        {
            // Create AchievementCondition
            AchievementCondition achievementCondition = new AchievementCondition("acUniqueId", "acKey", 5);
            string uniqueId = "aUniqueId";
            string atitel = "aTitel";
            string adescription = "aDescription";
            string aImagepath = "aImagepath";

            #region Constructor 1

            Achievement a = new Achievement(uniqueId, atitel, adescription, achievementCondition);
            Debug.Assert(!a.Unlocked, "!a.Unlocked");
            Debug.Assert(a.Conditions.ToList()[0] == achievementCondition, "a.Conditions[0]==achievementCondition");
            Debug.Assert(a.Description == adescription, "a.Description==adescription");
            Debug.Assert(String.IsNullOrEmpty(a.ImagePath), "String.IsNullOrEmpty(a.ImagePath)");
            Debug.Assert(a.Progress == 0, "a.Progress==0");
            Debug.Assert(a.Titel == atitel, "a.Titel==atitel");
            Debug.Assert(a.UniqueId == uniqueId, "a.UniqueId == uniqueId");
            Debug.Assert(a.Conditions.Any(), "a.Conditions.Count == 1");
            Debug.Assert(!a.Hidden, "!a.Hidden");

            #endregion

            #region Constructor 2

            a = new Achievement(uniqueId, atitel, adescription, new List<AchievementCondition> { achievementCondition });
            Debug.Assert(!a.Unlocked, "!a.Unlocked");
            Debug.Assert(a.Conditions.ToList()[0] == achievementCondition, "a.Conditions[0]==achievementCondition");
            Debug.Assert(a.Description == adescription, "a.Description==adescription");
            Debug.Assert(String.IsNullOrEmpty(a.ImagePath), "String.IsNullOrEmpty(a.ImagePath)");
            Debug.Assert(a.Progress == 0, "a.Progress==0");
            Debug.Assert(a.Titel == atitel, "a.Titel==atitel");
            Debug.Assert(a.UniqueId == uniqueId, "a.UniqueId == uniqueId");
            Debug.Assert(a.Conditions.Any(), "a.Conditions.Count == 1");
            Debug.Assert(!a.Hidden, "!a.Hidden");

            #endregion

            a.ImagePath = aImagepath;
            a.Hidden = true;

            Debug.Assert(a.ImagePath == aImagepath, "a.ImagePath == aImagepath");
            Debug.Assert(a.Hidden, "a.Hidden");
        }
    }
}
