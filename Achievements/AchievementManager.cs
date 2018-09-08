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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using DansCSharpLibrary.Serialization;

namespace sebingel.sharpchievements
{
    /// <summary>
    ///     Central management class for everything achievement related
    /// </summary>
    public class AchievementManager
    {
        #region - Felder privat -

        private readonly List<AchievementCondition> _registeredAchievementConditions;
        private List<Achievement> _registeredAchievements;

        #endregion

        #region - Konstruktoren -

        /// <summary>
        ///     List of Achievements
        /// </summary>
        public AchievementManager()
        {
            this._registeredAchievementConditions = new List<AchievementCondition>();
            this._registeredAchievements = new List<Achievement>();
        }

        #endregion

        #region - Methoden oeffentlich -

        /// <summary>
        ///     Register an AchievementCondition
        /// </summary>
        /// <remarks>Only registered AchievementConditions can be tracked!</remarks>
        /// <param name="achievementCondition">The AchievementCondition that should be registered</param>
        public void RegisterAchievementCondition(AchievementCondition achievementCondition)
        {
            if (this._registeredAchievementConditions.All(x => x.UniqueId != achievementCondition.UniqueId))
            {
                this._registeredAchievementConditions.Add(achievementCondition);
            }
        }

        /// <summary>
        ///     Register an Achievement
        /// </summary>
        /// <remarks>The AchievementCompleted event only fires for registered Achievements!</remarks>
        /// <param name="achievement">The Achievement that should be registered</param>
        public void RegisterAchievement(Achievement achievement)
        {
            if (this._registeredAchievements.All(x => x.UniqueId != achievement.UniqueId))
            {
                this._registeredAchievements.Add(achievement);
                achievement.AchievementCompleted += this.AchievementAchievementCompleted;

                this.InvokeAchievementsChanged();
                this.InvokeAchievementRegistered(achievement);
            }
        }

        /// <summary>
        ///     Report progress of an AchievementCondition
        /// </summary>
        /// <param name="achviementConditionKey">The AchievementCondition that should make a progress</param>
        public void ReportProgress(string achviementConditionKey)
        {
            foreach (var condition in this._registeredAchievementConditions)
            {
                if (condition.AchievementConditionKey == achviementConditionKey)
                {
                    condition.MakeProgress();
                    this.InvokeAchievementsChanged();
                }
            }
        }

        /// <summary>
        ///     Saves the achievements to the disk
        /// </summary>
        /// <param name="path">Path to save the file to</param>
        public void SaveAchiements(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new AchievementSaveException("Can not save to empty string.");
            }

            var sPathDirectory = Path.GetDirectoryName(path);

            if (string.IsNullOrEmpty(sPathDirectory))
            {
                throw new AchievementSaveException("\"" + path + "\" is not a valid path.");
            }

            if (!Directory.Exists(sPathDirectory))
            {
                try
                {
                    Directory.CreateDirectory(sPathDirectory);
                }
                catch (Exception e)
                {
                    throw new AchievementSaveException(
                        "Directory \"" + sPathDirectory + "\" does not exist.\nFailed to create directory \"" +
                        sPathDirectory + "\".\n" + e.Message, e);
                }
            }

            var sFilename = Path.GetFileName(path);

            if (string.IsNullOrEmpty(sFilename))
            {
                throw new AchievementSaveException("\"" + path + "\" contains no filename.");
            }

            BinarySerialization.WriteToBinaryFile(Path.Combine(sPathDirectory, sFilename), this._registeredAchievements);
        }

