namespace sebingel.sharpchievements
{
    /// <summary>
    /// Contains information about a progresschange of an AchievementCondition
    /// </summary>
    public class AchievementConditionProgressChangedArgs
    {
        /// <summary>
        /// Current progress of the AchievementCondition
        /// </summary>
        public int ProgressCount { get; private set; }

        /// <summary>
        /// Contains information about a progresschange of an AchievementCondition
        /// </summary>
        /// <param name="progressCount">Current progress of the AchievementCondition</param>
        public AchievementConditionProgressChangedArgs(int progressCount)
        {
            ProgressCount = progressCount;
        }
    }
}
