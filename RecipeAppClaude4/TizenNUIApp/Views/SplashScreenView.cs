using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;
using Tizen.NUI.Components;

namespace TizenNUIApp.Views
{
    public class SplashScreenView : View
    {
        private Timer splashTimer;
        public event EventHandler SplashCompleted;

        public SplashScreenView()
        {
            InitializeSplashScreen();
            StartSplashTimer();
        }

        private void InitializeSplashScreen()
        {
            // Set up the main splash container to match 720x1280 resolution
            this.Size2D = new Size2D(720, 1280);
            this.BackgroundColor = Color.White;
            this.Position2D = new Position2D(0, 0);

            // Get the application resource directory path
            string resourcePath = Application.Current.DirectoryInfo.Resource;

            // Calculate scaling factors from CSS design (375x667) to target resolution (720x1280)
            float scaleX = 720f / 375f; // 1.92
            float scaleY = 1280f / 667f; // 1.92

            // Create the background rectangle with coral gradient
            // Based on CSS: width: 375px, height: 667px, positioned at (0,0)
            ImageView rectangleBackground = new ImageView()
            {
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "splash", "Rectangle.png"),
                Size2D = new Size2D((int)(375 * scaleX), (int)(667 * scaleY)), // Scale to fit 720x1280
                Position2D = new Position2D(0, 0),
                WidthResizePolicy = ResizePolicyType.Fixed,
                HeightResizePolicy = ResizePolicyType.Fixed
            };

            // Create the main group image (Group.png)
            // Based on CSS: positioned at left: 91px, top: 111px
            ImageView groupImage = new ImageView()
            {
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "splash", "Group.png"),
                Position2D = new Position2D((int)(91 * scaleX), (int)(111 * scaleY)), // Scale positions
                WidthResizePolicy = ResizePolicyType.UseNaturalSize,
                HeightResizePolicy = ResizePolicyType.UseNaturalSize
            };

            // Create the second group image (Group_2.png)
            // Based on CSS: positioned at left: 93px, top: 365px
            ImageView group2Image = new ImageView()
            {
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "splash", "Group_2.png"),
                Position2D = new Position2D((int)(93 * scaleX), (int)(365 * scaleY)), // Scale positions
                WidthResizePolicy = ResizePolicyType.UseNaturalSize,
                HeightResizePolicy = ResizePolicyType.UseNaturalSize
            };

            // Add all elements to the splash screen in the correct order
            this.Add(rectangleBackground);
            this.Add(groupImage);
            this.Add(group2Image);
        }

        private void StartSplashTimer()
        {
            // Create a timer for 2 seconds (2000 milliseconds)
            splashTimer = new Timer(2000);
            splashTimer.Tick += OnSplashTimerTick;
            splashTimer.Start();
        }

        private bool OnSplashTimerTick(object sender, Timer.TickEventArgs e)
        {
            // Stop the timer
            splashTimer?.Stop();
            splashTimer?.Dispose();
            splashTimer = null;

            // Notify that splash is completed
            SplashCompleted?.Invoke(this, EventArgs.Empty);

            return false; // Don't repeat the timer
        }

        protected override void Dispose(DisposeTypes type)
        {
            if (splashTimer != null)
            {
                splashTimer.Stop();
                splashTimer.Dispose();
                splashTimer = null;
            }
            base.Dispose(type);
        }
    }
}
