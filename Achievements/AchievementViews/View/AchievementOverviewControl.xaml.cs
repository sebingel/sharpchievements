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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using sebingel.sharpchievements.Annotations;

namespace sebingel.sharpchievements.AchievementViews.View
{
    /// <summary>
    /// A basic overview of a set of Achievements
    /// </summary>
    public partial class AchievementOverviewControl : INotifyPropertyChanged
    {
        private Visibility unlockedAchievementsVisibility;
        private Visibility separatorVisibility;
        private Visibility lockedAchievementsVisibility;

        /// <summary>
        /// A list containing Achievements that are displayed
        /// </summary>
        public ObservableCollection<Achievement> AchievementList { get; set; }

        /// <summary>
        /// State of the visibility of unlocked Achievements
        /// </summary>
        public Visibility UnlockedAchievementsVisibility
        {
            get
            {
                return unlockedAchievementsVisibility;
            }
            set
            {
                unlockedAchievementsVisibility = value;
                InvokePropertyChanged();
            }
        }

        /// <summary>
        /// State of the visibility of the Separator between locken and unlocked Achievements
        /// </summary>
        public Visibility SeparatorVisibility
        {
            get
            {
                return separatorVisibility;
            }
            set
            {
                separatorVisibility = value;
                InvokePropertyChanged();
            }
        }

        /// <summary>
        /// State of the visibility of locked Achievements
        /// </summary>
        public Visibility LockedAchievementsVisibility
        {
            get
            {
                return lockedAchievementsVisibility;
            }
            set
            {
                lockedAchievementsVisibility = value;
                InvokePropertyChanged();
            }
        }

        /// <summary>
        /// This construcor purely exists because a Usercontrol needs a parameterless constructor
        /// </summary>
        public AchievementOverviewControl()
            : this(null)
        { }

        /// <summary>
        /// A basic overview of a set of Achievements
        /// </summary>
        /// <param name="achievements">A set of Achievements to be desplyed</param>
        public AchievementOverviewControl(IEnumerable<Achievement> achievements)
        {
            InitializeComponent();

            if (achievements == null)
            {
                // If the Control is invoked without a list of Achivements to display it will register
                // to AchievementManager.Instance.AchievementsChanged and will load all registered Achievements
                AchievementList = new ObservableCollection<Achievement>();
                AchievementManager.Instance.AchievementsChanged += InstanceAchievementsChanged;
                return;
            }

            AchievementList = new ObservableCollection<Achievement>(achievements);
        }

        private void InstanceAchievementsChanged()
        {
            AchievementList = new ObservableCollection<Achievement>(AchievementManager.Instance.AchievementList);
        }

        /// <summary>
        /// Happens when an Achievement is changed in the AchievementManager
        /// </summary>
        public void Refresh()
        {
            IcUnlocked.Items.Clear();
            foreach (Achievement achievement in AchievementList.Where(x => x.Unlocked))
                IcUnlocked.Items.Add(new AchievementControl(achievement) { Margin = new Thickness(5) });

            IcLocked.Items.Clear();
            foreach (Achievement achievement in AchievementList.Where(x => !x.Unlocked))
                IcLocked.Items.Add(new AchievementControl(achievement) { Margin = new Thickness(5) });

            if (AchievementList.Any(x => x.Unlocked))
                UnlockedAchievementsVisibility = Visibility.Visible;
            else
                UnlockedAchievementsVisibility = Visibility.Collapsed;

            if (AchievementList.Any(x => !x.Unlocked))
                LockedAchievementsVisibility = Visibility.Visible;
            else
                LockedAchievementsVisibility = Visibility.Collapsed;

            if (AchievementList.Any(x => x.Unlocked) && AchievementList.Any(x => !x.Unlocked))
                SeparatorVisibility = Visibility.Visible;
            else
                SeparatorVisibility = Visibility.Collapsed;
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
