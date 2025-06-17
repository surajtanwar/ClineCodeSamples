using System;
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace TizenNUIApp.Navigation
{
    public enum PageType
    {
        Splash,
        Home,
        Menu,
        SavedRecipes,
        ShoppingList,
        Settings,
        Profile
    }

    public class NavigationEventArgs : EventArgs
    {
        public PageType PageType { get; }
        public object Data { get; }

        public NavigationEventArgs(PageType pageType, object data = null)
        {
            PageType = pageType;
            Data = data;
        }
    }

    public class NavigationManager
    {
        private Window window;
        private Stack<PageType> navigationStack;
        private Dictionary<PageType, View> pageCache;
        private View currentPage;

        public event EventHandler<NavigationEventArgs> NavigationRequested;

        public NavigationManager(Window window)
        {
            this.window = window;
            navigationStack = new Stack<PageType>();
            pageCache = new Dictionary<PageType, View>();
        }

        public void NavigateTo(PageType pageType, object data = null, bool addToStack = true)
        {
            if (addToStack && currentPage != null)
            {
                var currentPageType = GetPageTypeFromView(currentPage);
                if (currentPageType.HasValue)
                {
                    navigationStack.Push(currentPageType.Value);
                }
            }

            NavigationRequested?.Invoke(this, new NavigationEventArgs(pageType, data));
        }

        public void NavigateBack()
        {
            if (navigationStack.Count > 0)
            {
                var previousPageType = navigationStack.Pop();
                NavigationRequested?.Invoke(this, new NavigationEventArgs(previousPageType));
            }
        }

        public void ShowPage(View page, PageType pageType)
        {
            HideCurrentPage();
            
            currentPage = page;
            window.Add(page);
            
            // Cache the page for potential reuse
            if (!pageCache.ContainsKey(pageType))
            {
                pageCache[pageType] = page;
            }
        }

        public void HideCurrentPage()
        {
            if (currentPage != null)
            {
                window.Remove(currentPage);
            }
        }

        public void ClearNavigationStack()
        {
            navigationStack.Clear();
        }

        public bool CanNavigateBack()
        {
            return navigationStack.Count > 0;
        }

        public View GetCachedPage(PageType pageType)
        {
            return pageCache.ContainsKey(pageType) ? pageCache[pageType] : null;
        }

        public void ClearPageCache()
        {
            foreach (var page in pageCache.Values)
            {
                page?.Dispose();
            }
            pageCache.Clear();
        }

        private PageType? GetPageTypeFromView(View view)
        {
            // This is a simple mapping - in a more complex app, you might want to use a more sophisticated approach
            var typeName = view.GetType().Name;
            switch (typeName)
            {
                case "SplashScreen":
                    return PageType.Splash;
                case "RecipeHomePage":
                    return PageType.Home;
                case "MenuPage":
                    return PageType.Menu;
                default:
                    return null;
            }
        }

        public void Dispose()
        {
            ClearPageCache();
            navigationStack.Clear();
            currentPage = null;
        }
    }
}
