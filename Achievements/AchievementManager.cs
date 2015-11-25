using System.Collections.Generic;

namespace sebingel.sharpchievements
{
    /// <summary>
    /// Central management class for everything achievement related
    /// </summary>
    public class AchievementManager
    {
        private static AchievementManager instance;
        private readonly List<AchievementCondition> registeredAchievementConditions;
        private readonly List<Achievement> registeredAchievements;

        public event AchievementCompleteHandler AchievementCompleted;
        protected virtual void InvokeAchievementCompleted(Achievement achievement)
        {
            if (AchievementCompleted != null)
                AchievementCompleted(achievement);
        }

        /// <summary>
        /// Central management class for everything achievement related
        /// </summary>
        private AchievementManager()
        {
            registeredAchievementConditions = new List<AchievementCondition>();
            registeredAchievements = new List<Achievement>();
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
        /// Register an Achievement
        /// </summary>
        /// <remarks>The AchievementCompleted event only fires for registered Achievements!</remarks>
        /// <param name="achievement">The Achievement that should be registered</param>
        public void RegisterAchievement(Achievement achievement)
        {
            registeredAchievements.Add(achievement);
            achievement.AchievementCompleted += AchievementAchievementCompleted;
        }

        /// <summary>
        /// Fires the AchievementCompleted event of the AchievementManager
        /// </summary>
        /// <param name="achievement">Achievement that fired the AchievementCompleted event</param>
        private void AchievementAchievementCompleted(Achievement achievement)
        {
            InvokeAchievementCompleted(achievement);
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
