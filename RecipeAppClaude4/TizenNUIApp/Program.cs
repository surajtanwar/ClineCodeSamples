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
        private RecipeHomePage recipeHomePage;
        private MenuPage menuPage;
        private View currentPage;

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
                splashScreen = null;
            }

            // Show main application content
            ShowMainContent();
        }

        void ShowMainContent()
        {
            // Create and show the Recipe Home Page
            recipeHomePage = new RecipeHomePage();
            recipeHomePage.MenuButtonClicked += OnMenuButtonClicked;
            currentPage = recipeHomePage;
            window.Add(recipeHomePage);
        }

        void OnMenuButtonClicked(object sender, EventArgs e)
        {
            ShowMenuPage();
        }

        void ShowMenuPage()
        {
            // Remove current page
            if (currentPage != null)
            {
                window.Remove(currentPage);
            }

            // Create and show menu page
            if (menuPage == null)
            {
                menuPage = new MenuPage();
                menuPage.MenuItemSelected += OnMenuItemSelected;
            }
            
            currentPage = menuPage;
            window.Add(menuPage);
        }

        void OnMenuItemSelected(object sender, MenuItemSelectedEventArgs e)
        {
            switch (e.MenuItem)
            {
                case "back":
                    ShowRecipeHomePage();
                    break;
                case "my_recipes":
                case "favorites":
                case "recently_viewed":
                case "shopping_list":
                case "appetizers":
                case "main_courses":
                case "desserts":
                case "beverages":
                case "snacks":
                case "cooking_timer":
                case "unit_converter":
                case "meal_planner":
                case "nutrition_info":
                case "profile":
                case "settings":
                case "help_&_support":
                case "about":
                    // Handle other menu items - for now just show a placeholder
                    // In a real app, you would navigate to the appropriate page
                    break;
            }
        }

        void ShowRecipeHomePage()
        {
            // Remove current page
            if (currentPage != null)
            {
                window.Remove(currentPage);
            }

            // Show recipe home page
            currentPage = recipeHomePage;
            window.Add(recipeHomePage);
        }

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}
