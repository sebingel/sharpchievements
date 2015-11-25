namespace sebingel.sharpchievements
{
    /// <summary>
    /// Condition that describes the requirements that must be met to unlock an achievement and can track the progress.
    /// </summary>
    public class AchievementCondition
    {
        private readonly int countToUnlock;
        private int progress;

        /// <summary>
        /// Key of this AchivementCondition. Is used to identify one ore more AchievementConditions by the AchievementManager.
        /// </summary>
        public string AchievementConditionKey { get; private set; }

        /// <summary>
        /// Gets the Unlocked status of an AchievementCondition.
        /// </summary>
        public bool Unlocked { get; private set; }

        /// <summary>
        /// Event that fires when the progress of an AchievementCondition is changed.
        /// </summary>
        public event AchievementConditionProgressChangedHandler ProgressChanged;
        private void InvokeProgressChanged(int progressCount)
        {
            if (ProgressChanged != null)
                ProgressChanged(this, new AchievementConditionProgressChangedArgs(progressCount));
        }

        /// <summary>
        /// Event that fires when an AchievementCondition is completed.
        /// </summary>
        public event AchievementConditionCompletedHandler ConditionCompleted;
        private void InvokeConditionCompleted()
        {
            Unlocked = true;
            if (ConditionCompleted != null)
                ConditionCompleted(this);
        }

        /// <summary>
        /// Condition that describes the requirements that must be met to unlock an achievement and can track the progress.
        /// </summary>
        /// <param name="achievementConditionKey">Key of this AchivementCondition. Is used to identify one ore more AchievementConditions by the AchievementManager.</param>
        /// <param name="countToUnlock">Sets the number of Calls until this AchievementCondtion counts as completed.</param>
        public AchievementCondition(string achievementConditionKey, int countToUnlock)
        {
            AchievementConditionKey = achievementConditionKey;
            this.countToUnlock = countToUnlock;
            Unlocked = false;
        }

        /// <summary>
        /// Adds one progress step for this AchievementCondition
        /// </summary>
        public void MakeProgress()
        {
            progress++;
            InvokeProgressChanged(progress);

            if (progress >= countToUnlock)
                InvokeConditionCompleted();
        }
    }
}
