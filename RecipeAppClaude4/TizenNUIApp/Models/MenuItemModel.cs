using System;

namespace TizenNUIApp.Models
{
    public class MenuItemModel
    {
        public string Text { get; set; }
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public string IconName { get; set; }

        public MenuItemModel(string text, string id, bool isActive = false, string iconName = null)
        {
            Text = text;
            Id = id;
            IsActive = isActive;
            IconName = iconName;
        }
    }

    public class UserProfileModel
    {
        public string Name { get; set; }
        public string ProfileImageName { get; set; }

        public UserProfileModel(string name, string profileImageName)
        {
            Name = name;
            ProfileImageName = profileImageName;
        }
    }
}
