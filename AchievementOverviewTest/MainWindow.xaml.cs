using System.Windows;
using sebingel.sharpchievements;
using sebingel.sharpchievements.AchievementViews.View;

namespace AchievementOverviewTest
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AchievementManager am;

        public MainWindow()
        {
            InitializeComponent();

            am = new AchievementManager();

            AchievementCondition con1 = new AchievementCondition("con1", "con1", 1);
            AchievementCondition con2 = new AchievementCondition("con2", "con2", 2);
            AchievementCondition con3 = new AchievementCondition("con3", "con3", 1);
            Achievement a1 = new Achievement("a1", "a1", "a1Desc", con1) { ImagePath = "/sebingel.sharpchievements;component/Images/award71.png" };
            Achievement a2 = new Achievement("a2", "a2", "a2Desc\nWrap", con2) { ImagePath = "/sebingel.sharpchievements;component/Images/award71.png" };
            Achievement a3 = new Achievement("a3", "a3", "a2Desc\nWrap", con3) { ImagePath = "/sebingel.sharpchievements;component/Images/award71.png", Hidden = true };
            //Achievement a4 = new Achievement("a4", "a4", "a2Desc\nWrap", con1, "/sebingel.sharpchievements;component/Images/question30.png");
            //Achievement a5 = new Achievement("a5", "a5", "a2Desc\nWrap", con1, "/sebingel.sharpchievements;component/Images/award71.png");
            //Achievement a6 = new Achievement("a6", "a6", "a2Desc\nWrap", con1, "/sebingel.sharpchievements;component/Images/award71.png");
            //Achievement a7 = new Achievement("a7", "a7", "a2Desc\nWrap", con1, "/sebingel.sharpchievements;component/Images/award71.png");
            //Achievement a8 = new Achievement("a8", "a8", "a2Desc\nWrap", con1, "/sebingel.sharpchievements;component/Images/award71.png");
            //Achievement a9 = new Achievement("a9", "a9", "a2Desc\nWrap", con1, "/sebingel.sharpchievements;component/Images/award71.png");
            //Achievement a10 = new Achievement("a10", "a10", "a2Desc\nWrap", con1, "/sebingel.sharpchievements;component/Images/award71.png");
            //Achievement a11 = new Achievement("a11", "a11", "a2Desc\nWrap", con1, "/sebingel.sharpchievements;component/Images/award71.png");

            am.RegisterAchievementCondition(con1);
            am.RegisterAchievementCondition(con2);
            am.RegisterAchievementCondition(con3);
            am.RegisterAchievement(a1);
            am.RegisterAchievement(a2);
            am.RegisterAchievement(a3);
            //am.RegisterAchievement(a4);
            //am.RegisterAchievement(a5);
            //am.RegisterAchievement(a6);
            //am.RegisterAchievement(a7);
            //am.RegisterAchievement(a8);
            //am.RegisterAchievement(a9);
            //am.RegisterAchievement(a10);
            //am.RegisterAchievement(a11);

            am.AchievementCompleted += AmAchievementCompleted;

            AchievementOverviewControl.Refresh();
        }

        private void AmAchievementCompleted(Achievement achievement)
        {
            new AchievementNotificationWindow(achievement, this).Show();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            am.ReportProgress("con2");
            AchievementOverviewControl.Refresh();
        }

        private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
        {
            am.ReportProgress("con1");
            AchievementOverviewControl.Refresh();
        }

        private void ButtonBase_OnClick3(object sender, RoutedEventArgs e)
        {
            am.ReportProgress("con3");
            AchievementOverviewControl.Refresh();
        }
    }
}
