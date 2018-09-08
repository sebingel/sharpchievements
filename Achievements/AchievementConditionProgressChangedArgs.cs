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

namespace sebingel.sharpchievements
{
    /// <summary>
    ///     Contains information about a progresschange of an AchievementCondition
    /// </summary>
    public class AchievementConditionProgressChangedArgs
    {
        #region - Konstruktoren -

        /// <summary>
        ///     Contains information about a progresschange of an AchievementCondition
        /// </summary>
        /// <param name="progressCount">Current progress of the AchievementCondition</param>
        public AchievementConditionProgressChangedArgs(int progressCount) => this.ProgressCount = progressCount;

        #endregion

        #region - Properties oeffentlich -

        /// <summary>
        ///     Current progress of the AchievementCondition
        /// </summary>
        public int ProgressCount { get; }

        #endregion
    }
}