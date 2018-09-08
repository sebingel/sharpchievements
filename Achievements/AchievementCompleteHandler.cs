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
    ///     Delegate for an event that fires when the achievement is unlocked
    /// </summary>
    /// <param name="achievement">The achievement that fired the event</param>
    public delegate void AchievementCompleteHandler(Achievement achievement);
}