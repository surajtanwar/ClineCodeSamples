using System;

namespace TizenNUIApp.Models
{
    public class RecipeModel
    {
        public string Title { get; set; }
        public string Time { get; set; }
        public string Likes { get; set; }
        public string Comments { get; set; }
        public string ImageName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Rating { get; set; } = 4; // Default 4 stars
        
        public RecipeModel(string title, string time, string likes, string comments, string imageName, string category, string description = null)
        {
            Title = title;
            Time = time;
            Likes = likes;
            Comments = comments;
            ImageName = imageName;
            Category = category;
            Description = description ?? GetDefaultDescription(title);
        }

        private string GetDefaultDescription(string title)
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
    }
}
