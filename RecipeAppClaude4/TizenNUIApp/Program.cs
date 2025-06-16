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
            // Create and show the Recipe Home Page
            var recipeHomePage = new RecipeHomePage();
            window.Add(recipeHomePage);
        }

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}
