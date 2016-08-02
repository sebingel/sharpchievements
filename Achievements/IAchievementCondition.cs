namespace sebingel.sharpchievements
{
    public interface IAchievementCondition
    {
        string AchievementConditionKey { get; }
        int CountToUnlock { get; }
        int Progress { get; }
        int ProgressCount { get; }
        string UniqueId { get; set; }
        bool Unlocked { get; }

        event AchievementConditionCompletedHandler ConditionCompleted;
        event AchievementConditionProgressChangedHandler ProgressChanged;

        void MakeProgress();
    }
}