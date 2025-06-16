using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;

namespace TizenNUIApp
{
    public class RecipeHomePage : View
    {
        private View headerView;
        private View categoryTabsView;
        private ScrollView recipeScrollView;
        private View recipeCardsContainer;
        private View recipeSection;

        // Colors based on typical recipe app design
        private readonly Color backgroundColor = new Color(0.98f, 0.98f, 0.98f, 1.0f);
        private readonly Color headerColor = Color.White;
        private readonly Color primaryTextColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);
        private readonly Color secondaryTextColor = new Color(0.6f, 0.6f, 0.6f, 1.0f);
        private readonly Color accentColor = new Color(0.95f, 0.4f, 0.4f, 1.0f); // Coral/salmon color
        private readonly Color cardBackgroundColor = Color.White;

        // Current active category
        private string currentCategory = "APPETIZERS";

        // Tab references for dynamic updates
        private View appetizersTab;
        private View entreesTab;
        private View dessertTab;

        public RecipeHomePage()
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
            CreateCategoryTabs();
            CreateRecipeScrollView();
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

            // Menu button
            ImageView menuButton = new ImageView()
            {
                Size2D = new Size2D(32, 32),
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "home", "btn-menu0.svg"),
                FittingMode = FittingModeType.ScaleToFill
            };

            // Title
            TextLabel titleLabel = new TextLabel("POPULAR RECIPES")
            {
                Size2D = new Size2D(520, 60),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = accentColor,
                PointSize = (24.0f / 1.33f) - 4, // Converting from px to pt: 24px / 1.33 = 18pt, then -4 = 14pt
                FontFamily = "Samsung One 600",
                FontStyle = new PropertyMap().Add("weight", new PropertyValue("bold"))
            };

            // Search button
            ImageView searchButton = new ImageView()
            {
                Size2D = new Size2D(32, 32),
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "home", "btn-search0.svg"),
                FittingMode = FittingModeType.ScaleToFill
            };

            headerView.Add(menuButton);
            headerView.Add(titleLabel);
            headerView.Add(searchButton);

            Add(headerView);
        }

        private void CreateCategoryTabs()
        {
            categoryTabsView = new View()
            {
                Size2D = new Size2D(720, 80),
                BackgroundColor = headerColor,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    LinearAlignment = LinearLayout.Alignment.Center,
                    CellPadding = new Size2D(0, 0)
                },
                Padding = new Extents(0, 0, 10, 10)
            };

            // Create category tabs and store references
            appetizersTab = CreateCategoryTab("APPETIZERS", true);
            entreesTab = CreateCategoryTab("ENTREES", false);
            dessertTab = CreateCategoryTab("DESSERT", false);

            Add(categoryTabsView);
        }

        private View CreateCategoryTab(string text, bool isActive)
        {
            View tabContainer = new View()
            {
                Size2D = new Size2D(240, 60),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    LinearAlignment = LinearLayout.Alignment.Center
                },
                Focusable = true
            };

            TextLabel tabLabel = new TextLabel(text)
            {
                Size2D = new Size2D(240, 40),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = isActive ? primaryTextColor : secondaryTextColor,
                PointSize = (16.0f / 1.33f) - 4, // Converting from px to pt: 16px / 1.33 = 12pt, then -4 = 8pt
                FontFamily = "Samsung One 400",
                FontStyle = isActive ? new PropertyMap().Add("weight", new PropertyValue("bold")) : new PropertyMap()
            };

            // Active tab underline
            View underline = null;
            if (isActive)
            {
                underline = new View()
                {
                    Size2D = new Size2D(60, 3),
                    BackgroundColor = accentColor,
                    Position2D = new Position2D(90, 50)
                };
                tabContainer.Add(underline);
            }

            // Add click event handler
            tabContainer.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    SwitchToCategory(text);
                    return true;
                }
                return false;
            };

            tabContainer.Add(tabLabel);
            categoryTabsView.Add(tabContainer);
            
            return tabContainer;
        }

        private void CreateRecipeScrollView()
        {
            // Main container for the recipe section
            View recipeSection = new View()
            {
                Size2D = new Size2D(720, 1080),
                BackgroundColor = backgroundColor,
                Padding = new Extents(20, 20, 20, 20)
            };

            // Horizontal scroll view for carousel
            recipeScrollView = new ScrollView()
            {
                Size2D = new Size2D(720, 600), // Height for one card plus some padding
                BackgroundColor = backgroundColor,
                Position2D = new Position2D(0, 0)
            };

            recipeCardsContainer = new View()
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    CellPadding = new Size2D(20, 0)
                },
                Padding = new Extents(20, 20, 20, 20)
            };

            // Create recipe cards for carousel
            CreateRecipeCard("Prime Rib Roast", "5HR", "685", "107", "maskgroup0.png");
            CreateRecipeCard("Grilled Salmon", "30MIN", "425", "89", "maskgroup1.png");
            CreateRecipeCard("Chocolate Cake", "2HR", "892", "156", "rectangle0.png");

            recipeScrollView.Add(recipeCardsContainer);
            recipeSection.Add(recipeScrollView);
            Add(recipeSection);
        }

        private void CreateRecipeCard(string title, string time, string likes, string comments, string imageName)
        {
            // Get the application resource directory path
            string resourcePath = Application.Current.DirectoryInfo.Resource;

            View cardView = new View()
            {
                Size2D = new Size2D(300, 450), // Smaller width for carousel, taller for content
                BackgroundColor = cardBackgroundColor,
                CornerRadius = 16.0f,
                Margin = new Extents(0, 0, 10, 10)
            };

            // Recipe image
            ImageView recipeImage = new ImageView()
            {
                Size2D = new Size2D(300, 180),
                Position2D = new Position2D(0, 0),
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "home", imageName),
                FittingMode = FittingModeType.ScaleToFill
            };

            // Heart button overlay
            ImageView heartButton = new ImageView()
            {
                Size2D = new Size2D(32, 32),
                Position2D = new Position2D(250, 15),
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "home", "button-heart0.svg"),
                FittingMode = FittingModeType.ScaleToFill
            };

            // Content area
            View contentView = new View()
            {
                Size2D = new Size2D(300, 270), // Adjusted for smaller card
                Position2D = new Position2D(0, 180),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    CellPadding = new Size2D(0, 6)
                },
                Padding = new Extents(15, 15, 15, 15)
            };

            // Star rating
            View starRatingView = CreateStarRating();

            // Recipe title
            TextLabel titleLabel = new TextLabel(title)
            {
                Size2D = new Size2D(270, 30),
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = primaryTextColor,
                PointSize = (20.0f / 1.33f) - 4, // Converting from px to pt: 20px / 1.33 = 15pt, then -4 = 11pt
                FontFamily = "Samsung One 600",
                FontStyle = new PropertyMap().Add("weight", new PropertyValue("bold"))
            };

            // Recipe description
            string description = GetRecipeDescription(title);
            TextLabel descriptionLabel = new TextLabel(description)
            {
                Size2D = new Size2D(270, 120),
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Top,
                TextColor = secondaryTextColor,
                PointSize = (14.0f / 1.33f) - 4, // Converting from px to pt: 14px / 1.33 = 10.5pt, then -4 = 6.5pt
                FontFamily = "Samsung One 400",
                MultiLine = true
            };

            // Stats row
            View statsView = new View()
            {
                Size2D = new Size2D(270, 30),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    LinearAlignment = LinearLayout.Alignment.Begin,
                    CellPadding = new Size2D(15, 0)
                }
            };

            // Time
            View timeView = CreateStatItem("icons0.svg", time);
            // Likes
            View likesView = CreateStatItem("icons1.svg", likes);
            // Comments
            View commentsView = CreateStatItem("icons2.svg", comments);

            statsView.Add(timeView);
            statsView.Add(likesView);
            statsView.Add(commentsView);

            contentView.Add(starRatingView);
            contentView.Add(titleLabel);
            contentView.Add(descriptionLabel);
            contentView.Add(statsView);

            cardView.Add(recipeImage);
            cardView.Add(heartButton);
            cardView.Add(contentView);

            recipeCardsContainer.Add(cardView);
        }

        private string GetRecipeDescription(string title)
        {
            switch (title)
            {
                // Appetizers
                case "Stuffed Mushrooms":
                    return "Delicious button mushrooms stuffed with a savory mixture of breadcrumbs, herbs, and cheese. Perfect for parties and gatherings.";
                case "Bruschetta":
                    return "Classic Italian appetizer with toasted bread topped with fresh tomatoes, basil, and garlic. Simple yet elegant.";
                case "Shrimp Cocktail":
                    return "Chilled jumbo shrimp served with tangy cocktail sauce. A timeless appetizer that never goes out of style.";
                
                // Entrees
                case "Prime Rib Roast":
                    return "The Prime Rib Roast is a classic and tender cut of beef taken from the rib primal cut. Learn how to make the perfect prime rib roast to serve your family and friends. Check out What's Cooking America's award-winning Classic Prime Rib Roast recipe and photo tutorial to help you make the Perfect Prime Rib Roast.";
                case "Grilled Salmon":
                    return "Fresh Atlantic salmon grilled to perfection with herbs and lemon. A healthy and delicious meal that's perfect for any occasion.";
                case "Chicken Parmesan":
                    return "Crispy breaded chicken breast topped with marinara sauce and melted mozzarella cheese. A family favorite that's sure to please.";
                
                // Desserts
                case "Chocolate Cake":
                    return "Rich, moist chocolate cake with layers of decadent chocolate frosting. A dessert that will satisfy any chocolate lover's cravings.";
                case "Tiramisu":
                    return "Classic Italian dessert with layers of coffee-soaked ladyfingers and creamy mascarpone cheese. An elegant end to any meal.";
                case "Apple Pie":
                    return "Traditional American apple pie with flaky crust and cinnamon-spiced apple filling. Serve warm with vanilla ice cream.";
                
                default:
                    return "A delicious recipe that will delight your taste buds and impress your guests.";
            }
        }

        private View CreateStarRating()
        {
            // Get the application resource directory path
            string resourcePath = Application.Current.DirectoryInfo.Resource;

            View starContainer = new View()
            {
                Size2D = new Size2D(100, 20),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    CellPadding = new Size2D(2, 0)
                }
            };

            // Add 4 filled stars and 1 empty star
            for (int i = 0; i < 4; i++)
            {
                ImageView star = new ImageView()
                {
                    Size2D = new Size2D(16, 16),
                    ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "home", "star4.svg"), // Filled star
                    FittingMode = FittingModeType.ScaleToFill
                };
                starContainer.Add(star);
            }

            // Empty star
            ImageView emptyStar = new ImageView()
            {
                Size2D = new Size2D(16, 16),
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "home", "star0.svg"), // Empty star
                FittingMode = FittingModeType.ScaleToFill
            };
            starContainer.Add(emptyStar);

            return starContainer;
        }

        private View CreateStatItem(string iconName, string value)
        {
            // Get the application resource directory path
            string resourcePath = Application.Current.DirectoryInfo.Resource;

            View statView = new View()
            {
                Size2D = new Size2D(80, 30),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    LinearAlignment = LinearLayout.Alignment.Begin,
                    CellPadding = new Size2D(5, 0)
                }
            };

            ImageView icon = new ImageView()
            {
                Size2D = new Size2D(16, 16),
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "home", iconName),
                FittingMode = FittingModeType.ScaleToFill
            };

            TextLabel valueLabel = new TextLabel(value)
            {
                Size2D = new Size2D(60, 30),
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = secondaryTextColor,
                PointSize = (12.0f / 1.33f) - 4, // Converting from px to pt: 12px / 1.33 = 9pt, then -4 = 5pt
                FontFamily = "Samsung One 400"
            };

            statView.Add(icon);
            statView.Add(valueLabel);

            return statView;
        }

        private void SwitchToCategory(string category)
        {
            if (currentCategory == category) return; // Already on this category

            currentCategory = category;
            UpdateTabAppearance();
            RefreshRecipeCards();
        }

        private void UpdateTabAppearance()
        {
            // Update all tabs to inactive state first
            UpdateTabState(appetizersTab, "APPETIZERS", currentCategory == "APPETIZERS");
            UpdateTabState(entreesTab, "ENTREES", currentCategory == "ENTREES");
            UpdateTabState(dessertTab, "DESSERT", currentCategory == "DESSERT");
        }

        private void UpdateTabState(View tabContainer, string tabText, bool isActive)
        {
            // Clear existing children
            while (tabContainer.ChildCount > 0)
            {
                View child = tabContainer.GetChildAt(0);
                tabContainer.Remove(child);
                child?.Dispose();
            }

            // Recreate tab label
            TextLabel tabLabel = new TextLabel(tabText)
            {
                Size2D = new Size2D(240, 40),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = isActive ? primaryTextColor : secondaryTextColor,
                PointSize = (16.0f / 1.33f) - 4,
                FontFamily = "Samsung One 400",
                FontStyle = isActive ? new PropertyMap().Add("weight", new PropertyValue("bold")) : new PropertyMap()
            };

            // Add underline if active
            if (isActive)
            {
                View underline = new View()
                {
                    Size2D = new Size2D(60, 3),
                    BackgroundColor = accentColor,
                    Position2D = new Position2D(90, 50)
                };
                tabContainer.Add(underline);
            }

            tabContainer.Add(tabLabel);
        }

        private void RefreshRecipeCards()
        {
            // Clear existing cards
            while (recipeCardsContainer.ChildCount > 0)
            {
                View child = recipeCardsContainer.GetChildAt(0);
                recipeCardsContainer.Remove(child);
                child?.Dispose();
            }

            // Add new cards based on category
            switch (currentCategory)
            {
                case "APPETIZERS":
                    CreateRecipeCard("Stuffed Mushrooms", "25MIN", "342", "78", "appetizer.png");
                    CreateRecipeCard("Bruschetta", "15MIN", "289", "45", "maskgroup0.png");
                    CreateRecipeCard("Shrimp Cocktail", "20MIN", "456", "92", "maskgroup1.png");
                    break;
                case "ENTREES":
                    CreateRecipeCard("Prime Rib Roast", "5HR", "685", "107", "maskgroup0.png");
                    CreateRecipeCard("Grilled Salmon", "30MIN", "425", "89", "maskgroup1.png");
                    CreateRecipeCard("Chicken Parmesan", "45MIN", "567", "134", "rectangle0.png");
                    break;
                case "DESSERT":
                    CreateRecipeCard("Chocolate Cake", "2HR", "892", "156", "dessert.png");
                    CreateRecipeCard("Tiramisu", "4HR", "634", "98", "rectangle0.png");
                    CreateRecipeCard("Apple Pie", "1HR", "523", "87", "maskgroup0.png");
                    break;
            }
        }
    }
}
