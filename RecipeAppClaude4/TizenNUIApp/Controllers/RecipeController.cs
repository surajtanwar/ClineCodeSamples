using System;
using System.Collections.Generic;
using System.Linq;
using TizenNUIApp.Models;

namespace TizenNUIApp.Controllers
{
    public class RecipeController
    {
        private Dictionary<string, List<RecipeModel>> recipesByCategory;
        private string currentCategory;

        public RecipeController()
        {
            InitializeRecipeData();
            currentCategory = "ENTREES";
        }

        public List<RecipeModel> GetRecipesByCategory(string category)
        {
            if (recipesByCategory.ContainsKey(category))
            {
                return recipesByCategory[category];
            }
            return new List<RecipeModel>();
        }

        public RecipeModel GetRecipeById(string category, int index)
        {
            var recipes = GetRecipesByCategory(category);
            if (index >= 0 && index < recipes.Count)
            {
                return recipes[index];
            }
            return null;
        }

        public List<string> GetCategories()
        {
            return recipesByCategory.Keys.ToList();
        }

        public void SetCurrentCategory(string category)
        {
            if (recipesByCategory.ContainsKey(category))
            {
                currentCategory = category;
            }
        }

        public string GetCurrentCategory()
        {
            return currentCategory;
        }

        private void InitializeRecipeData()
        {
            recipesByCategory = new Dictionary<string, List<RecipeModel>>();

            // Appetizers
            recipesByCategory["APPETIZERS"] = new List<RecipeModel>
            {
                new RecipeModel("Stuffed Mushrooms", "25MIN", "342", "78", "appetizer.png", "APPETIZERS"),
                new RecipeModel("Bruschetta", "15MIN", "289", "45", "maskgroup0.png", "APPETIZERS"),
                new RecipeModel("Shrimp Cocktail", "20MIN", "456", "92", "maskgroup1.png", "APPETIZERS")
            };

            // Entrees
            recipesByCategory["ENTREES"] = new List<RecipeModel>
            {
                new RecipeModel("Prime Rib Roast", "5HR", "685", "107", "maskgroup0.png", "ENTREES"),
                new RecipeModel("Grilled Salmon", "30MIN", "425", "89", "maskgroup1.png", "ENTREES"),
                new RecipeModel("Chicken Parmesan", "45MIN", "567", "134", "rectangle0.png", "ENTREES")
            };

            // Desserts
            recipesByCategory["DESSERT"] = new List<RecipeModel>
            {
                new RecipeModel("Chocolate Cake", "2HR", "892", "156", "dessert.png", "DESSERT"),
                new RecipeModel("Tiramisu", "4HR", "634", "98", "rectangle0.png", "DESSERT"),
                new RecipeModel("Apple Pie", "1HR", "523", "87", "maskgroup0.png", "DESSERT")
            };
        }
    }
}
