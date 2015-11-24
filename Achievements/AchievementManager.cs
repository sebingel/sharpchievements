using System.Collections.Generic;

namespace sebingel.Achievements
{
    public class AchievementManager
    {
        private static AchievementManager instance;
        private readonly List<AchievementCondition> registeredAchievementConditions;

        private AchievementManager()
        {
            registeredAchievementConditions = new List<AchievementCondition>();
        }

        public static AchievementManager GetInstance()
        {
            if (instance == null)
                instance = new AchievementManager();
            return instance;
        }

        public void RegisterAchievementCondition(AchievementCondition achievementCondition)
        {
            registeredAchievementConditions.Add(achievementCondition);
        }

        public void ReportProgress(string achviementConditionKey)
        {
            foreach (AchievementCondition condition in registeredAchievementConditions)
            {
                if (condition.AchievementConditionKey == achviementConditionKey)
                    condition.MakeProgress();
            }
        }
    }
}
