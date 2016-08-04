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

// Inspired by https://stackoverflow.com/questions/3034741/create-popup-toaster-notifications-in-windows-with-net

using System;
using System.ComponentModel;
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
        #region Fields

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

        #endregion

        #region Events

        /// <summary>
        /// Event that fires when the animation is completed
        /// </summary>
        public event Action<AchievementNotificationWindow> Completed;
        private void InvokeCompleted()
        {
            if (Completed != null)
                Completed(this);
        }

        #endregion

        #region Constructors

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
            if (achievement == null)
                throw new ArgumentNullException("achievement");
            if (window == null)
                throw new ArgumentNullException("window");

            window.Closing += WindowClosing;

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

        #endregion

        #region Methods

        /// <summary>
        /// Unsubscribes the event and closes the AchievementNotificationWindow when the calling window is closed
        /// </summary>
        /// <param name="sender">the calling window</param>
        /// <param name="e">CancelEventArgs</param>
        private void WindowClosing(object sender, CancelEventArgs e)
        {
            ((Window)sender).Closing -= WindowClosing;
            Close();
        }

        /// <summary>
        /// Shows the AchievementNotificationWindow in the lower right corner of the given coordinates
        /// </summary>
        /// <param name="left">Distance from left screen border</param>
        /// <param name="top">Distance from top screen border</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="achievement">The Achievement</param>
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

        #endregion
    }
}