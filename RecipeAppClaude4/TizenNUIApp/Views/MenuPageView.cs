using System;
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;
using TizenNUIApp.Models;
using TizenNUIApp.Controllers;

namespace TizenNUIApp.Views
{
    public class MenuPageView : View
    {
        private MenuController menuController;
        private View headerView;
        private View menuItemsContainer;
        private View profileSection;
        private View redAreaContainer;

        // Colors matching the design
        private readonly Color menuBackgroundColor = new Color(0.95f, 0.4f, 0.4f, 1.0f);
        private readonly Color whiteTextColor = Color.White;
        private readonly Color indicatorColor = Color.White;
        private readonly Color whiteBackgroundColor = Color.White;

        // Layout dimensions
        private readonly int totalWidth = 720;
        private readonly int redAreaWidth = 576;
        private readonly int whiteAreaWidth = 144;

        // Events
        public event EventHandler<string> MenuItemSelected;
        public event EventHandler BackButtonClicked;

        public MenuPageView(MenuController controller)
        {
            menuController = controller;
            InitializeComponent();
            CreateLayout();
        }

        private void InitializeComponent()
        {
            Size2D = new Size2D(totalWidth, 1280);
            BackgroundColor = whiteBackgroundColor;
            Layout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Horizontal
            };
        }

        private void CreateLayout()
        {
            CreateRedArea();
            CreateWhiteArea();
        }

        private void CreateWhiteArea()
        {
            string resourcePath = Application.Current.DirectoryInfo.Resource;

            View whiteArea = new View()
            {
                Size2D = new Size2D(whiteAreaWidth, 1280),
                BackgroundColor = whiteBackgroundColor,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    LinearAlignment = LinearLayout.Alignment.Begin
                },
                Padding = new Extents(20, 20, 40, 20)
            };

            ImageView menuButton = new ImageView()
            {
                Size2D = new Size2D(32, 32),
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "menu", "btn-menu0.svg"),
                FittingMode = FittingModeType.ScaleToFill
            };

            menuButton.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    BackButtonClicked?.Invoke(this, EventArgs.Empty);
                    return true;
                }
                return false;
            };

            whiteArea.Add(menuButton);
            Add(whiteArea);
        }

        private void CreateRedArea()
        {
            redAreaContainer = new View()
            {
                Size2D = new Size2D(redAreaWidth, 1280),
                BackgroundColor = menuBackgroundColor,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical
                }
            };

            CreateMenuItems();
            CreateProfileSection();

            Add(redAreaContainer);
        }

        private void CreateMenuItems()
        {
            menuItemsContainer = new View()
            {
                Size2D = new Size2D(redAreaWidth, 700),
                BackgroundColor = menuBackgroundColor,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    CellPadding = new Size2D(0, 40)
                },
                Padding = new Extents(40, 40, 100, 0)
            };

            var menuItems = menuController.GetMenuItems();
            foreach (var menuItem in menuItems)
            {
                CreateMenuItem(menuItem);
            }

            redAreaContainer.Add(menuItemsContainer);
        }

        private void CreateMenuItem(MenuItemModel menuItem)
        {
            View menuItemView = new View()
            {
                Size2D = new Size2D(redAreaWidth - 80, 60),
                BackgroundColor = menuBackgroundColor,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    LinearAlignment = LinearLayout.Alignment.Begin,
                    CellPadding = new Size2D(20, 0)
                },
                Focusable = true
            };

            if (menuItem.IsActive)
            {
                View indicator = new View()
                {
                    Size2D = new Size2D(4, 40),
                    BackgroundColor = indicatorColor
                };
                menuItemView.Add(indicator);
            }
            else
            {
                View spacer = new View()
                {
                    Size2D = new Size2D(24, 40),
                    BackgroundColor = menuBackgroundColor
                };
                menuItemView.Add(spacer);
            }

            TextLabel itemLabel = new TextLabel(menuItem.Text)
            {
                Size2D = new Size2D(redAreaWidth - 150, 60),
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = whiteTextColor,
                PointSize = (20.0f / 1.33f) - 4,
                FontFamily = "Samsung One 400",
                FontStyle = new PropertyMap().Add("weight", new PropertyValue("normal"))
            };

            menuItemView.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    MenuItemSelected?.Invoke(this, menuItem.Id);
                    return true;
                }
                return false;
            };

            menuItemView.Add(itemLabel);
            menuItemsContainer.Add(menuItemView);
        }

        private void CreateProfileSection()
        {
            string resourcePath = Application.Current.DirectoryInfo.Resource;

            profileSection = new View()
            {
                Size2D = new Size2D(redAreaWidth, 460),
                BackgroundColor = menuBackgroundColor,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    LinearAlignment = LinearLayout.Alignment.End,
                    CellPadding = new Size2D(0, 10)
                },
                Padding = new Extents(40, 40, 80, 80)
            };

            View profileContainer = new View()
            {
                Size2D = new Size2D(redAreaWidth - 160, 80),
                BackgroundColor = menuBackgroundColor,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    LinearAlignment = LinearLayout.Alignment.Begin,
                    CellPadding = new Size2D(15, 0)
                }
            };

            var userProfile = menuController.GetUserProfile();

            ImageView profileImage = new ImageView()
            {
                Size2D = new Size2D(60, 60),
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "menu", userProfile.ProfileImageName),
                FittingMode = FittingModeType.ScaleToFill,
                CornerRadius = 30.0f
            };

            TextLabel nameLabel = new TextLabel(userProfile.Name)
            {
                Size2D = new Size2D(redAreaWidth - 250, 60),
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = whiteTextColor,
                PointSize = (18.0f / 1.33f) - 4,
                FontFamily = "Samsung One 600",
                FontStyle = new PropertyMap().Add("weight", new PropertyValue("bold"))
            };

            profileContainer.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    MenuItemSelected?.Invoke(this, "profile");
                    return true;
                }
                return false;
            };

            profileContainer.Add(profileImage);
            profileContainer.Add(nameLabel);
            profileSection.Add(profileContainer);
            redAreaContainer.Add(profileSection);
        }

        public void UpdateActiveMenuItem(string itemId)
        {
            menuController.SetActiveMenuItem(itemId);
            RefreshMenuItems();
        }

        private void RefreshMenuItems()
        {
            // Clear existing menu items
            while (menuItemsContainer.ChildCount > 0)
            {
                View child = menuItemsContainer.GetChildAt(0);
                menuItemsContainer.Remove(child);
                child?.Dispose();
            }

            // Recreate menu items
            var menuItems = menuController.GetMenuItems();
            foreach (var menuItem in menuItems)
            {
                CreateMenuItem(menuItem);
            }
        }
    }
}
