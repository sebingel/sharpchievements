using System;
using System.Collections.Generic;

namespace sebingel.sharpchievements
{
    /// <summary>
    /// Class that represents an achievement
    /// </summary>
    [Serializable]
    public class Achievement
    {
        #region Properties

        /// <summary>
        /// Applicationwide unique uniqueId of the achievement
        /// </summary>
        public string UniqueId { get; private set; }

        /// <summary>
        /// List of conditions which must be met to unlock the achievement
        /// </summary>
        public List<AchievementCondition> Conditions { get; private set; }

        /// <summary>
        /// The Titel of the Achievement
        /// </summary>
        public string Titel { get; private set; }

        /// <summary>
        /// Description of the Achievement
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Flag which shows if the Achievement is unlocked
        /// </summary>
        public bool Unlocked { get; private set; }

        /// <summary>
        /// Progress of unlocking the Achievement
        /// </summary>
        public int Progress { get; private set; }

        /// <summary>
        /// Path to the Image that is displayed in notifications etc.
        /// </summary>
        public string ImagePath { get; private set; }

        #endregion

        #region Events

        /// <summary>
        /// Event that fires when the progress has changed
        /// </summary>
        [field: NonSerialized]
        public event AchievementProgressChangedHandler ProgressChanged;
        private void InvokeProgressChanged(int progressCount)
        {
            if (ProgressChanged != null)
                ProgressChanged(this, new AchievementProgressChangedArgs(progressCount));
        }

        /// <summary>
        /// Event that fires when the achievement is unlocked
        /// </summary>
        [field: NonSerialized]
        public event AchievementCompleteHandler AchievementCompleted;
        private void InvokeAchievementCompleted()
        {
            Unlocked = true;
            if (AchievementCompleted != null)
                AchievementCompleted(this);
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// An Achievement
        /// </summary>
        /// <param name="uniqueId">Applicationwide unique uniqueId of the achievement</param>
        /// <param name="titel">Titel of the Achievement</param>
        /// <param name="description">Description of the achievement</param>
        /// <param name="conditions">List of conditions which must be met to unlock the achievement</param>
        public Achievement(string uniqueId, string titel, string description, List<AchievementCondition> conditions)
            : this(uniqueId, titel, description, conditions, String.Empty)
        { }

        /// <summary>
        /// An achievement
        /// </summary>
        /// <param name="uniqueId">Applicationwide unique uniqueId of the achievement</param>
        /// <param name="titel">Applicationwide unique uniqueId of the achievement</param>
        /// <param name="description">Description of the achievement</param>
        /// <param name="conditions">List of conditions which must be met to unlock the achievement</param>
        /// <param name="imagePath">Path to the image that is displayed in notifivations</param>
        public Achievement(string uniqueId, string titel, string description, List<AchievementCondition> conditions, string imagePath)
        {
            UniqueId = uniqueId;
            Conditions = conditions;
            ImagePath = imagePath;
            Titel = titel;
            Description = description;

            foreach (AchievementCondition condition in Conditions)
            {
                condition.ProgressChanged += ConditionProgressChanged;
                condition.ConditionCompleted += ConditionCompleted;
            }
        }

        /// <summary>
        /// An achievement
        /// </summary>
        /// /// <param name="uniqueId">Applicationwide unique uniqueId of the achievement</param>
        /// <param name="titel">Applicationwide unique uniqueId of the achievement</param>
        /// <param name="description">Description of the achievement</param>
        /// <param name="condition">Condition that must be met to unlock the achievement</param>
        public Achievement(string uniqueId, string titel, string description, AchievementCondition condition)
            : this(uniqueId, titel, description, condition, String.Empty)
        { }

        /// <summary>
        /// An achievement
        /// </summary>
        /// /// <param name="uniqueId">Applicationwide unique uniqueId of the achievement</param>
        /// <param name="titel">Applicationwide unique uniqueId of the achievement</param>
        /// <param name="description">Description of the achievement</param>
        /// <param name="condition">Condition that must be met to unlock the achievement</param>
        /// <param name="imagePath">Path to the image that is displayed in notifivations</param>
        public Achievement(string uniqueId, string titel, string description, AchievementCondition condition, string imagePath)
            : this(uniqueId, titel, description, new List<AchievementCondition> { condition }, imagePath)
        { }

        #endregion

        #region Methods

        /// <summary>
        /// This method is tied to the ConditionCompleted events in the AchievementConditions of this Achievement
        /// </summary>
        /// <param name="achievementCondition">AchievementCondition thath fired the event</param>
        private void ConditionCompleted(AchievementCondition achievementCondition)
        {
            if (Unlocked)
                return;

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

        /// <summary>
        /// This method is tied to the ProgressChanged events in the AchievementConditions of this Achievement
        /// </summary>
        /// <param name="sender">AchievementCondition thath fired the event</param>
        /// <param name="args">Parameters that are important for this event</param>
        private void ConditionProgressChanged(AchievementCondition sender, AchievementConditionProgressChangedArgs args)
        {
            if (sender.Unlocked)
            {
                return;
            }

            // TODO: Calculate exact progress
            Progress = 100;

            InvokeProgressChanged(Progress);
        }

        #endregion

        public void Clear()
        {
            foreach (AchievementCondition achievementCondition in Conditions)
            {
                achievementCondition.ProgressChanged -= ConditionProgressChanged;
                achievementCondition.ConditionCompleted -= ConditionCompleted;
            }
        }
    }
}
