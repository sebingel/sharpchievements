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
using sebingel.sharpchievements.Annotations;

namespace sebingel.sharpchievements.AchievementViews.View
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

        public int Progress { get; private set; }

        public Visibility ProgressVisibility { get; private set; }

        /// <summary>
        /// A control to display an Achievement
        /// </summary>
        public AchievementControl()
            : this(null)
        { }

        /// <summary>
        /// A control to display an Achievement
        /// </summary>
        /// <param name="achievement">Achievement to display</param>
        public AchievementControl([NotNull] Achievement achievement)
        {
            if (achievement == null)
                throw new ArgumentNullException("achievement");

            InitializeComponent();

            Titel = achievement.Titel;

            if (achievement.Unlocked)
            {
                Description = achievement.Description;
                ImagePath = achievement.ImagePath;
            }
            else
            {
                Description = String.Empty;
                ImagePath = "/sebingel.sharpchievements;component/Images/question30.png";
            }

            if (String.IsNullOrEmpty(achievement.ImagePath))
                ImageVisibility = Visibility.Collapsed;
            else
                ImageVisibility = Visibility.Visible;

            if (achievement.Conditions.Any() || achievement.Conditions.ToList()[0].CountToUnlock > 1)
            {
                Progress = achievement.Progress;
                ProgressVisibility = Visibility.Visible;
            }
            else
                ProgressVisibility = Visibility.Collapsed;

            InvokePropertyChanged("Titel");
            InvokePropertyChanged("Description");
            InvokePropertyChanged("ImagePath");
            InvokePropertyChanged("ImageVisibility");
            InvokePropertyChanged("Progress");
            InvokePropertyChanged("ProgressVisibility");
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void InvokePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
