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
    /// Condition that describes the requirements that must be met to unlock an achievement and can track the iProgressCount.
    /// </summary>
    [Serializable]
    public class AchievementCondition : IAchievementCondition
    {
        private readonly int countToUnlock;
        private int progressCount;

        /// <summary>
        /// Applicationwide Unique uniqueId of the AchievementCondition
        /// </summary>
        public string UniqueId { get; set; }

        /// <summary>
        /// Key of this AchivementCondition. Is used to identify one ore more AchievementConditions by the AchievementManager.
        /// </summary>
        public string AchievementConditionKey { get; private set; }

        /// <summary>
        /// Gets the Unlocked status of an AchievementCondition.
        /// </summary>
        public bool Unlocked { get; private set; }

        /// <summary>
        /// The current progress of the AchievementCondition in percent
        /// </summary>
        public int Progress
        {
            get
            {
                return 100 / countToUnlock * progressCount;
            }
        }

        /// <summary>
        /// The current absoute progress of the AchievementCondition
        /// </summary>
        public int ProgressCount
        {
            get
            {
                return progressCount;
            }
        }

        /// <summary>
        /// Gets the number of progresses that needs to be made to complete this AchievementCondition
        /// </summary>
        public int CountToUnlock
        {
            get
            {
                return countToUnlock;
            }
        }

        /// <summary>
        /// Event that fires when the iProgressCount of an AchievementCondition is changed.
        /// </summary>
        public event AchievementConditionProgressChangedHandler ProgressChanged;
        private void InvokeProgressChanged(int iProgressCount)
        {
            if (ProgressChanged != null)
                ProgressChanged(this, new AchievementConditionProgressChangedArgs(iProgressCount));
        }

        /// <summary>
        /// Event that fires when an AchievementCondition is completed.
        /// </summary>
        public event AchievementConditionCompletedHandler ConditionCompleted;
        private void InvokeConditionCompleted()
        {
            Unlocked = true;
            if (ConditionCompleted != null)
                ConditionCompleted(this);
        }

        /// <summary>
        /// Condition that describes the requirements that must be met to unlock an achievement and can track the iProgressCount.
        /// </summary>
        /// <param name="uniqueId">Applicationwide Unique uniqueId of the AchievementCondition</param>
        /// <param name="achievementConditionKey">Key of this AchivementCondition. Is used to identify one ore more AchievementConditions by the AchievementManager.</param>
        /// <param name="countToUnlock">Sets the number of Calls until this AchievementCondtion counts as completed.</param>
        public AchievementCondition(string uniqueId, string achievementConditionKey, int countToUnlock)
        {
            UniqueId = uniqueId;
            AchievementConditionKey = achievementConditionKey;
            this.countToUnlock = countToUnlock;
            Unlocked = false;
        }

        /// <summary>
        /// Adds one iProgressCount step for this AchievementCondition
        /// </summary>
        public void MakeProgress()
        {
            progressCount++;
            InvokeProgressChanged(progressCount);

            if (progressCount >= countToUnlock)
                InvokeConditionCompleted();
        }
    }
}
