using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace TizenNUIApp
{
    class Program : NUIApplication
    {
        private Window window;
        private View mainView;
        private SplashScreen splashScreen;

        protected override void OnCreate()
        {
            base.OnCreate();
            Initialize();
        }

        void Initialize()
        {
            // Get the default window
            window = GetDefaultWindow();
            window.WindowSize = new Size2D(720, 1280);
            window.BackgroundColor = Color.White;

            // Show splash screen first
            ShowSplashScreen();
        }

        void ShowSplashScreen()
        {
            // Create and show splash screen
            splashScreen = new SplashScreen();
            splashScreen.SplashCompleted += OnSplashCompleted;
            window.Add(splashScreen);
        }

        void OnSplashCompleted(object sender, EventArgs e)
        {
            // Remove splash screen
            if (splashScreen != null)
            {
                window.Remove(splashScreen);
                splashScreen.Dispose();
                splashScreen = null;
            }

            // Show main application content
            ShowMainContent();
        }

        void ShowMainContent()
        {
            // Create main view
            mainView = new View()
            {
                Size2D = new Size2D(720, 1280),
                BackgroundColor = new Color(0.9f, 0.9f, 0.9f, 1.0f),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    LinearAlignment = LinearLayout.Alignment.Center,
                    CellPadding = new Size2D(10, 10)
                }
            };

            // Create title text
            TextLabel titleLabel = new TextLabel("Tizen NUI Application")
            {
                Size2D = new Size2D(700, 80),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = Color.Black,
                PointSize = 24.0f,
                FontFamily = "Samsung One 400"
            };

            // Create description text
            TextLabel descriptionLabel = new TextLabel("Mobile App - 720x1280 Resolution")
            {
                Size2D = new Size2D(700, 60),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = new Color(0.5f, 0.5f, 0.5f, 1.0f),
                PointSize = 16.0f,
                FontFamily = "Samsung One 400"
            };

            // Create a button
            // Button actionButton = new Button()
            // {
            //     Size2D = new Size2D(200, 60),
            //     Text = "Click Me!",
            //     BackgroundColor = new Color(0.2f, 0.6f, 1.0f, 1.0f),
            //     TextColor = Color.White,
            //     CornerRadius = 10.0f
            // };

            // actionButton.Clicked += (sender, e) =>
            // {
            //     titleLabel.Text = "Button Clicked!";
            //     actionButton.BackgroundColor = new Color(0.2f, 0.8f, 0.2f, 1.0f);
            // };

            // Create a colored rectangle view
            View coloredView = new View()
            {
                Size2D = new Size2D(300, 200),
                BackgroundColor = new Color(1.0f, 0.5f, 0.2f, 1.0f),
                CornerRadius = 15.0f,
                Margin = new Extents(0, 0, 20, 20)
            };

            // Add views to main view
            mainView.Add(titleLabel);
            mainView.Add(descriptionLabel);
            //mainView.Add(actionButton);
            mainView.Add(coloredView);

            // Add main view to window
            window.Add(mainView);
        }

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}
