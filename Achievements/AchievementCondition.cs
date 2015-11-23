namespace sebingel.Achievements
{
    public class AchievementCondition
    {
        private readonly int countToUnlock;

        private int progress;

        public string AchievementConditionKey { get; private set; }
        public bool Unlocked { get; private set; }

        public event AchievementConditionProgressChangedHandler ProgressChanged;
        private void InvokeProgressChanged(int progressCount)
        {
            if (ProgressChanged != null)
                ProgressChanged(this, new AchievementConditionProgressChangedArgs(progressCount));
        }

        public event AchievementConditionCompletedHandler ConditionCompleted;
        private void InvokeConditionCompleted()
        {
            Unlocked = true;
            if (ConditionCompleted != null)
                ConditionCompleted(this);
        }

        public AchievementCondition(string achievementConditionKey, int countToUnlock)
        {
            AchievementConditionKey = achievementConditionKey;
            this.countToUnlock = countToUnlock;
            Unlocked = false;
        }

        public void MakeProgress()
        {
            progress++;
            InvokeProgressChanged(progress);

            if (progress >= countToUnlock)
                InvokeConditionCompleted();
        }
    }
}
