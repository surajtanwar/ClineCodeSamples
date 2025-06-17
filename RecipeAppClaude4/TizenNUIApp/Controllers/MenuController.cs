using System;
using System.Collections.Generic;
using TizenNUIApp.Models;

namespace TizenNUIApp.Controllers
{
    public class MenuController
    {
        private List<MenuItemModel> menuItems;
        private UserProfileModel userProfile;

        public MenuController()
        {
            InitializeMenuData();
        }

        public List<MenuItemModel> GetMenuItems()
        {
            return menuItems;
        }

        public UserProfileModel GetUserProfile()
        {
            return userProfile;
        }

        public void SetActiveMenuItem(string itemId)
        {
            foreach (var item in menuItems)
            {
                item.IsActive = item.Id == itemId;
            }
        }

        public MenuItemModel GetMenuItemById(string itemId)
        {
            return menuItems.Find(item => item.Id == itemId);
        }

        private void InitializeMenuData()
        {
            menuItems = new List<MenuItemModel>
            {
                new MenuItemModel("POPULAR RECIPES", "popular_recipes", true),
                new MenuItemModel("SAVED RECIPES", "saved_recipes", false),
                new MenuItemModel("SHOPPING LIST", "shopping_list", false),
                new MenuItemModel("SETTINGS", "settings", false)
            };

            userProfile = new UserProfileModel("HARRY TRUMAN", "ellipse0.png");
        }
    }
}
