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
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Humanizer;

namespace sebingel.sharpchievements.View
{
    /// <summary>
    /// A control to display an Achievement
    /// </summary>
    public partial class AchievementControl : INotifyPropertyChanged
    {
        /// <summary>
        /// The Titel of the Achievement
        /// </summary>
        public string Titel { get; private set; }

        /// <summary>
        /// The Description of the Achievement
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Path to the Image that is displayed in the notification
        /// </summary>
        public string ImagePath { get; private set; }

        /// <summary>
        /// Visibility of the image
        /// </summary>
        public Visibility ImageVisibility { get; private set; }

        /// <summary>
        /// Progress in Percent
        /// </summary>
        public int Progress { get; private set; }

        /// <summary>
        /// Show or hide the Progress
        /// </summary>
        public Visibility ProgressVisibility { get; private set; }

        /// <summary>
        /// Progress in x/y format
        /// </summary>
        public string ProgressString { get; private set; }

        public string UnlockString { get; private set; }

        /// <summary>
        /// A control to display an Achievement
        /// </summary>
        /// <param name="achievement">Achievement to display</param>
        public AchievementControl(Achievement achievement)
        {
            if (achievement == null)
                throw new ArgumentNullException("achievement");

            this.InitializeComponent();

            // Set the title
            this.Titel = achievement.Titel;

            // set Description and Image according to the unlock status of the Achievement
            if (achievement.Unlocked)
            {
                this.Description = achievement.Description;
                this.ImagePath = achievement.ImagePath;

                // Display UnlockedString: If achievement was unlocked within the last six hours we "Humanize" the Timestamp.
                // If it was unlocked more than 6 hours ago we show only the day.
                if ((DateTime.UtcNow - achievement.UnlockTimeStamp) > new TimeSpan(6, 0, 0))
                    this.UnlockString = "Unlocked: " + achievement.UnlockTimeStamp.ToLocalTime().ToShortDateString();
                else
                    this.UnlockString = "Unlocked: " + achievement.UnlockTimeStamp.Humanize();
            }
            else
            {
                this.Description = String.Empty;
                this.ImagePath = "/sebingel.sharpchievements;component/Images/question30.png";
            }

            // Show or hide the image
            if (String.IsNullOrEmpty(achievement.ImagePath))
                this.ImageVisibility = Visibility.Collapsed;
            else
                this.ImageVisibility = Visibility.Visible;

            // Display the progress
            if (achievement.Conditions.Count() > 1 || achievement.Conditions.ToList()[0].CountToUnlock > 1)
            {
                this.Progress = achievement.Progress;
                this.ProgressVisibility = Visibility.Visible;

                // calculate the overall Progress of the Achievement in a X/Y format
                int overallNeeded = 0;
                int overallProgress = 0;
                foreach (AchievementCondition achievementCondition in achievement.Conditions)
                {
                    overallNeeded += achievementCondition.CountToUnlock;
                    overallProgress += achievementCondition.ProgressCount;
                }

                // AchievementConditions never stop counting. But we don't want a progress of 20/2 displayed.
                if (achievement.Unlocked)
                    overallProgress = overallNeeded;

                this.ProgressString = overallProgress + " / " + overallNeeded;
            }
            else
                this.ProgressVisibility = Visibility.Collapsed;

            this.InvokePropertyChanged("Titel");
            this.InvokePropertyChanged("Description");
            this.InvokePropertyChanged("ImagePath");
            this.InvokePropertyChanged("ImageVisibility");
            this.InvokePropertyChanged("Progress");
            this.InvokePropertyChanged("ProgressVisibility");
            this.InvokePropertyChanged("ProgressString");
            this.InvokePropertyChanged("UnlockString");
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        
        private void InvokePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
