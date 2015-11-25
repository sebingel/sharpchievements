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
        /// Notification Window that can be used to inform the user about unlocked achievements
        /// </summary>
        /// <param name="left">The position of the left border of the application's GUI</param>
        /// <param name="top">The position of the top border of the application's GUI</param>
        /// <param name="width">The width of the application's GUI</param>
        /// <param name="height">The height of the application's GUI</param>
        /// <param name="achievement">The unlocked Achievement</param>
        public AchievementNotificationWindow(double left, double top, double width, double height, Achievement achievement)
        {
            Titel = achievement.Titel;

            InitializeComponent();

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                //Rectangle workingArea = System.Windows.SystemParameters.WorkArea;
                Rectangle workingArea = new Rectangle(Convert.ToInt32(left), Convert.ToInt32(top), Convert.ToInt32(width), Convert.ToInt32(height));
                Matrix transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                Point corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

                Left = corner.X - ActualWidth;
                Top = corner.Y - ActualHeight;
            }));
        }

        private void Timeline_OnCompleted(object sender, EventArgs e)
        {
            Close();
        }
    }
}