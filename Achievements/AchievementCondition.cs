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

namespace sebingel.sharpchievements
{
    /// <summary>
    ///     Condition that describes the requirements that must be met to unlock an achievement and can track the
    ///     iProgressCount.
    /// </summary>
    [Serializable]
    public class AchievementCondition
    {
        #region - Konstruktoren -

        /// <summary>
        ///     Condition that describes the requirements that must be met to unlock an achievement and can track the
        ///     iProgressCount.
        /// </summary>
        /// <param name="uniqueId">Applicationwide Unique uniqueId of the AchievementCondition</param>
        /// <param name="achievementConditionKey">
        ///     Key of this AchivementCondition. Is used to identify one ore more
        ///     AchievementConditions by the AchievementManager.
        /// </param>
        /// <param name="countToUnlock">Sets the number of Calls until this AchievementCondtion counts as completed.</param>
        public AchievementCondition(string uniqueId, string achievementConditionKey, int countToUnlock)
        {
            this.UniqueId = uniqueId;
            this.AchievementConditionKey = achievementConditionKey;
            this.CountToUnlock = countToUnlock;
            this.Unlocked = false;
        }

        #endregion

        #region - Methoden privat -

        private void InvokeProgressChanged(int iProgressCount)
        {
            this.ProgressChanged?.Invoke(this, new AchievementConditionProgressChangedArgs(iProgressCount));
        }

        private void InvokeConditionCompleted()
        {
            this.Unlocked = true;
            this.ConditionCompleted?.Invoke(this);
        }

        #endregion

        #region - Properties oeffentlich -

        /// <summary>
        ///     Applicationwide Unique uniqueId of the AchievementCondition
        /// </summary>
        public string UniqueId { get; set; }

        /// <summary>
        ///     Key of this AchivementCondition. Is used to identify one ore more AchievementConditions by the AchievementManager.
        /// </summary>
        public string AchievementConditionKey { get; }

        /// <summary>
        ///     Gets the Unlocked status of an AchievementCondition.
        /// </summary>
        public bool Unlocked { get; private set; }

        /// <summary>
        ///     The current progress of the AchievementCondition in percent
        /// </summary>
        public int Progress => 100 / this.CountToUnlock * this.ProgressCount;

        /// <summary>
        ///     The current absoute progress of the AchievementCondition
        /// </summary>
        public int ProgressCount { get; private set; }

        /// <summary>
        ///     Gets the number of progresses that needs to be made to complete this AchievementCondition
        /// </summary>
        public int CountToUnlock { get; }

        #endregion

        #region AchievementCondition Members

        /// <summary>
        ///     Event that fires when the iProgressCount of an AchievementCondition is changed.
        /// </summary>
        public event AchievementConditionProgressChangedHandler ProgressChanged;

        /// <summary>
        ///     Event that fires when an AchievementCondition is completed.
        /// </summary>
        public event AchievementConditionCompletedHandler ConditionCompleted;

        /// <summary>
        ///     Adds one iProgressCount step for this AchievementCondition
        /// </summary>
        public void MakeProgress()
        {
            this.ProgressCount++;
            this.InvokeProgressChanged(this.ProgressCount);

            if (this.ProgressCount >= this.CountToUnlock)
            {
                this.InvokeConditionCompleted();
            }
        }

        #endregion
    }
}