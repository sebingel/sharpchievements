//Copyright 2015 Sebastian Bingel

//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at

//    http://www.apache.org/licenses/LICENSE-2.0

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;

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
        public IEnumerable<IAchievementCondition> Conditions { get; private set; }

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
        /// The current progress of the Achievement in percent
        /// </summary>
        public int Progress { get; private set; }

        /// <summary>
        /// Path to the Image that is displayed in notifications etc.
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Gets the Hidden-Flag. Hidden Achievements reveal only when unlocked.
        /// </summary>
        public bool Hidden { get; set; }

        public DateTime UnlockTimeStamp { get; private set; }

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
        /// An achievement
        /// </summary>
        /// <param name="uniqueId">Applicationwide unique uniqueId of the achievement</param>
        /// <param name="titel">Applicationwide unique uniqueId of the achievement</param>
        /// <param name="description">Description of the achievement</param>
        /// <param name="conditions">List of conditions which must be met to unlock the achievement</param>
        public Achievement(string uniqueId, string titel, string description,
            IEnumerable<IAchievementCondition> conditions)
        {
            if (conditions == null)
                throw new ArgumentNullException(nameof(conditions));
            if (String.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentOutOfRangeException(nameof(description),
                    "description can not be null, empty or whitespace only");
            }
            if (String.IsNullOrWhiteSpace(uniqueId))
            {
                throw new ArgumentOutOfRangeException(nameof(uniqueId),
                    "uniqueId can not be null, empty or whitespace only");
            }
            if (String.IsNullOrWhiteSpace(titel))
                throw new ArgumentOutOfRangeException(nameof(titel), "titel can not be null, empty or whitespace only");

            UniqueId = uniqueId;
            Titel = titel;
            Description = description;

            // Add AchievementConditions
            List<IAchievementCondition> conditionList = new List<IAchievementCondition>();
            foreach (IAchievementCondition achievementCondition in conditions)
            {
                // but only of not already added
                if (conditionList.Find(x => x.UniqueId == achievementCondition.UniqueId) == null)
                    conditionList.Add(achievementCondition);
            }
            Conditions = conditionList;

            foreach (IAchievementCondition condition in Conditions)
            {
                if (condition != null)
                {
                    condition.ProgressChanged += ConditionProgressChanged;
                    condition.ConditionCompleted += ConditionCompleted;
                }
            }
        }

        /// <summary>
        /// An achievement
        /// </summary>
        /// /// <param name="uniqueId">Applicationwide unique uniqueId of the achievement</param>
        /// <param name="titel">Applicationwide unique uniqueId of the achievement</param>
        /// <param name="description">Description of the achievement</param>
        /// <param name="condition">Condition that must be met to unlock the achievement</param>
        public Achievement(string uniqueId, string titel, string description, IAchievementCondition condition)
            : this(uniqueId, titel, description, new List<IAchievementCondition> { condition })
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is tied to the ConditionCompleted events in the AchievementConditions of this Achievement
        /// </summary>
        /// <param name="achievementCondition">IAchievementCondition that fired the event</param>
        private void ConditionCompleted(IAchievementCondition achievementCondition)
        {
            CheckUnlockStatus();
        }

        /// <summary>
        /// Checks the status of all assigned AchievementConditions and determines if the Achievement unlocks
        /// </summary>
        /// <remarks>Is useful to evaluate if a newly added Achievement should be unlocked without having to make progress again</remarks>
        public void CheckUnlockStatus()
        {
            if (Unlocked)
                return;

            Unlocked = Conditions.All(condition => condition.Unlocked);

            if (Unlocked)
            {
                UnlockTimeStamp = DateTime.UtcNow;
                InvokeAchievementCompleted();
            }
        }

        /// <summary>
        /// This method is tied to the ProgressChanged events in the AchievementConditions of this Achievement
        /// </summary>
        /// <param name="sender">IAchievementCondition thath fired the event</param>
        /// <param name="args">Parameters that are important for this event</param>
        private void ConditionProgressChanged(IAchievementCondition sender, AchievementConditionProgressChangedArgs args)
        {
            if (sender.Unlocked)
                return;

            // Calculate overall progress of Achievement based on progress of AchievementConditions
            Progress = Conditions.Sum(condition => condition.Progress) / Conditions.Count();

            InvokeProgressChanged(Progress);
        }

        public void Clear()
        {
            foreach (IAchievementCondition achievementCondition in Conditions)
            {
                achievementCondition.ProgressChanged -= ConditionProgressChanged;
                achievementCondition.ConditionCompleted -= ConditionCompleted;
            }

            Conditions = Enumerable.Empty<IAchievementCondition>();
        }

        #endregion
    }
}