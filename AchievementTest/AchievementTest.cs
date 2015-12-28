using System;
using System.Diagnostics;

namespace sebingel.sharpchievements.Tests
{
    internal static class AchievementTest
    {
        internal static void Execute()
        {
            CreateAchievementTest();
        }

        private static void CreateAchievementTest()
        {
            AchievementCondition achievementCondition = new AchievementCondition("acUniqueId", "acKey", 5);
            string uniqueId = "aUniqueId";
            string atitel = "aTitel";
            string adescription = "aDescription";

            Achievement a = new Achievement(uniqueId, atitel, adescription, achievementCondition);

            Debug.Assert(!a.Unlocked, "!a.Unlocked");
            Debug.Assert(a.Conditions[0] == achievementCondition, "a.Conditions[0]==achievementCondition");
            Debug.Assert(a.Description == adescription, "a.Description==adescription");
            Debug.Assert(String.IsNullOrEmpty(a.ImagePath), "String.IsNullOrEmpty(a.ImagePath)");
            Debug.Assert(a.Progress == 0, "a.Progress==0");
            Debug.Assert(a.Titel == atitel, "a.Titel==atitel");
            Debug.Assert(a.UniqueId == uniqueId);
        }
    }
}
