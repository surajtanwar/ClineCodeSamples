using System;
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;
using TizenNUIApp.Models;
using TizenNUIApp.Controllers;

namespace TizenNUIApp.Views
{
    public class RecipeHomePageView : View
    {
        private RecipeController recipeController;
        private View headerView;
        private View categoryTabsView;
        private ScrollView recipeScrollView;
        private View recipeCardsContainer;
        private View recipeDetailsView;

        // Event for menu button click
        public event EventHandler MenuButtonClicked;
        public event EventHandler<string> CategoryChanged;

        // Colors based on typical recipe app design
        private readonly Color backgroundColor = new Color(0.98f, 0.98f, 0.98f, 1.0f);
        private readonly Color headerColor = Color.White;
        private readonly Color primaryTextColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);
        private readonly Color secondaryTextColor = new Color(0.6f, 0.6f, 0.6f, 1.0f);
        private readonly Color accentColor = new Color(0.95f, 0.4f, 0.4f, 1.0f);
        private readonly Color cardBackgroundColor = Color.White;

        // Tab references for dynamic updates
        private View appetizersTab;
        private View entreesTab;
        private View dessertTab;

        // Current recipe data
        private List<RecipeModel> currentRecipes;
        private RecipeModel currentRecipe;
        private int currentRecipeIndex = 0;

        public RecipeHomePageView(RecipeController controller)
        {
            recipeController = controller;
            InitializeComponent();
            CreateLayout();
            LoadRecipeData();
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

        private void LoadRecipeData()
        {
            var category = recipeController.GetCurrentCategory();
            currentRecipes = recipeController.GetRecipesByCategory(category);
            if (currentRecipes.Count > 0)
            {
                currentRecipe = currentRecipes[0];
                currentRecipeIndex = 0;
            }
            RefreshRecipeCards();
        }

        public void UpdateCategory(string category)
        {
            recipeController.SetCurrentCategory(category);
            LoadRecipeData();
            UpdateTabAppearance();
        }

        private void CreateHeader()
        {
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

            menuButton.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    MenuButtonClicked?.Invoke(this, EventArgs.Empty);
                    return true;
                }
                return false;
            };

            // Title
            TextLabel titleLabel = new TextLabel("POPULAR RECIPES")
            {
                Size2D = new Size2D(520, 60),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = accentColor,
                PointSize = (24.0f / 1.33f) - 6,
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

            appetizersTab = CreateCategoryTab("APPETIZERS", false);
            entreesTab = CreateCategoryTab("ENTREES", true);
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
                PointSize = (16.0f / 1.33f) - 4,
                FontFamily = "Samsung One 400",
                FontStyle = isActive ? new PropertyMap().Add("weight", new PropertyValue("bold")) : new PropertyMap()
            };

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

