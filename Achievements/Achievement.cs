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
    ///     Class that represents an achievement
    /// </summary>
    [Serializable]
    public class Achievement
    {
        #region - Konstruktoren -

        /// <summary>
        ///     An achievement
        /// </summary>
        /// <param name="uniqueId">Applicationwide unique Id of the achievement</param>
        /// <param name="titel">Title of the achievement</param>
        /// <param name="description">Description of the achievement</param>
        /// <param name="conditions">List of conditions which must be met to unlock the achievement</param>
        public Achievement(string uniqueId,
            string titel,
            string description,
            IList<AchievementCondition> conditions)
        {
            if (conditions == null)
            {
                throw new ArgumentNullException(nameof(conditions));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentOutOfRangeException(nameof(description),
                    "description can not be null, empty or whitespace only");
            }

            if (string.IsNullOrWhiteSpace(uniqueId))
            {
                throw new ArgumentOutOfRangeException(nameof(uniqueId),
                    "uniqueId can not be null, empty or whitespace only");
            }

            if (string.IsNullOrWhiteSpace(titel))
            {
                throw new ArgumentOutOfRangeException(nameof(titel), "titel can not be null, empty or whitespace only");
            }

            this.UniqueId = uniqueId;
            this.Titel = titel;
            this.Description = description;

            // Add AchievementConditions
            var conditionList = new List<AchievementCondition>();
            foreach (var achievementCondition in conditions)
            {
                // but only if not already added
                if (conditionList.Find(x => x.UniqueId == achievementCondition.UniqueId) == null)
                {
                    conditionList.Add(achievementCondition);
                }
            }

            this.Conditions = conditionList;

            foreach (var condition in this.Conditions)
            {
                if (condition != null)
                {
                    condition.ProgressChanged += this.ConditionProgressChanged;
                    condition.ConditionCompleted += this.ConditionCompleted;
                }
            }
        }

        /// <summary>
        ///     An achievement
        /// </summary>
        /// ///
        /// <param name="uniqueId">Applicationwide unique uniqueId of the achievement</param>
        /// <param name="titel">Applicationwide unique uniqueId of the achievement</param>
        /// <param name="description">Description of the achievement</param>
        /// <param name="condition">Condition that must be met to unlock the achievement</param>
        public Achievement(string uniqueId, string titel, string description, AchievementCondition condition)
            : this(uniqueId, titel, description, new List<AchievementCondition> {condition})
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }
        }

        #endregion

        #region - Methoden oeffentlich -

        /// <summary>
        ///     Checks the status of all assigned AchievementConditions and determines if the Achievement unlocks
        /// </summary>
        /// <remarks>Is useful to evaluate if a newly added Achievement should be unlocked without having to make progress again</remarks>
        public void CheckUnlockStatus()
        {
            if (this.Unlocked)
            {
                return;
            }

            this.Unlocked = this.Conditions.All(condition => condition.Unlocked);

            if (this.Unlocked)
            {
                this.UnlockTimeStamp = DateTime.UtcNow;
                this.InvokeAchievementCompleted();
            }
        }

        public void Clear()
        {
            foreach (var achievementCondition in this.Conditions)
            {
                achievementCondition.ProgressChanged -= this.ConditionProgressChanged;
                achievementCondition.ConditionCompleted -= this.ConditionCompleted;
            }

            this.Conditions = Enumerable.Empty<AchievementCondition>();
        }

        #endregion

        #region - Methoden privat -

        private void InvokeProgressChanged(int progressCount)
        {
            this.ProgressChanged?.Invoke(this, new AchievementProgressChangedArgs(progressCount));
        }

        private void InvokeAchievementCompleted()
        {
            this.Unlocked = true;
            this.AchievementCompleted?.Invoke(this);
        }

        /// <summary>
        ///     This method is tied to the ConditionCompleted events in the AchievementConditions of this Achievement
        /// </summary>
        /// <param name="achievementCondition">AchievementCondition that fired the event</param>
        private void ConditionCompleted(AchievementCondition achievementCondition)
        {
            this.CheckUnlockStatus();
        }

        /// <summary>
        ///     This method is tied to the ProgressChanged events in the AchievementConditions of this Achievement
        /// </summary>
        /// <param name="sender">AchievementCondition thath fired the event</param>
        /// <param name="args">Parameters that are important for this event</param>
        private void ConditionProgressChanged(AchievementCondition sender, AchievementConditionProgressChangedArgs args)
        {
            if (sender.Unlocked)
            {
                return;
            }

            // Calculate overall progress of Achievement based on progress of AchievementConditions
            this.Progress = this.Conditions.Sum(condition => condition.Progress) / this.Conditions.Count();

            this.InvokeProgressChanged(this.Progress);
        }

        #endregion

        #region - Properties oeffentlich -

        /// <summary>
        ///     Applicationwide unique uniqueId of the achievement
        /// </summary>
        public string UniqueId { get; }

        /// <summary>
        ///     List of conditions which must be met to unlock the achievement
        /// </summary>
        public IEnumerable<AchievementCondition> Conditions { get; private set; }

        /// <summary>
        ///     The Titel of the Achievement
        /// </summary>
        public string Titel { get; }

        /// <summary>
        ///     Description of the Achievement
        /// </summary>
        public string Description { get; }

        /// <summary>
        ///     Flag which shows if the Achievement is unlocked
        /// </summary>
        public bool Unlocked { get; private set; }

        /// <summary>
        ///     The current progress of the Achievement in percent
        /// </summary>
        public int Progress { get; private set; }

        /// <summary>
        ///     Path to the Image that is displayed in notifications etc.
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        ///     Gets the Hidden-Flag. Hidden Achievements reveal only when unlocked.
        /// </summary>
        public bool Hidden { get; set; }

        public DateTime UnlockTimeStamp { get; private set; }

        #endregion

        /// <summary>
        ///     Event that fires when the progress has changed
        /// </summary>
        [field: NonSerialized]
        public event AchievementProgressChangedHandler ProgressChanged;

        /// <summary>
        ///     Event that fires when the achievement is unlocked
        /// </summary>
        [field: NonSerialized]
        public event AchievementCompleteHandler AchievementCompleted;
    }
}