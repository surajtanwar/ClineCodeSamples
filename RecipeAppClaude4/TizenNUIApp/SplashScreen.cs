using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

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

            // Create the background rectangle with gradient effect
            // Since Tizen NUI doesn't support CSS gradients directly, we'll use the Rectangle.png image
            ImageView rectangleBackground = new ImageView()
            {
                ResourceUrl = "res/images/splash/Rectangle.png",
                Size2D = new Size2D(720, 1280), // Scale to fit 720x1280 from original 375x667
                Position2D = new Position2D(0, 0)
            };

            // Create the main chef hat icon (Group.png)
            // Original position: left: 91px, top: 111px (scaled proportionally)
            // Scale factor: 720/375 = 1.92 for width, 1280/667 = 1.92 for height
            ImageView chefHatIcon = new ImageView()
            {
                ResourceUrl = "res/images/splash/Group.png",
                Position2D = new Position2D((int)(91 * 1.92), (int)(111 * 1.92)), // Scaled position
                SizeModeFactor = new Vector3(1.92f, 1.92f, 1.0f) // Scale the image
            };

            // Create the text logo (Group_2.png)
            // Original position: left: 93px, top: 365px (scaled proportionally)
            ImageView textLogo = new ImageView()
            {
                ResourceUrl = "res/images/splash/Group_2.png",
                Position2D = new Position2D((int)(93 * 1.92), (int)(365 * 1.92)), // Scaled position
                SizeModeFactor = new Vector3(1.92f, 1.92f, 1.0f) // Scale the image
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
