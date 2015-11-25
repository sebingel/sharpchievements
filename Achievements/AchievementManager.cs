using System.Collections.Generic;

namespace sebingel.scharpchievements
{
    /// <summary>
    /// Central management class for everything achievement related
    /// </summary>
    public class AchievementManager
    {
        private static AchievementManager instance;
        private readonly List<AchievementCondition> registeredAchievementConditions;

        /// <summary>
        /// Central management class for everything achievement related
        /// </summary>
        private AchievementManager()
        {
            registeredAchievementConditions = new List<AchievementCondition>();
        }

        /// <summary>
        /// Singleton access
        /// </summary>
        /// <returns>The only existing instance</returns>
        public static AchievementManager GetInstance()
        {
            if (instance == null)
                instance = new AchievementManager();
            return instance;
        }

        /// <summary>
        /// Register an AchievementCondition
        /// </summary>
        /// <remarks>Only registered AchievementConditions can be tracked!</remarks>
        /// <param name="achievementCondition">The AchievementCondition that should be registered</param>
        public void RegisterAchievementCondition(AchievementCondition achievementCondition)
        {
            registeredAchievementConditions.Add(achievementCondition);
        }

        /// <summary>
        /// Report progress of an AchievementCondition
        /// </summary>
        /// <param name="achviementConditionKey">The AchievementCondition that should make a progress</param>
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
