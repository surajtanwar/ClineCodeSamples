# MVC Architecture Implementation for TizenNUIApp

## Overview
The TizenNUIApp has been refactored to follow the Model-View-Controller (MVC) architectural pattern with a dedicated Navigation Manager for handling page transitions and navigation operations.

## Project Structure

```
TizenNUIApp/
├── Models/
│   ├── RecipeModel.cs          # Recipe data model
│   └── MenuItemModel.cs        # Menu item and user profile models
├── Views/
│   ├── SplashScreenView.cs     # Splash screen view
│   ├── RecipeHomePageView.cs   # Recipe home page view
│   └── MenuPageView.cs         # Menu page view
├── Controllers/
│   ├── RecipeController.cs     # Recipe business logic
│   └── MenuController.cs       # Menu business logic
├── Navigation/
│   └── NavigationManager.cs    # Navigation handler
└── Program.cs                  # Main application entry point
```

## Architecture Components

### 1. Models
**Purpose**: Define data structures and business entities.

- **RecipeModel**: Represents recipe data including title, time, likes, comments, image, description, category, and rating.
- **MenuItemModel**: Represents menu items with text, ID, active state, and optional icon.
- **UserProfileModel**: Represents user profile information.

### 2. Views
**Purpose**: Handle UI presentation and user interactions.

- **SplashScreenView**: Displays the application splash screen with timer functionality.
- **RecipeHomePageView**: Shows recipe categories, recipe carousel, and detailed recipe information.
- **MenuPageView**: Displays navigation menu with user profile section.

### 3. Controllers
**Purpose**: Manage business logic and data operations.

- **RecipeController**: 
  - Manages recipe data by category
  - Provides methods to get recipes, categories, and current category
  - Handles category switching logic

- **MenuController**:
  - Manages menu items and user profile data
  - Provides methods to get menu items and set active states
  - Handles menu item selection logic

### 4. Navigation Manager
**Purpose**: Centralized navigation handling with show, hide, and back functionalities.

**Features**:
- **Page Navigation**: Navigate between different pages with optional data passing
- **Navigation Stack**: Maintains navigation history for back functionality
- **Page Caching**: Caches pages for performance optimization
- **Event-Driven**: Uses events to communicate navigation requests

**Key Methods**:
- `NavigateTo(PageType, data, addToStack)`: Navigate to a specific page
- `NavigateBack()`: Navigate to the previous page in the stack
- `ShowPage(View, PageType)`: Display a page and manage window operations
- `HideCurrentPage()`: Hide the currently displayed page
- `ClearNavigationStack()`: Clear navigation history

## Navigation Flow

```
Splash Screen → Home Page ⟷ Menu Page
                    ↓
            [Other Pages: Saved Recipes, Shopping List, Settings, Profile]
```

## Key Features Implemented

### 1. Separation of Concerns
- **Models**: Pure data structures without UI dependencies
- **Views**: UI components that consume data from controllers
- **Controllers**: Business logic separated from UI and data models

### 2. Navigation Management
- **Centralized Navigation**: All navigation logic handled by NavigationManager
- **Back Navigation**: Automatic back stack management
- **Page Caching**: Improved performance through view caching
- **Event-Driven Architecture**: Loose coupling between components

### 3. Event-Driven Communication
- Views communicate with the main application through events
- Controllers provide data through method calls
- Navigation requests are handled through event system

## Usage Examples

### Creating a New Page
1. Create a model in `Models/` folder
2. Create a controller in `Controllers/` folder
3. Create a view in `Views/` folder
4. Add page type to `PageType` enum in NavigationManager
5. Handle navigation in `Program.cs`

### Adding Navigation
```csharp
// Navigate to a page
navigationManager.NavigateTo(PageType.NewPage, data);

// Navigate back
navigationManager.NavigateBack();

// Check if can navigate back
if (navigationManager.CanNavigateBack())
{
    navigationManager.NavigateBack();
}
```

### Controller Usage
```csharp
// In View constructor
public MyView(MyController controller)
{
    this.controller = controller;
    LoadData();
}

private void LoadData()
{
    var data = controller.GetData();
    UpdateUI(data);
}
```

## Benefits of This Architecture

1. **Maintainability**: Clear separation makes code easier to maintain
2. **Testability**: Controllers can be unit tested independently
3. **Reusability**: Models and controllers can be reused across different views
4. **Scalability**: Easy to add new pages and features
5. **Navigation Management**: Centralized navigation with back functionality
6. **Performance**: Page caching improves app performance

## Navigation Manager Features

### Show/Hide Operations
- `ShowPage()`: Displays a page and adds it to the window
- `HideCurrentPage()`: Removes current page from the window
- Automatic page lifecycle management

### Back Functionality
- Maintains navigation stack automatically
- `NavigateBack()` returns to previous page
- `CanNavigateBack()` checks if back navigation is possible
- `ClearNavigationStack()` resets navigation history

### Page Caching
- Caches created pages for performance
- `GetCachedPage()` retrieves cached pages
- `ClearPageCache()` clears all cached pages
- Automatic disposal of cached pages

## Future Enhancements

1. **Page Transitions**: Add animated transitions between pages
2. **Deep Linking**: Support for direct navigation to specific pages
3. **State Management**: Preserve page state during navigation
4. **Lazy Loading**: Load pages only when needed
5. **Navigation Guards**: Add navigation validation and guards
