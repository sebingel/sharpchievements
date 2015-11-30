using System.Collections.Generic;
using sebingel.sharpchievements;
using sebingel.sharpchievements.AchievementViews.View;

namespace AchievementNotificationTest
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private AchievementNotificationWindow w;

        public MainWindow()
        {
            // for test purposes no saving or loading here

            InitializeComponent();

            AchievementManager achievementManager = AchievementManager.GetInstance();

            AchievementCondition clickConditionOne = new AchievementCondition("ClickedOne", "Click1", 3);
            Achievement a1 = new Achievement("Clicked ONE", "You clicked on ONE", clickConditionOne, "/sebingel.sharpchievements;component/Images/award71.png");

            AchievementCondition clickConditionTwo = new AchievementCondition("ClickedTwo", "Click2", 1);
            Achievement a2 = new Achievement("Clicked TWO", "You clicked on TWO", clickConditionTwo);

            AchievementCondition clickConditionThree = new AchievementCondition("ClickedThree", "Click3", 1);
            Achievement a3 = new Achievement("Clicked THREE", "You clicked on THREE", clickConditionThree);

            AchievementCondition clickConditionFour = new AchievementCondition("SpecialOne", "Click1", 1);
            AchievementCondition clickConditionFive = new AchievementCondition("SpecialTwo", "Click2", 3);
            Achievement a4 = new Achievement("SPECIAL!!!", "You clicked on ONE and on TWO several times!", new List<AchievementCondition> { clickConditionFour, clickConditionFive });

            achievementManager.RegisterAchievement(a1);
            achievementManager.RegisterAchievement(a2);
            achievementManager.RegisterAchievement(a3);
            achievementManager.RegisterAchievement(a4);

            achievementManager.RegisterAchievementCondition(clickConditionOne);
            achievementManager.RegisterAchievementCondition(clickConditionTwo);
            achievementManager.RegisterAchievementCondition(clickConditionThree);
            achievementManager.RegisterAchievementCondition(clickConditionFour);
            achievementManager.RegisterAchievementCondition(clickConditionFive);

            achievementManager.AchievementCompleted += AchievementManagerAchievementCompleted;
        }

        private void AchievementManagerAchievementCompleted(Achievement achievement)
        {
            // w = new AchievementNotificationWindow(achievement).Show();
            // w = new AchievementNotificationWindow(achievement,Left, Top, ActualWidth, ActualHeight).Show();
            // w = new AchievementNotificationWindow(achievement, 100, 100).Show();
            new AchievementNotificationWindow(achievement, this).Show();

            //w.Completed += W_Completed;
            //w.Show();
        }

        private void W_Completed(AchievementNotificationWindow w)
        {
            w.Completed -= W_Completed;
        }

        private void ButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            AchievementManager.GetInstance().ReportProgress("Click1");
        }

        private void ButtonClick1(object sender, System.Windows.RoutedEventArgs e)
        {
            AchievementManager.GetInstance().ReportProgress("Click2");
        }

        private void ButtonClick2(object sender, System.Windows.RoutedEventArgs e)
        {
            AchievementManager.GetInstance().ReportProgress("Click3");
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            if (w != null)
                w.Close();
        }
    }
}
