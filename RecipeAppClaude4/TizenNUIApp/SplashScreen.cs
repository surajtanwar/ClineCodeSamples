using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;
using Tizen.NUI.Components;

namespace TizenNUIApp
{
    public class SplashScreen : View
    {
        private Timer splashTimer;
        public event EventHandler SplashCompleted;

        public SplashScreen()
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

            // Create the coral/pink gradient background
            // Use the Rectangle.png image which should contain the coral gradient
            ImageView rectangleBackground = new ImageView()
            {
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "splash", "Rectangle.png"),
                Size2D = new Size2D(720, 1280),
                Position2D = new Position2D(0, 0),
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent
            };

            // Create the main chef hat icon (Group.png) - properly centered
            // Using center positioning: screen width/2 - image width/2 for horizontal center
            // Positioned in upper-middle area for vertical placement
            ImageView chefHatIcon = new ImageView()
            {
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "splash", "Group.png"),
                Size2D = new Size2D(200, 200), // Set appropriate size for the chef hat
                Position2D = new Position2D(260, 440), // Centered horizontally (720/2 - 200/2 = 260), positioned vertically
                PositionUsesPivotPoint = true,
                PivotPoint = new Position(0.5f, 0.5f, 0.5f), // Center pivot point
                ParentOrigin = new Position(0.5f, 0.5f, 0.5f) // Center parent origin
            };

            // Create the text logo (Group_2.png) - "Chef Recipes" text, properly centered
            // Position it below the chef hat icon, centered horizontally
            ImageView textLogo = new ImageView()
            {
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "splash", "Group_2.png"),
                Size2D = new Size2D(300, 120), // Set appropriate size for the text
                Position2D = new Position2D(210, 680), // Centered horizontally (720/2 - 300/2 = 210), positioned below chef hat
                PositionUsesPivotPoint = true,
                PivotPoint = new Position(0.5f, 0.5f, 0.5f), // Center pivot point
                ParentOrigin = new Position(0.5f, 0.5f, 0.5f) // Center parent origin
            };

            // Add all elements to the splash screen
            this.Add(rectangleBackground);
            this.Add(chefHatIcon);
            this.Add(textLogo);
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
