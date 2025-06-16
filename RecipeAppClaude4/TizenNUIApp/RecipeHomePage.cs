using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace TizenNUIApp
{
    public class RecipeHomePage : View
    {
        private View headerView;
        private View categoryTabsView;
        private ScrollView recipeScrollView;
        private View recipeCardsContainer;

        // Colors based on typical recipe app design
        private readonly Color backgroundColor = new Color(0.98f, 0.98f, 0.98f, 1.0f);
        private readonly Color headerColor = Color.White;
        private readonly Color primaryTextColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);
        private readonly Color secondaryTextColor = new Color(0.6f, 0.6f, 0.6f, 1.0f);
        private readonly Color accentColor = new Color(0.95f, 0.4f, 0.4f, 1.0f); // Coral/salmon color
        private readonly Color cardBackgroundColor = Color.White;

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
                ResourceUrl = "res/images/home/btn-menu0.svg",
                FittingMode = FittingModeType.ScaleToFill
            };

            // Title
            TextLabel titleLabel = new TextLabel("POPULAR RECIPES")
            {
                Size2D = new Size2D(520, 60),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = accentColor,
                PointSize = 24.0f * 1.33f, // Converting to px: 24pt * 1.33 = 32px
                FontFamily = "Samsung One 600",
                FontStyle = new PropertyMap().Add("weight", new PropertyValue("bold"))
            };

            // Search button
            ImageView searchButton = new ImageView()
            {
                Size2D = new Size2D(32, 32),
                ResourceUrl = "res/images/home/btn-search0.svg",
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

            // Create category tabs
            CreateCategoryTab("APPETIZERS", true);
            CreateCategoryTab("ENTREES", false);
            CreateCategoryTab("DESSERT", false);

            Add(categoryTabsView);
        }

        private void CreateCategoryTab(string text, bool isActive)
        {
            View tabContainer = new View()
            {
                Size2D = new Size2D(240, 60),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    LinearAlignment = LinearLayout.Alignment.Center
                }
            };

            TextLabel tabLabel = new TextLabel(text)
            {
                Size2D = new Size2D(240, 40),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = isActive ? primaryTextColor : secondaryTextColor,
                PointSize = 16.0f * 1.33f, // 16pt * 1.33 = 21px
                FontFamily = "Samsung One 400",
                FontStyle = isActive ? new PropertyMap().Add("weight", new PropertyValue("bold")) : new PropertyMap()
            };

            // Active tab underline
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
            categoryTabsView.Add(tabContainer);
        }

        private void CreateRecipeScrollView()
        {
            recipeScrollView = new ScrollView()
            {
                Size2D = new Size2D(720, 1080),
                BackgroundColor = backgroundColor
            };

            recipeCardsContainer = new View()
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    CellPadding = new Size2D(0, 20)
                },
                Padding = new Extents(20, 20, 20, 20)
            };

            // Create recipe cards
            CreateRecipeCard("Prime Rib Roast", "5HR", "685", "107", "maskgroup0.png");
            CreateRecipeCard("Grilled Salmon", "30MIN", "425", "89", "maskgroup1.png");
            CreateRecipeCard("Chocolate Cake", "2HR", "892", "156", "rectangle0.png");

            recipeScrollView.Add(recipeCardsContainer);
            Add(recipeScrollView);
        }

        private void CreateRecipeCard(string title, string time, string likes, string comments, string imageName)
        {
            View cardView = new View()
            {
                Size2D = new Size2D(680, 320),
                BackgroundColor = cardBackgroundColor,
                CornerRadius = 16.0f,
                // BoxShadow = new Shadow(8.0f, new Color(0.0f, 0.0f, 0.0f, 0.1f), new Vector2(0, 4)),
                Margin = new Extents(0, 0, 10, 10)
            };

            // Recipe image
            ImageView recipeImage = new ImageView()
            {
                Size2D = new Size2D(680, 200),
                Position2D = new Position2D(0, 0),
                ResourceUrl = $"res/images/home/{imageName}",
                FittingMode = FittingModeType.ScaleToFill
            };

            // Heart button overlay
            ImageView heartButton = new ImageView()
            {
                Size2D = new Size2D(40, 40),
                Position2D = new Position2D(620, 20),
                ResourceUrl = "res/images/home/button-heart0.svg",
                FittingMode = FittingModeType.ScaleToFill
            };

            // Content area
            View contentView = new View()
            {
                Size2D = new Size2D(680, 120),
                Position2D = new Position2D(0, 200),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    CellPadding = new Size2D(0, 8)
                },
                Padding = new Extents(20, 20, 15, 15)
            };

            // Star rating
            View starRatingView = CreateStarRating();

            // Recipe title
            TextLabel titleLabel = new TextLabel(title)
            {
                Size2D = new Size2D(640, 35),
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = primaryTextColor,
                PointSize = 20.0f * 1.33f, // 20pt * 1.33 = 27px
                FontFamily = "Samsung One 600",
                FontStyle = new PropertyMap().Add("weight", new PropertyValue("bold"))
            };

            // Stats row
            View statsView = new View()
            {
                Size2D = new Size2D(640, 30),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    LinearAlignment = LinearLayout.Alignment.Begin,
                    CellPadding = new Size2D(20, 0)
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
            contentView.Add(statsView);

            cardView.Add(recipeImage);
            cardView.Add(heartButton);
            cardView.Add(contentView);

            recipeCardsContainer.Add(cardView);
        }

        private View CreateStarRating()
        {
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
                    ResourceUrl = "res/images/home/star4.svg", // Filled star
                    FittingMode = FittingModeType.ScaleToFill
                };
                starContainer.Add(star);
            }

            // Empty star
            ImageView emptyStar = new ImageView()
            {
                Size2D = new Size2D(16, 16),
                ResourceUrl = "res/images/home/star0.svg", // Empty star
                FittingMode = FittingModeType.ScaleToFill
            };
            starContainer.Add(emptyStar);

            return starContainer;
        }

        private View CreateStatItem(string iconName, string value)
        {
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
                ResourceUrl = $"res/images/home/{iconName}",
                FittingMode = FittingModeType.ScaleToFill
            };

            TextLabel valueLabel = new TextLabel(value)
            {
                Size2D = new Size2D(60, 30),
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = secondaryTextColor,
                PointSize = 12.0f * 1.33f, // 12pt * 1.33 = 16px
                FontFamily = "Samsung One 400"
            };

            statView.Add(icon);
            statView.Add(valueLabel);

            return statView;
        }
    }
}
