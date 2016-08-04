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
    /// Delegate of the event that fires when the progress of an AchievementCondition is changed
    /// </summary>
    /// <param name="sender">The AchievementCondition which progress is changed</param>
    /// <param name="args">Parameters containing further information of the progress change</param>
    public delegate void AchievementConditionProgressChangedHandler(IAchievementCondition sender, AchievementConditionProgressChangedArgs args);
}
