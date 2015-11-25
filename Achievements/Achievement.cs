﻿using System.Collections.Generic;

namespace sebingel.scharpchievements
{
    /// <summary>
    /// Class that represents an achievement
    /// </summary>
    public class Achievement
    {
        #region Properties

        /// <summary>
        /// List of conditions which must be met to unlock the achievement
        /// </summary>
        public List<AchievementCondition> Conditions { get; private set; }

        /// <summary>
        /// Titel of the achievement
        /// </summary>
        public string Titel { get; private set; }

        /// <summary>
        /// Description of the achievement
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Flag which shows if the achievement is unlocked
        /// </summary>
        public bool Unlocked { get; private set; }

        /// <summary>
        /// Progress of unlocking the achievement
        /// </summary>
        public int Progress { get; private set; }

        #endregion

        /// <summary>
        /// Event that fires when the progress has changed
        /// </summary>
        public event AchievementProgressChangedHandler ProgressChanged;
        private void InvokeProgressChanged(int progressCount)
        {
            if (ProgressChanged != null)
                ProgressChanged(this, new AchievementProgressChangedArgs(progressCount));
        }

        /// <summary>
        /// Event that fires when the achievement is unlocked
        /// </summary>
        public event AchievementCompleteHandler AchievementCompleted;
        private void InvokeAchievementCompleted()
        {
            Unlocked = true;
            if (AchievementCompleted != null)
                AchievementCompleted(this);
        }

        #region Constructor(s)

        /// <summary>
        /// An achievement
        /// </summary>
        /// <param name="titel">Titel of the achievement</param>
        /// <param name="description">Description of the achievement</param>
        /// <param name="conditions">List of conditions which must be met to unlock the achievement</param>
        public Achievement(string titel, string description, List<AchievementCondition> conditions)
        {
            Conditions = conditions;
            Titel = titel;
            Description = description;

            foreach (AchievementCondition condition in Conditions)
            {
                condition.ProgressChanged += ConditionProgressChanged;
                condition.ConditionCompleted += ConditionCompleted;
            }
        }

        #endregion

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
    }
}
