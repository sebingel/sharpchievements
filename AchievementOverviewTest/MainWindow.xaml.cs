using System.Windows;
using sebingel.sharpchievements;

namespace AchievementOverviewTest
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AchievementManager am = AchievementManager.GetInstance();

            AchievementCondition con1 = new AchievementCondition("con1", "con1", 1);
            Achievement a1 = new Achievement("a1", "a1Desc", con1);
            Achievement a2 = new Achievement("a2", "a2Desc\nWrap", con1);
            Achievement a3 = new Achievement("a3", "a2Desc\nWrap", con1);
            Achievement a4 = new Achievement("a4", "a2Desc\nWrap", con1);
            Achievement a5 = new Achievement("a5", "a2Desc\nWrap", con1);
            Achievement a6 = new Achievement("a6", "a2Desc\nWrap", con1);
            Achievement a7 = new Achievement("a7", "a2Desc\nWrap", con1);
            Achievement a8 = new Achievement("a8", "a2Desc\nWrap", con1);
            Achievement a9 = new Achievement("a9", "a2Desc\nWrap", con1);
            Achievement a10 = new Achievement("a10", "a2Desc\nWrap", con1);
            Achievement a11 = new Achievement("a11", "a2Desc\nWrap", con1);

            am.RegisterAchievementCondition(con1);
            am.RegisterAchievement(a1);
            am.RegisterAchievement(a2);
            am.RegisterAchievement(a3);
            am.RegisterAchievement(a4);
            am.RegisterAchievement(a5);
            am.RegisterAchievement(a6);
            am.RegisterAchievement(a7);
            am.RegisterAchievement(a8);
            am.RegisterAchievement(a9);
            am.RegisterAchievement(a10);
            am.RegisterAchievement(a11);
        }
    }
}