            tabContainer.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    CategoryChanged?.Invoke(this, text);
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
            View recipeSection = new View()
            {
                Size2D = new Size2D(720, 1080),
                BackgroundColor = backgroundColor,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    CellPadding = new Size2D(0, 0)
                }
            };

            CreateImageCarousel();
            CreateRecipeDetailsSection();

            recipeSection.Add(recipeScrollView);
            recipeSection.Add(recipeDetailsView);
            Add(recipeSection);
        }

        private void CreateImageCarousel()
        {
            recipeScrollView = new ScrollView()
            {
                Size2D = new Size2D(720, 400),
                BackgroundColor = backgroundColor,
                Position2D = new Position2D(0, 0)
            };

            recipeScrollView.SetAxisAutoLock(true);
            recipeScrollView.SetAxisAutoLockGradient(1.0f);

            recipeCardsContainer = new View()
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    CellPadding = new Size2D(0, 0)
                },
                Padding = new Extents(0, 0, 0, 0)
            };

            recipeScrollView.Add(recipeCardsContainer);
        }

        private void CreateRecipeDetailsSection()
        {
            recipeDetailsView = new View()
            {
                Size2D = new Size2D(720, 680),
                BackgroundColor = cardBackgroundColor,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    CellPadding = new Size2D(0, 20)
                },
                Padding = new Extents(30, 30, 30, 30)
            };

            UpdateRecipeDetails();
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

            // Create new cards
            if (currentRecipes != null)
            {
                foreach (var recipe in currentRecipes)
                {
                    CreateImageCard(recipe);
                }

                recipeCardsContainer.Size2D = new Size2D(720 * currentRecipes.Count, 400);
            }

            UpdateRecipeDetails();
            recipeScrollView.ScrollTo(new Vector2(0, 0), 0.0f);
        }

        private void CreateImageCard(RecipeModel recipe)
        {
            string resourcePath = Application.Current.DirectoryInfo.Resource;

            View imageCard = new View()
            {
                Size2D = new Size2D(720, 400),
                BackgroundColor = cardBackgroundColor
            };

            ImageView recipeImage = new ImageView()
            {
                Size2D = new Size2D(720, 400),
                Position2D = new Position2D(0, 0),
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "home", recipe.ImageName),
                FittingMode = FittingModeType.ScaleToFill
            };

            ImageView heartButton = new ImageView()
            {
                Size2D = new Size2D(40, 40),
                Position2D = new Position2D(650, 30),
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "home", "button-heart0.svg"),
                FittingMode = FittingModeType.ScaleToFill
            };

            imageCard.Add(recipeImage);
            imageCard.Add(heartButton);
            recipeCardsContainer.Add(imageCard);
        }

        private void UpdateRecipeDetails()
        {
            if (currentRecipe == null) return;

            // Clear existing details
            while (recipeDetailsView.ChildCount > 0)
            {
                View child = recipeDetailsView.GetChildAt(0);
                recipeDetailsView.Remove(child);
                child?.Dispose();
            }

            // Star rating
            View starRatingView = CreateStarRating();

            // Recipe title
            TextLabel titleLabel = new TextLabel(currentRecipe.Title)
            {
                Size2D = new Size2D(660, 80),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = primaryTextColor,
                PointSize = 20.0f - 4,
                FontFamily = "Samsung One 600",
                FontStyle = new PropertyMap().Add("weight", new PropertyValue("bold"))
            };

            // Stats row
            View statsView = CreateStatsView();

            // Recipe description
            TextLabel descriptionLabel = new TextLabel(currentRecipe.Description)
            {
                Size2D = new Size2D(660, 400),
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Top,
                TextColor = secondaryTextColor,
                PointSize = 14.0f,
                FontFamily = "Samsung One 400",
                MultiLine = true
            };

            recipeDetailsView.Add(starRatingView);
            recipeDetailsView.Add(titleLabel);
            recipeDetailsView.Add(statsView);
            recipeDetailsView.Add(descriptionLabel);
        }

        private View CreateStarRating()
        {
            string resourcePath = Application.Current.DirectoryInfo.Resource;

            View starContainer = new View()
            {
                Size2D = new Size2D(660, 30),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    LinearAlignment = LinearLayout.Alignment.Center,
                    CellPadding = new Size2D(2, 0)
                }
            };

            for (int i = 0; i < currentRecipe.Rating; i++)
            {
                ImageView star = new ImageView()
                {
                    Size2D = new Size2D(16, 16),
                    ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "home", "star4.svg"),
                    FittingMode = FittingModeType.ScaleToFill
                };
                starContainer.Add(star);
            }

            if (currentRecipe.Rating < 5)
            {
                ImageView emptyStar = new ImageView()
                {
                    Size2D = new Size2D(16, 16),
                    ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "home", "star0.svg"),
                    FittingMode = FittingModeType.ScaleToFill
                };
                starContainer.Add(emptyStar);
            }

            return starContainer;
        }

        private View CreateStatsView()
        {
            View statsView = new View()
            {
                Size2D = new Size2D(660, 50),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    LinearAlignment = LinearLayout.Alignment.Center,
                    CellPadding = new Size2D(40, 0)
                }
            };

            View timeView = CreateLargeStatItem("icons0.svg", currentRecipe.Time);
            View likesView = CreateLargeStatItem("icons1.svg", currentRecipe.Likes);
            View commentsView = CreateLargeStatItem("icons2.svg", currentRecipe.Comments);

            statsView.Add(timeView);
            statsView.Add(likesView);
            statsView.Add(commentsView);

            return statsView;
        }

        private View CreateLargeStatItem(string iconName, string value)
        {
            string resourcePath = Application.Current.DirectoryInfo.Resource;

            View statView = new View()
            {
                Size2D = new Size2D(120, 50),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    LinearAlignment = LinearLayout.Alignment.Center,
                    CellPadding = new Size2D(10, 0)
                }
            };

            ImageView icon = new ImageView()
            {
                Size2D = new Size2D(24, 24),
                ResourceUrl = System.IO.Path.Combine(resourcePath, "images", "home", iconName),
                FittingMode = FittingModeType.ScaleToFill
            };

            TextLabel valueLabel = new TextLabel(value)
            {
                Size2D = new Size2D(80, 50),
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = secondaryTextColor,
                PointSize = 12.0f - 4,
                FontFamily = "Samsung One 400"
            };

            statView.Add(icon);
            statView.Add(valueLabel);

            return statView;
        }

        private void UpdateTabAppearance()
        {
            var currentCategory = recipeController.GetCurrentCategory();
            UpdateTabState(appetizersTab, "APPETIZERS", currentCategory == "APPETIZERS");
            UpdateTabState(entreesTab, "ENTREES", currentCategory == "ENTREES");
            UpdateTabState(dessertTab, "DESSERT", currentCategory == "DESSERT");
        }

        private void UpdateTabState(View tabContainer, string tabText, bool isActive)
        {
            while (tabContainer.ChildCount > 0)
            {
                View child = tabContainer.GetChildAt(0);
                tabContainer.Remove(child);
                child?.Dispose();
            }

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
    }
}
