using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using TizenNUIApp.Controllers;
using TizenNUIApp.Views;
using TizenNUIApp.Navigation;

namespace TizenNUIApp
{
    class Program : NUIApplication
    {
        private Window window;
        private NavigationManager navigationManager;
        
        // Controllers
        private RecipeController recipeController;
        private MenuController menuController;
        
        // Views
        private SplashScreenView splashScreenView;
        private RecipeHomePageView recipeHomePageView;
        private MenuPageView menuPageView;

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

            // Initialize controllers
            recipeController = new RecipeController();
            menuController = new MenuController();

            // Initialize navigation manager
            navigationManager = new NavigationManager(window);
            navigationManager.NavigationRequested += OnNavigationRequested;

            // Show splash screen first
            ShowSplashScreen();
        }

        void ShowSplashScreen()
        {
            splashScreenView = new SplashScreenView();
            splashScreenView.SplashCompleted += OnSplashCompleted;
            navigationManager.ShowPage(splashScreenView, PageType.Splash);
        }

        void OnSplashCompleted(object sender, EventArgs e)
        {
            // Navigate to home page
            navigationManager.NavigateTo(PageType.Home, addToStack: false);
        }

        void OnNavigationRequested(object sender, NavigationEventArgs e)
        {
            switch (e.PageType)
            {
                case PageType.Splash:
                    ShowSplashScreen();
                    break;
                case PageType.Home:
                    ShowHomePage();
                    break;
                case PageType.Menu:
                    ShowMenuPage();
                    break;
                case PageType.SavedRecipes:
                case PageType.ShoppingList:
                case PageType.Settings:
                case PageType.Profile:
                    // For now, navigate back to home page
                    // In a real app, you would implement these pages
                    navigationManager.NavigateTo(PageType.Home, addToStack: false);
                    break;
            }
        }

        void ShowHomePage()
        {
            if (recipeHomePageView == null)
            {
                recipeHomePageView = new RecipeHomePageView(recipeController);
                recipeHomePageView.MenuButtonClicked += OnMenuButtonClicked;
                recipeHomePageView.CategoryChanged += OnCategoryChanged;
            }
            
            navigationManager.ShowPage(recipeHomePageView, PageType.Home);
        }

        void ShowMenuPage()
        {
            if (menuPageView == null)
            {
                menuPageView = new MenuPageView(menuController);
                menuPageView.MenuItemSelected += OnMenuItemSelected;
                menuPageView.BackButtonClicked += OnMenuBackButtonClicked;
            }
            
            navigationManager.ShowPage(menuPageView, PageType.Menu);
        }

        void OnMenuButtonClicked(object sender, EventArgs e)
        {
            navigationManager.NavigateTo(PageType.Menu);
        }

        void OnCategoryChanged(object sender, string category)
        {
            if (recipeHomePageView != null)
            {
                recipeHomePageView.UpdateCategory(category);
            }
        }

        void OnMenuItemSelected(object sender, string menuItem)
        {
            switch (menuItem)
            {
                case "popular_recipes":
                    navigationManager.NavigateTo(PageType.Home, addToStack: false);
                    break;
                case "saved_recipes":
                    navigationManager.NavigateTo(PageType.SavedRecipes);
                    break;
                case "shopping_list":
                    navigationManager.NavigateTo(PageType.ShoppingList);
                    break;
                case "settings":
                    navigationManager.NavigateTo(PageType.Settings);
                    break;
                case "profile":
                    navigationManager.NavigateTo(PageType.Profile);
                    break;
            }
        }

        void OnMenuBackButtonClicked(object sender, EventArgs e)
        {
            navigationManager.NavigateBack();
        }

        protected override void OnTerminate()
        {
            navigationManager?.Dispose();
            base.OnTerminate();
        }

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}
