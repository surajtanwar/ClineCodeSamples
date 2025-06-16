using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;

namespace TizenNUIApp
{
    public class MenuPage : View
    {
        private View headerView;
        private ScrollView menuScrollView;
        private View menuItemsContainer;

        // Colors consistent with the app design
        private readonly Color backgroundColor = new Color(0.98f, 0.98f, 0.98f, 1.0f);
        private readonly Color headerColor = Color.White;
        private readonly Color primaryTextColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);
        private readonly Color secondaryTextColor = new Color(0.6f, 0.6f, 0.6f, 1.0f);
        private readonly Color accentColor = new Color(0.95f, 0.4f, 0.4f, 1.0f); // Coral/salmon color
        private readonly Color menuItemBackgroundColor = Color.White;
        private readonly Color separatorColor = new Color(0.9f, 0.9f, 0.9f, 1.0f);

        // Event for navigation
        public event EventHandler<MenuItemSelectedEventArgs> MenuItemSelected;

        public MenuPage()
        {
            InitializeComponent();
            CreateLayout();
        }

        private void InitializeComponent()
        {
            Size2D = new Size2D(720, 1280);
            BackgroundColor = backgroundColor;
            Layout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Vertical
            };
        }

        private void CreateLayout()
        {
            CreateHeader();
            CreateMenuScrollView();
        }

        private void CreateHeader()
        {
            // Get the application resource directory path
            string resourcePath = Application.Current.DirectoryInfo.Resource;

            headerView = new View()
            {
                Size2D = new Size2D(720, 120),
                BackgroundColor = headerColor,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    LinearAlignment = LinearLayout.Alignment.Center,
                    CellPadding = new Size2D(20, 0)
                },
                Padding = new Extents(20, 20, 40, 20)
            };

            // Back button (using menu icon as placeholder)
            ImageView backButton = new ImageView()
            {
                Size2D = new Size2D(32, 32),
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "menu", "btn-menu0.svg"),
                FittingMode = FittingModeType.ScaleToFill
            };

            // Add click event for back button
            backButton.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    OnMenuItemSelected("back");
                    return true;
                }
                return false;
            };

            // Title
            TextLabel titleLabel = new TextLabel("MENU")
            {
                Size2D = new Size2D(520, 60),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = accentColor,
                PointSize = (24.0f / 1.33f) - 6, // Converting from px to pt: 24px / 1.33 = 18pt, then -6 = 12pt
                FontFamily = "Samsung One 600",
                FontStyle = new PropertyMap().Add("weight", new PropertyValue("bold"))
            };

            // Profile/Settings icon (using ellipse as placeholder)
            ImageView profileButton = new ImageView()
            {
                Size2D = new Size2D(32, 32),
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "menu", "ellipse0.png"),
                FittingMode = FittingModeType.ScaleToFill
            };

            // Add click event for profile button
            profileButton.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    OnMenuItemSelected("profile");
                    return true;
                }
                return false;
            };

            headerView.Add(backButton);
            headerView.Add(titleLabel);
            headerView.Add(profileButton);

            Add(headerView);
        }

        private void CreateMenuScrollView()
        {
            // Main container for the menu section
            View menuSection = new View()
            {
                Size2D = new Size2D(720, 1160),
                BackgroundColor = backgroundColor,
                Padding = new Extents(0, 0, 20, 20)
            };

            // Vertical scroll view for menu items
            menuScrollView = new ScrollView()
            {
                Size2D = new Size2D(720, 1160),
                BackgroundColor = backgroundColor,
                Position2D = new Position2D(0, 0)
            };

            // Configure vertical scrolling
            menuScrollView.SetAxisAutoLock(true);
            menuScrollView.SetAxisAutoLockGradient(1.0f);

            menuItemsContainer = new View()
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    CellPadding = new Size2D(0, 0)
                },
                Padding = new Extents(0, 0, 0, 0),
                Size2D = new Size2D(720, 1400) // Height larger than container to enable scrolling
            };

            // Create menu sections
            CreateMenuSection("RECIPES", new string[] { "My Recipes", "Favorites", "Recently Viewed", "Shopping List" });
            CreateMenuSection("CATEGORIES", new string[] { "Appetizers", "Main Courses", "Desserts", "Beverages", "Snacks" });
            CreateMenuSection("COOKING", new string[] { "Cooking Timer", "Unit Converter", "Meal Planner", "Nutrition Info" });
            CreateMenuSection("ACCOUNT", new string[] { "Profile", "Settings", "Help & Support", "About" });

            menuScrollView.Add(menuItemsContainer);
            menuSection.Add(menuScrollView);
            Add(menuSection);
        }

        private void CreateMenuSection(string sectionTitle, string[] menuItems)
        {
            // Section header
            View sectionHeaderView = new View()
            {
                Size2D = new Size2D(720, 60),
                BackgroundColor = backgroundColor,
                Padding = new Extents(30, 30, 15, 15)
            };

            TextLabel sectionLabel = new TextLabel(sectionTitle)
            {
                Size2D = new Size2D(660, 30),
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = secondaryTextColor,
                PointSize = (14.0f / 1.33f) - 4, // Converting from px to pt: 14px / 1.33 = 10.5pt, then -4 = 6.5pt
                FontFamily = "Samsung One 600",
                FontStyle = new PropertyMap().Add("weight", new PropertyValue("bold"))
            };

            sectionHeaderView.Add(sectionLabel);
            menuItemsContainer.Add(sectionHeaderView);

            // Menu items in this section
            foreach (string itemText in menuItems)
            {
                CreateMenuItem(itemText);
            }

            // Add separator after section
            CreateSeparator();
        }

        private void CreateMenuItem(string itemText)
        {
            View menuItemView = new View()
            {
                Size2D = new Size2D(720, 70),
                BackgroundColor = menuItemBackgroundColor,
                Margin = new Extents(20, 20, 0, 0),
                Padding = new Extents(30, 30, 0, 0),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    LinearAlignment = LinearLayout.Alignment.Center,
                    CellPadding = new Size2D(20, 0)
                },
                Focusable = true
            };

            // Menu item text
            TextLabel itemLabel = new TextLabel(itemText)
            {
                Size2D = new Size2D(600, 70),
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = primaryTextColor,
                PointSize = (18.0f / 1.33f) - 4, // Converting from px to pt: 18px / 1.33 = 13.5pt, then -4 = 9.5pt
                FontFamily = "Samsung One 400"
            };

            // Arrow indicator (using a simple text arrow)
            TextLabel arrowLabel = new TextLabel(">")
            {
                Size2D = new Size2D(40, 70),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = secondaryTextColor,
                PointSize = (16.0f / 1.33f) - 4, // Converting from px to pt: 16px / 1.33 = 12pt, then -4 = 8pt
                FontFamily = "Samsung One 400"
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

            // Add hover effect
            menuItemView.FocusGained += (sender, e) =>
            {
                menuItemView.BackgroundColor = new Color(0.95f, 0.95f, 0.95f, 1.0f);
            };

            menuItemView.FocusLost += (sender, e) =>
            {
                menuItemView.BackgroundColor = menuItemBackgroundColor;
            };

            menuItemView.Add(itemLabel);
            menuItemView.Add(arrowLabel);
            menuItemsContainer.Add(menuItemView);
        }

        private void CreateSeparator()
        {
            View separator = new View()
            {
                Size2D = new Size2D(720, 20),
                BackgroundColor = backgroundColor
            };

            menuItemsContainer.Add(separator);
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
