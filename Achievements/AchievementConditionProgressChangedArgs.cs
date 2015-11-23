namespace sebingel.Achievements
{
    public class AchievementConditionProgressChangedArgs
    {
        public int ProgressCount { get; private set; }

        public AchievementConditionProgressChangedArgs(int progressCount)
        {
            ProgressCount = progressCount;
        }
    }
}
