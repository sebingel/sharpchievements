using System;

namespace sebingel.sharpchievements
{
    public class AchievementSaveException : Exception
    {
        public AchievementSaveException(string message)
            : base(message)
        { }

        public AchievementSaveException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
