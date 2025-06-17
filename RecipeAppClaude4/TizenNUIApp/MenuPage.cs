using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;

namespace TizenNUIApp
{
    public class MenuPage : View
    {
        private View headerView;
        private View menuItemsContainer;
        private View profileSection;
        private View redAreaContainer;

        // Colors matching the design
        private readonly Color menuBackgroundColor = new Color(0.95f, 0.4f, 0.4f, 1.0f); // Coral/salmon color
        private readonly Color whiteTextColor = Color.White;
        private readonly Color indicatorColor = Color.White;
        private readonly Color whiteBackgroundColor = Color.White;

        // Layout dimensions
        private readonly int totalWidth = 720; // Full window width
        private readonly int redAreaWidth = 576; // 80% of 720
        private readonly int whiteAreaWidth = 144; // 20% of 720

        // Event for navigation
        public event EventHandler<MenuItemSelectedEventArgs> MenuItemSelected;

        public MenuPage()
        {
            InitializeComponent();
            CreateLayout();
        }

        private void InitializeComponent()
        {
            Size2D = new Size2D(totalWidth, 1280);
            BackgroundColor = whiteBackgroundColor; // White background for the entire page
            Layout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Horizontal
            };
        }

        private void CreateLayout()
        {
            CreateWhiteArea();
            CreateRedArea();
        }

        private void CreateWhiteArea()
        {
            // Get the application resource directory path
            string resourcePath = Application.Current.DirectoryInfo.Resource;

            // White area container (20% width) - contains the menu icon
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

            // Hamburger menu button (close menu) - now in white area
            ImageView menuButton = new ImageView()
            {
                Size2D = new Size2D(32, 32),
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "menu", "btn-menu0.svg"),
                FittingMode = FittingModeType.ScaleToFill
            };

            // Add click event for menu button (close menu)
            menuButton.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    OnMenuItemSelected("back");
                    return true;
                }
                return false;
            };

            whiteArea.Add(menuButton);
            Add(whiteArea);
        }

        private void CreateRedArea()
        {
            // Red area container (80% width) - contains menu items and profile
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

            // Create menu items as shown in the image
            CreateMenuItem("POPULAR RECIPES", true); // First item has indicator
            CreateMenuItem("SAVED RECIPES", false);
            CreateMenuItem("SHOPPING LIST", false);
            CreateMenuItem("SETTINGS", false);

            redAreaContainer.Add(menuItemsContainer);
        }

        private void CreateMenuItem(string itemText, bool hasIndicator)
        {
            View menuItemView = new View()
            {
                Size2D = new Size2D(redAreaWidth - 80, 60), // Adjust width for red area minus padding
                BackgroundColor = menuBackgroundColor,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    LinearAlignment = LinearLayout.Alignment.Begin,
                    CellPadding = new Size2D(20, 0)
                },
                Focusable = true
            };

            // White vertical line indicator for active item
            if (hasIndicator)
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
                // Add spacing for non-active items to align text
                View spacer = new View()
                {
                    Size2D = new Size2D(24, 40),
                    BackgroundColor = menuBackgroundColor
                };
                menuItemView.Add(spacer);
            }

            // Menu item text
            TextLabel itemLabel = new TextLabel(itemText)
            {
                Size2D = new Size2D(redAreaWidth - 150, 60), // Adjust width for red area
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = whiteTextColor,
                PointSize = (20.0f / 1.33f) - 4, // Reduced by 2 more: 20px / 1.33 = 15pt, then -4 = 11pt
                FontFamily = "Samsung One 400",
                FontStyle = new PropertyMap().Add("weight", new PropertyValue("normal"))
            };

            // Add click event handler
            menuItemView.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    OnMenuItemSelected(itemText.ToLower().Replace(" ", "_"));
                    return true;
                }
                return false;
            };

            menuItemView.Add(itemLabel);
            menuItemsContainer.Add(menuItemView);
        }

        private void CreateProfileSection()
        {
            // Get the application resource directory path
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

            // Profile container with horizontal layout for image and name
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

            // Profile image (circular)
            ImageView profileImage = new ImageView()
            {
                Size2D = new Size2D(60, 60),
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "menu", "ellipse0.png"),
                FittingMode = FittingModeType.ScaleToFill,
                CornerRadius = 30.0f // Make it circular
            };

            // User name
            TextLabel nameLabel = new TextLabel("HARRY TRUMAN")
            {
                Size2D = new Size2D(redAreaWidth - 250, 60), // Adjust width for red area and image
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = whiteTextColor,
                PointSize = (18.0f / 1.33f) - 4, // Reduced by 2 more: 18px / 1.33 = 13.5pt, then -4 = 9.5pt
                FontFamily = "Samsung One 600",
                FontStyle = new PropertyMap().Add("weight", new PropertyValue("bold"))
            };

            // Add click event for profile section
            profileContainer.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    OnMenuItemSelected("profile");
                    return true;
                }
                return false;
            };

            profileContainer.Add(profileImage);
            profileContainer.Add(nameLabel);
            profileSection.Add(profileContainer);
            redAreaContainer.Add(profileSection);
        }

        private void OnMenuItemSelected(string menuItem)
        {
            MenuItemSelected?.Invoke(this, new MenuItemSelectedEventArgs(menuItem));
        }
    }

    // Event args class for menu item selection
    public class MenuItemSelectedEventArgs : EventArgs
    {
        public string MenuItem { get; }

        public MenuItemSelectedEventArgs(string menuItem)
        {
            MenuItem = menuItem;
        }
    }
}
