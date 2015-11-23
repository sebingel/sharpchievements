using System.Collections.Generic;

namespace sebingel.Achievements
{
    public class Achievement
    {
        public string AchievementKey { get; private set; }
        public List<AchievementCondition> Conditions { get; private set; }
        public bool Unlocked { get; private set; }
        public int Progress { get; private set; }

        public event AchievementProgressChangedHandler ProgressChanged;
        protected virtual void InvokeProgressChanged(int progressCount)
        {
            if (ProgressChanged != null)
                ProgressChanged(this, new AchievementProgressChangedArgs(progressCount));
        }

        public event AchievementCompleteHandler AchievementCompleted;
        private void InvokeAchievementCompleted()
        {
            Unlocked = true;
            if (AchievementCompleted != null)
                AchievementCompleted(this);
        }

        public Achievement(string achievementKey, List<AchievementCondition> conditions)
        {
            AchievementKey = achievementKey;
            Conditions = conditions;

            foreach (AchievementCondition condition in Conditions)
            {
                condition.ProgressChanged += ConditionProgressChanged;
                condition.ConditionCompleted += ConditionCompleted;
            }
        }

        private void ConditionCompleted(AchievementCondition achievementCondition)
        {
            if (achievementCondition.Unlocked)
            {
                return;
            }

            bool allConditionsCompleted = true;
            foreach (AchievementCondition condition in Conditions)
            {
                if (!condition.Unlocked)
                    allConditionsCompleted = false;
            }
            Unlocked = allConditionsCompleted;

            if (Unlocked)
            {
                InvokeAchievementCompleted();
            }
        }

        private void ConditionProgressChanged(AchievementCondition sender, AchievementConditionProgressChangedArgs args)
        {
            if (sender.Unlocked)
            {
                return;
            }

            Progress = 100;

            InvokeProgressChanged(Progress);
        }
    }
}
