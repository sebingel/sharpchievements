namespace sebingel.scharpchievements
{
    /// <summary>
    /// This class contains data that about the progresschange of an Achievement
    /// </summary>
    public class AchievementProgressChangedArgs
    {
        /// <summary>
        /// Current progress of the Achievement
        /// </summary>
        public int ProgressCount { get; private set; }

        /// <summary>
        /// This class contains data that about the progresschange of an Achievement
        /// </summary>
        /// <param name="progressCount">Current progress of the Achievement</param>
        public AchievementProgressChangedArgs(int progressCount)
        {
            ProgressCount = progressCount;
        }
    }
}
