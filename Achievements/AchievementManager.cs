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
    /// Central management class for everything achievement related
    /// </summary>
    public class AchievementManager
    {
        #region Fields
        
        private readonly List<AchievementCondition> registeredAchievementConditions;
        private List<Achievement> registeredAchievements;

        #endregion

        #region Events

        public event AchievementCompleteHandler AchievementCompleted;

        private void InvokeAchievementCompleted(Achievement achievement)
        {
            if (AchievementCompleted != null)
                AchievementCompleted(achievement);
        }

        /// <summary>
        /// Event that fires when anything is changed in any registered Achievement
        /// </summary>
        public event Action AchievementsChanged;

        private void InvokeAchievementsChanged()
        {
            if (AchievementsChanged != null)
                AchievementsChanged();
        }

        /// <summary>
        /// Event that fires when a new Achievement is registered
        /// </summary>
        public event AchievementRegisteredHandler AchievementRegistered;

        private void InvokeAchievementRegistered(Achievement a)
        {
            if (AchievementRegistered != null)
                AchievementRegistered(a);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns a ReadOnlyCollection of all registered Achievements
        /// </summary>
        /// <returns>A ReadOnlyCollection of all registered Achievements</returns>
        public ReadOnlyCollection<Achievement> AchievementList
        {
            get
            {
                return new ReadOnlyCollection<Achievement>(registeredAchievements);
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Central management class for everything achievement related
        /// </summary>
        public AchievementManager()
        {
            registeredAchievementConditions = new List<AchievementCondition>();
            registeredAchievements = new List<Achievement>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Register an AchievementCondition
        /// </summary>
        /// <remarks>Only registered AchievementConditions can be tracked!</remarks>
        /// <param name="achievementCondition">The AchievementCondition that should be registered</param>
        public void RegisterAchievementCondition(AchievementCondition achievementCondition)
        {
            if (registeredAchievementConditions.All(x => x.UniqueId != achievementCondition.UniqueId))
                registeredAchievementConditions.Add(achievementCondition);
        }

        /// <summary>
        /// Register an Achievement
        /// </summary>
        /// <remarks>The AchievementCompleted event only fires for registered Achievements!</remarks>
        /// <param name="achievement">The Achievement that should be registered</param>
        public void RegisterAchievement(Achievement achievement)
        {
            if (registeredAchievements.All(x => x.UniqueId != achievement.UniqueId))
            {
                registeredAchievements.Add(achievement);
                achievement.AchievementCompleted += AchievementAchievementCompleted;

                InvokeAchievementsChanged();
                InvokeAchievementRegistered(achievement);
            }
        }

        /// <summary>
        /// Fires the AchievementCompleted event of the AchievementManager
        /// </summary>
        /// <param name="achievement">Achievement that fired the AchievementCompleted event</param>
        private void AchievementAchievementCompleted(Achievement achievement)
        {
            InvokeAchievementCompleted(achievement);
            InvokeAchievementsChanged();
        }

        /// <summary>
        /// Report progress of an AchievementCondition
        /// </summary>
        /// <param name="achviementConditionKey">The AchievementCondition that should make a progress</param>
        public void ReportProgress(string achviementConditionKey)
        {
            foreach (AchievementCondition condition in registeredAchievementConditions)
            {
                if (condition.AchievementConditionKey == achviementConditionKey)
                {
                    condition.MakeProgress();
                    InvokeAchievementsChanged();
                }
            }
        }

        /// <summary>
        /// Saves the achievements to the disk
        /// </summary>
        /// <param name="path">Path to save the file to</param>
        public void SaveAchiements(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new AchievementSaveException("Can not save to empty string.");

            string sPathDirectory = Path.GetDirectoryName(path);

            if (String.IsNullOrEmpty(sPathDirectory))
                throw new AchievementSaveException("\"" + path + "\" is not a valid path.");

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

            string sFilename = Path.GetFileName(path);

            if (String.IsNullOrEmpty(sFilename))
                throw new AchievementSaveException("\"" + path + "\" contains no filename.");

            BinarySerialization.WriteToBinaryFile(Path.Combine(sPathDirectory, sFilename), registeredAchievements);
        }

        /// <summary>
        /// Loads the achievements from the disk
        /// </summary>
        /// <param name="path">Path to load from</param>
        /// <param name="suppressFileNotFound">true if you want to supress a FileNotFoundException</param>
        public bool LoadAchievements(string path, bool suppressFileNotFound)
        {
            if (String.IsNullOrEmpty(path))
                throw new AchievementSaveException("Can not load from empty string.");

            if (!File.Exists(path))
            {
                if (!suppressFileNotFound)
                    throw new FileNotFoundException("File not found.", path);

                return false;
            }

            // retain compatibility with pre-strong-named assembly
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;
            registeredAchievements = BinarySerialization.ReadFromBinaryFile<List<Achievement>>(path);
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomainAssemblyResolve;

            foreach (Achievement registeredAchievement in registeredAchievements)
            {
                registeredAchievement.AchievementCompleted += AchievementAchievementCompleted;

                foreach (AchievementCondition achievementCondition in registeredAchievement.Conditions)
                {
                    if (!registeredAchievementConditions.Contains(achievementCondition))
                        registeredAchievementConditions.Add(achievementCondition);
                }
            }

            return true;
        }

        private static Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.GetAssembly(typeof (Achievement));
        }

        /// <summary>
        /// Deletes an Achievement by its UniqueId
        /// </summary>
        /// <param name="uniqueId">UniqueId of Achievement that should be deleted</param>
        public void DeleteAchievementByUniqueId(string uniqueId)
        {
            Achievement a = registeredAchievements.Find(x => x.UniqueId == uniqueId);
            if (a != null)
            {
                registeredAchievements.Remove(a);
                a.AchievementCompleted -= AchievementAchievementCompleted;
                a.Clear();
                // Do not delete all unusedAchievementConditions automatically to prevent progress loss
                //CleanUpAchievementConditions();
            }
        }

        /// <summary>
        /// Removes no longer needed AchievementConditions
        /// </summary>
        private void CleanUpAchievementConditions()
        {
            List<AchievementCondition> toDelete = new List<AchievementCondition>();
            foreach (AchievementCondition achievementCondition in registeredAchievementConditions)
            {
                if (registeredAchievements.Find(x => x.Conditions.Contains(achievementCondition)) == null)
                    toDelete.Add(achievementCondition);
            }

            foreach (AchievementCondition achievementCondition in toDelete)
                registeredAchievementConditions.Remove(achievementCondition);
        }

        /// <summary>
        /// Deletes every Achievement and every AchievementCondition
        /// </summary>
        public void Reset()
        {
            foreach (Achievement registeredAchievement in registeredAchievements)
            {
                registeredAchievement.AchievementCompleted -= AchievementAchievementCompleted;
                registeredAchievement.Clear();
            }

            registeredAchievements.Clear();
            registeredAchievementConditions.Clear();
        }

        /// <summary>
        /// Checks the status of all locked Achievements by checking all assigned AchievementConditions and determines if the Achievement unlocks
        /// </summary>
        public void ReevaluateUnlockStatus()
        {
            AchievementList.ToList().FindAll(x => !x.Unlocked).ForEach(x => x.CheckUnlockStatus());
        }

        #endregion
    }
}