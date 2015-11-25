using System.Collections.Generic;
using sebingel.sharpchievements;

namespace AchievementNotificationTest
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            AchievementManager achievementManager = AchievementManager.GetInstance();

            AchievementCondition clickConditionOne = new AchievementCondition("Click1", 3);
            Achievement a1 = new Achievement("Clicked ONE", "You clicked on ONE", clickConditionOne);

            AchievementCondition clickConditionTwo = new AchievementCondition("Click2", 1);
            Achievement a2 = new Achievement("Clicked TWO", "You clicked on TWO", clickConditionTwo);

            AchievementCondition clickConditionThree = new AchievementCondition("Click3", 1);
            Achievement a3 = new Achievement("Clicked THREE", "You clicked on THREE", clickConditionThree);

            AchievementCondition clickConditionFour = new AchievementCondition("Click1", 1);
            AchievementCondition clickConditionFive = new AchievementCondition("Click2", 3);
            Achievement a4 = new Achievement("SPECIAL!!!", "You clicked on ONE", new List<AchievementCondition> { clickConditionFour, clickConditionFive });

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
            new AchievementNotificationWindow(Left, Top, Width, Height, achievement).Show();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AchievementManager.GetInstance().ReportProgress("Click1");
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            AchievementManager.GetInstance().ReportProgress("Click2");
        }

        private void Button_Click_2(object sender, System.Windows.RoutedEventArgs e)
        {
            AchievementManager.GetInstance().ReportProgress("Click3");
        }
    }
}