        /// <summary>
        ///     Loads the achievements from the disk
        /// </summary>
        /// <param name="path">Path to load from</param>
        /// <param name="suppressFileNotFound">true if you want to supress a FileNotFoundException</param>
        public bool LoadAchievements(string path, bool suppressFileNotFound)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new AchievementSaveException("Can not load from empty string.");
            }

            if (!File.Exists(path))
            {
                if (!suppressFileNotFound)
                {
                    throw new FileNotFoundException("File not found.", path);
                }

                return false;
            }

            // retain compatibility with pre-strong-named assembly
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;
            this._registeredAchievements = BinarySerialization.ReadFromBinaryFile<List<Achievement>>(path);
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomainAssemblyResolve;

            foreach (var registeredAchievement in this._registeredAchievements)
            {
                registeredAchievement.AchievementCompleted += this.AchievementAchievementCompleted;

                foreach (var achievementCondition in registeredAchievement.Conditions)
                {
                    if (!this._registeredAchievementConditions.Contains(achievementCondition))
                    {
                        this._registeredAchievementConditions.Add(achievementCondition);
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///     Deletes an Achievement by its UniqueId
        /// </summary>
        /// <param name="uniqueId">UniqueId of Achievement that should be deleted</param>
        public void DeleteAchievementByUniqueId(string uniqueId)
        {
            var a = this._registeredAchievements.Find(x => x.UniqueId == uniqueId);
            if (a != null)
            {
                this._registeredAchievements.Remove(a);
                a.AchievementCompleted -= this.AchievementAchievementCompleted;
                a.Clear();
                // Do not delete all unusedAchievementConditions automatically to prevent progress loss
                //CleanUpAchievementConditions();
            }
        }

        /// <summary>
        ///     Deletes every Achievement and every AchievementCondition
        /// </summary>
        public void Reset()
        {
            foreach (var registeredAchievement in this._registeredAchievements)
            {
                registeredAchievement.AchievementCompleted -= this.AchievementAchievementCompleted;
                registeredAchievement.Clear();
            }

            this._registeredAchievements.Clear();
            this._registeredAchievementConditions.Clear();
        }

        /// <summary>
        ///     Checks the status of all locked Achievements by checking all assigned AchievementConditions and determines if the
        ///     Achievement unlocks
        /// </summary>
        public void ReevaluateUnlockStatus()
        {
            this.RegisteredAchievements.ToList().FindAll(x => !x.Unlocked).ForEach(x => x.CheckUnlockStatus());
        }

        #endregion

        #region - Methoden privat -

        private void InvokeAchievementCompleted(Achievement achievement)
        {
            if (this.AchievementCompleted != null)
            {
                this.AchievementCompleted(achievement);
            }
        }

        private void InvokeAchievementsChanged()
        {
            if (this.AchievementsChanged != null)
            {
                this.AchievementsChanged();
            }
        }

        private void InvokeAchievementRegistered(Achievement a)
        {
            if (this.AchievementRegistered != null)
            {
                this.AchievementRegistered(a);
            }
        }

        /// <summary>
        ///     Fires the AchievementCompleted event of the AchievementManager
        /// </summary>
        /// <param name="achievement">Achievement that fired the AchievementCompleted event</param>
        private void AchievementAchievementCompleted(Achievement achievement)
        {
            this.InvokeAchievementCompleted(achievement);
            this.InvokeAchievementsChanged();
        }

        private static Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args) => Assembly.GetAssembly(typeof(Achievement));

        /// <summary>
        ///     Removes no longer needed AchievementConditions
        /// </summary>
        private void CleanUpAchievementConditions()
        {
            var toDelete = new List<AchievementCondition>();
            foreach (var achievementCondition in this._registeredAchievementConditions)
            {
                if (this._registeredAchievements.Find(x => x.Conditions.Contains(achievementCondition)) == null)
                {
                    toDelete.Add(achievementCondition);
                }
            }

            foreach (var achievementCondition in toDelete)
            {
                this._registeredAchievementConditions.Remove(achievementCondition);
            }
        }

        #endregion

        #region - Properties oeffentlich -

        /// <summary>
        ///     Returns a ReadOnlyCollection of all registered Achievements
        /// </summary>
        /// <returns>A ReadOnlyCollection of all registered Achievements</returns>
        public ReadOnlyCollection<Achievement> RegisteredAchievements => new ReadOnlyCollection<Achievement>(this._registeredAchievements);

        #endregion

        public event AchievementCompleteHandler AchievementCompleted;

        /// <summary>
        ///     Event that fires when anything is changed in any registered Achievement
        /// </summary>
        public event Action AchievementsChanged;

        /// <summary>
        ///     Event that fires when a new Achievement is registered
        /// </summary>
        public event AchievementRegisteredHandler AchievementRegistered;
    }
}