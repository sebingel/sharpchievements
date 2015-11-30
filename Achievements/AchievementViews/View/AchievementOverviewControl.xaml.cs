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
        /// A basic overview of all registered Achievements
        /// </summary>
        public AchievementOverviewControl()
            : this(null)
        {
            InitializeComponent();
            AchievementOverViewControlAchievementsChanged();

            // When called with no parameters the control will keep track of all registered Achievements by subscribing the according event in the AchievementManager
            AchievementManager.GetInstance().AchievementsChanged += AchievementOverViewControlAchievementsChanged;
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
        private void AchievementOverViewControlAchievementsChanged()
        {
            AchievementList = new List<Achievement>(AchievementManager.GetInstance().GetAchievementList());

            ic.Items.Clear();
            foreach (Achievement achievement in AchievementList)
            {
                ic.Items.Add(new AchievementControl(achievement) { Margin = new Thickness(5) });
            }
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
