﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace sebingel.sharpchievements.Tests
{
    /// <summary>
    /// Test Class for Achievement
    /// </summary>
    internal static class AchievementTest
    {
        private static bool completed;

        /// <summary>
        /// Executes Tests for creating an Achievement, clearing an Achievement and completing an Achievement
        /// </summary>
        internal static void Execute()
        {
            CreateAchievementTest();
            ClearAchievementTest();
            CompleteAchievementTest();
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
            Achievement a = new Achievement(uniqueId, atitel, adescription, new List<AchievementCondition> { achievementCondition, achievementCondition2 });
            // check of Achievement has both AchievementConditions
            Debug.Assert(a.Conditions.Count == 2, "a.Conditions.Count == 2");

            // clear Achievement
            a.Clear();
            // Check if Conditions are gone
            Debug.Assert(a.Conditions.Count == 0, "a.Conditions.Count == 0");
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
            Debug.Assert(a.Conditions[0] == achievementCondition, "a.Conditions[0]==achievementCondition");
            Debug.Assert(a.Description == adescription, "a.Description==adescription");
            Debug.Assert(String.IsNullOrEmpty(a.ImagePath), "String.IsNullOrEmpty(a.ImagePath)");
            Debug.Assert(a.Progress == 0, "a.Progress==0");
            Debug.Assert(a.Titel == atitel, "a.Titel==atitel");
            Debug.Assert(a.UniqueId == uniqueId, "a.UniqueId == uniqueId");
            Debug.Assert(a.Conditions.Count == 1, "a.Conditions.Count == 1");

            #endregion

            #region Constructor 2

            a = new Achievement(uniqueId, atitel, adescription, achievementCondition, aImagepath);
            Debug.Assert(!a.Unlocked, "!a.Unlocked");
            Debug.Assert(a.Conditions[0] == achievementCondition, "a.Conditions[0]==achievementCondition");
            Debug.Assert(a.Description == adescription, "a.Description==adescription");
            Debug.Assert(a.ImagePath == aImagepath, "a.ImagePath == aImagepath");
            Debug.Assert(a.Progress == 0, "a.Progress==0");
            Debug.Assert(a.Titel == atitel, "a.Titel==atitel");
            Debug.Assert(a.UniqueId == uniqueId, "a.UniqueId == uniqueId");
            Debug.Assert(a.Conditions.Count == 1, "a.Conditions.Count == 1");

            #endregion

            #region Constructor 3

            a = new Achievement(uniqueId, atitel, adescription, new List<AchievementCondition> { achievementCondition });
            Debug.Assert(!a.Unlocked, "!a.Unlocked");
            Debug.Assert(a.Conditions[0] == achievementCondition, "a.Conditions[0]==achievementCondition");
            Debug.Assert(a.Description == adescription, "a.Description==adescription");
            Debug.Assert(String.IsNullOrEmpty(a.ImagePath), "String.IsNullOrEmpty(a.ImagePath)");
            Debug.Assert(a.Progress == 0, "a.Progress==0");
            Debug.Assert(a.Titel == atitel, "a.Titel==atitel");
            Debug.Assert(a.UniqueId == uniqueId, "a.UniqueId == uniqueId");
            Debug.Assert(a.Conditions.Count == 1, "a.Conditions.Count == 1");

            a = new Achievement(uniqueId, atitel, adescription, new List<AchievementCondition> { achievementCondition, achievementCondition });
            Debug.Assert(a.Conditions.Count == 1, "a.Conditions.Count == 1");

            AchievementCondition achievementCondition2 = new AchievementCondition("acUniqueId2", "acKey", 5);
            a = new Achievement(uniqueId, atitel, adescription, new List<AchievementCondition> { achievementCondition, achievementCondition2 });
            Debug.Assert(a.Conditions.Count == 2, "a.Conditions.Count == 2");

            #endregion

            #region Constructor 4

            a = new Achievement(uniqueId, atitel, adescription, new List<AchievementCondition> { achievementCondition },
                aImagepath);
            Debug.Assert(!a.Unlocked, "!a.Unlocked");
            Debug.Assert(a.Conditions[0] == achievementCondition, "a.Conditions[0]==achievementCondition");
            Debug.Assert(a.Description == adescription, "a.Description==adescription");
            Debug.Assert(a.ImagePath == aImagepath, "a.ImagePath==aImagepath");
            Debug.Assert(a.Progress == 0, "a.Progress==0");
            Debug.Assert(a.Titel == atitel, "a.Titel==atitel");
            Debug.Assert(a.UniqueId == uniqueId, "a.UniqueId == uniqueId");
            Debug.Assert(a.Conditions.Count == 1, "a.Conditions.Count == 1");

            #endregion
        }
    }
}