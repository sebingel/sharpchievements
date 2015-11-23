using System.Collections.Generic;

namespace sebingel.Achievements
{
    public class AchievementManager
    {
        private static AchievementManager instance;
        private readonly List<Achievement> registeredAchievements;

        private AchievementManager()
        {
            registeredAchievements = new List<Achievement>();
        }

        public static AchievementManager GetInstance()
        {
            if (instance == null)
                instance = new AchievementManager();
            return instance;
        }

        public void RegisterAchievement(Achievement achievement)
        {
            registeredAchievements.Add(achievement);
        }

        public void ReportProgress(string achviementConditionKey)
        {
            foreach (Achievement achievement in registeredAchievements)
            {
                foreach (AchievementCondition condition in achievement.Conditions)
                {
                    if (condition.AchievementConditionKey == achviementConditionKey)
                    {
                        condition.MakeProgress();
                    }
                }
            }
        }
    }
}
