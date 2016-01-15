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
using System.ComponentModel;
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
        private List<Achievement> achievementList;
        private Visibility unlockedAchievementsVisibility;
        private Visibility separatorVisibility;
        private Visibility lockedAchievementsVisibility;

        /// <summary>
        /// A list containing Achievements that are displayed
        /// </summary>
        public List<Achievement> AchievementList
        {
            get
            {
                return achievementList;
            }
            set
            {
                achievementList = value;
                InvokePropertyChanged();
            }
        }

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
        /// A basic overview of all registered Achievements
        /// </summary>
        public AchievementOverviewControl()
            : this(null)
        {
            InitializeComponent();
            Refresh();

            // Disabled due to performance problems
            // When called with no parameters the control will keep track of all registered Achievements by subscribing the according event in the AchievementManager
            //AchievementManager.Instance().AchievementsChanged += AchievementOverViewControlAchievementsChanged;
        }

        /// <summary>
        /// A basic overview of a set of Achievements
        /// </summary>
        /// <param name="achievementList">A set of Achievements to be desplyed</param>
        public AchievementOverviewControl(List<Achievement> achievementList)
        {
            InitializeComponent();
            AchievementList = achievementList;
        }

        /// <summary>
        /// Happens when an Achievement is changed in the AchievementManager
        /// </summary>
        public void Refresh()
        {
            AchievementList = new List<Achievement>(AchievementManager.Instance.AchievementList);

            IcUnlocked.Items.Clear();
            foreach (Achievement achievement in AchievementList.FindAll(x => x.Unlocked))
                IcUnlocked.Items.Add(new AchievementControl(achievement) { Margin = new Thickness(5) });

            IcLocked.Items.Clear();
            foreach (Achievement achievement in AchievementList.FindAll(x => !x.Unlocked))
                IcLocked.Items.Add(new AchievementControl(achievement) { Margin = new Thickness(5) });

            if (AchievementList.FindAll(x => x.Unlocked).Count > 0)
                UnlockedAchievementsVisibility = Visibility.Visible;
            else
                UnlockedAchievementsVisibility = Visibility.Collapsed;

            if (AchievementList.FindAll(x => !x.Unlocked).Count > 0)
                LockedAchievementsVisibility = Visibility.Visible;
            else
                LockedAchievementsVisibility = Visibility.Collapsed;

            if (AchievementList.FindAll(x => x.Unlocked).Count > 0 && AchievementList.FindAll(x => !x.Unlocked).Count > 0)
                SeparatorVisibility = Visibility.Visible;
            else
                SeparatorVisibility = Visibility.Collapsed;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void InvokePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
