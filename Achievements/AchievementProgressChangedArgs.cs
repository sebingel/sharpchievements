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
    ///     This class contains data that about the progresschange of an Achievement
    /// </summary>
    public class AchievementProgressChangedArgs
    {
        #region - Konstruktoren -

        /// <summary>
        ///     This class contains data that about the progresschange of an Achievement
        /// </summary>
        /// <param name="progressCount">Current progress of the Achievement</param>
        public AchievementProgressChangedArgs(int progressCount) => this.ProgressCount = progressCount;

        #endregion

        #region - Properties oeffentlich -

        /// <summary>
        ///     Current progress of the Achievement
        /// </summary>
        public int ProgressCount { get; }

        #endregion
    }
}