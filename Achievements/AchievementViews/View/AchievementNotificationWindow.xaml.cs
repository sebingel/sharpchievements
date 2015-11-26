// Inspired by https://stackoverflow.com/questions/3034741/create-popup-toaster-notifications-in-windows-with-net

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Point = System.Windows.Point;

namespace sebingel.sharpchievements.AchievementViews.View
{
    /// <summary>
    /// Notification Window that can be used to inform the user aabout unlocked achievements
    /// </summary>
    public partial class AchievementNotificationWindow
    {
        /// <summary>
        /// The Titel of the Achievement
        /// </summary>
        public string Titel { get; private set; }

        /// <summary>
        /// Path to the Image that is displayed in the notification
        /// </summary>
        public string ImagePath { get; private set; }

        /// <summary>
        /// Visibility of the image
        /// </summary>
        public Visibility ImageVisibility { get; private set; }

        /// <summary>
        /// Notification Window that can be used to inform the user about unlocked achievements.
        /// Shows the AchievementNotificationWindow in the bottom right corner of the given coordinates.
        /// </summary>
        /// <remarks>This has some problems with maximized windows</remarks>
        /// <param name="achievement">The unlocked Achievement</param>
        /// <param name="left">The position of the left border of the application's GUI</param>
        /// <param name="top">The position of the top border of the application's GUI</param>
        /// <param name="width">The width of the application's GUI</param>
        /// <param name="height">The height of the application's GUI</param>
        public AchievementNotificationWindow(Achievement achievement, double left, double top, double width, double height)
        {
            Initialize(left, top, width, height, achievement);
        }

        /// <summary>
        /// Notification Window that can be used to inform the user about unlocked achievements.
        /// Shows the AchievementNotificationWindow in the bottom right corner of the given Window.
        /// </summary>
        /// <param name="achievement">The unlocked Achievement</param>
        /// <param name="window">The Window in which the Notification should be displayed</param>
        public AchievementNotificationWindow(Achievement achievement, Window window)
        {
            double left;
            double top;
            if (window.WindowState == WindowState.Maximized)
            {
                left = 0;
                top = 0;
            }
            else
            {
                left = window.Left;
                top = window.Top;
            }

            Initialize(left, top, window.ActualWidth, window.ActualHeight, achievement);
        }

        /// <summary>
        /// Notification Window that can be used to inform the user about unlocked achievements.
        /// Shows the AchievementNotificationWindow in the bottom right corner of the screen.
        /// </summary>
        /// <param name="achievement">The unlocked Achievement</param>
        public AchievementNotificationWindow(Achievement achievement)
        {
            Initialize(0, 0, SystemParameters.WorkArea.Right, SystemParameters.WorkArea.Bottom, achievement);
        }

        /// <summary>
        /// Notification Window that can be used to inform the user about unlocked achievements
        /// Shows the bottom rigth corner of the notification at the given coordinates
        /// </summary>
        /// <param name="achievement">The unlocked Achievement</param>
        /// <param name="left">The position of the left border of the Notification</param>
        /// <param name="top">The position of the top border of the Notification</param>
        public AchievementNotificationWindow(Achievement achievement, double left, double top)
        {
            Initialize(left, top, 0, 0, achievement);
        }

        private void Initialize(double left, double top, double width, double height, Achievement achievement)
        {
            Titel = achievement.Titel;
            ImagePath = achievement.ImagePath;

            if (String.IsNullOrEmpty(achievement.ImagePath))
                ImageVisibility = Visibility.Collapsed;
            else
                ImageVisibility = Visibility.Visible;

            InitializeComponent();

            Action method = () =>
            {
                Matrix transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                Point corner = transform.Transform(new Point(left + width, top + height));

                Left = corner.X - ActualWidth;
                Top = corner.Y - ActualHeight;
            };

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, method);
        }

        private void Timeline_OnCompleted(object sender, EventArgs e)
        {
            Close();
        }
    }
}