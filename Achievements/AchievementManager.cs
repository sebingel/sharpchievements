using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DansCSharpLibrary.Serialization;

namespace sebingel.sharpchievements
{
    /// <summary>
    /// Central management class for everything achievement related
    /// </summary>
    [Serializable]
    public class AchievementManager
    {
        private static AchievementManager instance;
        private readonly List<AchievementCondition> registeredAchievementConditions;
        private List<Achievement> registeredAchievements;

        public event AchievementCompleteHandler AchievementCompleted;
        private void InvokeAchievementCompleted(Achievement achievement)
        {
            if (AchievementCompleted != null)
                AchievementCompleted(achievement);
        }

        /// <summary>
        /// Central management class for everything achievement related
        /// </summary>
        private AchievementManager()
        {
            registeredAchievementConditions = new List<AchievementCondition>();
            registeredAchievements = new List<Achievement>();
        }

        /// <summary>
        /// Singleton access
        /// </summary>
        /// <returns>The only existing instance</returns>
        public static AchievementManager GetInstance()
        {
            if (instance == null)
                instance = new AchievementManager();
            return instance;
        }

        /// <summary>
        /// Register an AchievementCondition
        /// </summary>
        /// <remarks>Only registered AchievementConditions can be tracked!</remarks>
        /// <param name="achievementCondition">The AchievementCondition that should be registered</param>
        public void RegisterAchievementCondition(AchievementCondition achievementCondition)
        {
            registeredAchievementConditions.Add(achievementCondition);
        }

        /// <summary>
        /// Register an Achievement
        /// </summary>
        /// <remarks>The AchievementCompleted event only fires for registered Achievements!</remarks>
        /// <param name="achievement">The Achievement that should be registered</param>
        public void RegisterAchievement(Achievement achievement)
        {
            if (registeredAchievements.All(x => x.Titel != achievement.Titel))
                registeredAchievements.Add(achievement);
            achievement.AchievementCompleted += AchievementAchievementCompleted;
        }

        /// <summary>
        /// Fires the AchievementCompleted event of the AchievementManager
        /// </summary>
        /// <param name="achievement">Achievement that fired the AchievementCompleted event</param>
        private void AchievementAchievementCompleted(Achievement achievement)
        {
            InvokeAchievementCompleted(achievement);
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
                    condition.MakeProgress();
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

            registeredAchievements = BinarySerialization.ReadFromBinaryFile<List<Achievement>>(path);
            foreach (Achievement registeredAchievement in registeredAchievements)
            {
                foreach (AchievementCondition achievementCondition in registeredAchievement.Conditions)
                {
                    if (!registeredAchievementConditions.Contains(achievementCondition))
                        registeredAchievementConditions.Add(achievementCondition);
                }
            }

            return true;
        }
    }
}
