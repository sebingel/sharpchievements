// Inspired by https://stackoverflow.com/questions/3034741/create-popup-toaster-notifications-in-windows-with-net

using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Point = System.Windows.Point;

namespace sebingel.sharpchievements
{
    /// <summary>
    /// Notification Window that can be used to inform the user aabout unlocked achievements
    /// </summary>
    public partial class AchievementNotificationWindow
    {
        /// <summary>
        /// The Titel of the Achievement
        /// </summary>
        public string Titel { get; set; }

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
            Titel = achievement.Titel;

            InitializeComponent();

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                Matrix transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                Point corner = transform.Transform(new Point(left+width, top+height));

                Left = corner.X - ActualWidth;
                Top = corner.Y - ActualHeight;
            }));
        }

        /// <summary>
        /// Notification Window that can be used to inform the user about unlocked achievements.
        /// Shows the AchievementNotificationWindow in the bottom right corner of the given Window.
        /// </summary>
        /// <param name="achievement">The unlocked Achievement</param>
        /// <param name="window">The Window in which the Notification should be displayed</param>
        public AchievementNotificationWindow(Achievement achievement, Window window)
        {
            Titel = achievement.Titel;

            InitializeComponent();

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                double left = window.Left;
                double width = window.ActualWidth;
                double top = window.Top;
                double height = window.ActualHeight;

                if(window.WindowState == WindowState.Maximized)
                {
                    left = 0;
                    top = 0;
                }

                Matrix transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                Point corner = transform.Transform(new Point(left + width, top + height));

                Left = corner.X - ActualWidth;
                Top = corner.Y - ActualHeight;
            }));
        }

        /// <summary>
        /// Notification Window that can be used to inform the user about unlocked achievements.
        /// Shows the AchievementNotificationWindow in the bottom right corner of the screen.
        /// </summary>
        /// <param name="achievement">The unlocked Achievement</param>
        public AchievementNotificationWindow(Achievement achievement)
        {
            Titel = achievement.Titel;

            InitializeComponent();

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                Matrix transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                Point corner = transform.Transform(new Point(SystemParameters.WorkArea.Right, SystemParameters.WorkArea.Bottom));

                Left = corner.X - ActualWidth;
                Top = corner.Y - ActualHeight;
            }));
        }

        /// <summary>
        /// Notification Window that can be used to inform the user about unlocked achievements
        /// </summary>
        /// <param name="achievement">The unlocked Achievement</param>
        /// <param name="left">The position of the left border of the Notification</param>
        /// <param name="top">The position of the top border of the Notification</param>
        public AchievementNotificationWindow(Achievement achievement, double left, double top)
        {
            Titel = achievement.Titel;

            InitializeComponent();

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                Left = left;
                Top = top;
            }));
        }

        private void Timeline_OnCompleted(object sender, EventArgs e)
        {
            Close();
        }
    }
}