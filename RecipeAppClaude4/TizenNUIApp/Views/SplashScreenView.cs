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
            // Based on the reference image, position it in the upper-center area
            ImageView chefHatIcon = new ImageView()
            {
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "splash", "Group.png"),
                Size2D = new Size2D(180, 180), // Adjusted size for better proportion
                Position2D = new Position2D(270, 350), // Centered horizontally (720/2 - 180/2 = 270), positioned higher
                PositionUsesPivotPoint = true,
                PivotPoint = new Position(0.5f, 0.5f, 0.5f), // Center pivot point
                ParentOrigin = new Position(0.5f, 0.5f, 0.5f) // Center parent origin
            };

            // Create the text logo (Group_2.png) - "Chef Recipes" text, properly centered
            // Position it closer below the chef hat icon, as shown in reference image
            ImageView textLogo = new ImageView()
            {
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "splash", "Group_2.png"),
                Size2D = new Size2D(280, 100), // Adjusted size for better proportion
                Position2D = new Position2D(220, 580), // Centered horizontally (720/2 - 280/2 = 220), positioned closer to chef hat
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
