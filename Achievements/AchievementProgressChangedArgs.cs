namespace sebingel.Achievements
{
    public class AchievementProgressChangedArgs
    {
        public int ProgressCount { get; private set; }

        public AchievementProgressChangedArgs(int progressCount)
        {
            ProgressCount = progressCount;
        }
    }
}
