# Sequence Diagrams for TizenNUIApp Recipe Application

## Current Architecture Sequence Diagrams

### 1. Application Startup and Splash Screen Flow

```mermaid
sequenceDiagram
    participant User
    participant Program
    participant NavigationManager
    participant SplashScreenView
    participant RecipeHomePageView
    participant RecipeController

    User->>Program: Launch App
    Program->>Program: OnCreate()
    Program->>Program: Initialize()
    Program->>NavigationManager: new NavigationManager(window)
    Program->>RecipeController: new RecipeController()
    Program->>Program: ShowSplashScreen()
    Program->>SplashScreenView: new SplashScreenView()
    Program->>NavigationManager: ShowPage(splashScreenView, PageType.Splash)
    NavigationManager->>SplashScreenView: Add to window
    
    Note over SplashScreenView: Timer runs for splash duration
    
    SplashScreenView->>Program: SplashCompleted event
    Program->>NavigationManager: NavigateTo(PageType.Home, addToStack: false)
    NavigationManager->>Program: NavigationRequested event
    Program->>Program: ShowHomePage()
    Program->>RecipeHomePageView: new RecipeHomePageView(recipeController)
    Program->>NavigationManager: ShowPage(recipeHomePageView, PageType.Home)
    NavigationManager->>RecipeHomePageView: Add to window
    NavigationManager->>SplashScreenView: Remove from window
```

### 2. Recipe Category Selection Flow

```mermaid
sequenceDiagram
    participant User
    participant RecipeHomePageView
    participant RecipeController
    participant Program

    User->>RecipeHomePageView: Tap Category Button
    RecipeHomePageView->>RecipeHomePageView: OnCategoryButtonClicked()
    RecipeHomePageView->>Program: CategoryChanged event
    Program->>RecipeHomePageView: UpdateCategory(category)
    RecipeHomePageView->>RecipeController: GetRecipesByCategory(category)
    RecipeController->>RecipeController: Check recipesByCategory dictionary
    RecipeController-->>RecipeHomePageView: Return List<RecipeModel>
    RecipeHomePageView->>RecipeHomePageView: UpdateRecipeCarousel()
    RecipeHomePageView->>RecipeHomePageView: UpdateUI with new recipes
    RecipeHomePageView-->>User: Display updated recipes
```

### 3. Navigation to Menu Page Flow

```mermaid
sequenceDiagram
    participant User
    participant RecipeHomePageView
    participant Program
    participant NavigationManager
    participant MenuPageView
    participant MenuController

    User->>RecipeHomePageView: Tap Menu Button
    RecipeHomePageView->>Program: MenuButtonClicked event
    Program->>NavigationManager: NavigateTo(PageType.Menu)
    NavigationManager->>NavigationManager: Push current page to stack
    NavigationManager->>Program: NavigationRequested event
    Program->>Program: ShowMenuPage()
    
    alt MenuPageView not created
        Program->>MenuPageView: new MenuPageView(menuController)
        Program->>MenuPageView: Subscribe to events
    end
    
    Program->>NavigationManager: ShowPage(menuPageView, PageType.Menu)
    NavigationManager->>RecipeHomePageView: Remove from window
    NavigationManager->>MenuPageView: Add to window
    MenuPageView-->>User: Display menu page
```

### 4. Menu Item Selection and Back Navigation Flow

```mermaid
sequenceDiagram
    participant User
    participant MenuPageView
    participant Program
    participant NavigationManager

    User->>MenuPageView: Tap Menu Item
    MenuPageView->>Program: MenuItemSelected event
    Program->>Program: OnMenuItemSelected(menuItem)
    
    alt menuItem == "popular_recipes"
        Program->>NavigationManager: NavigateTo(PageType.Home, addToStack: false)
    else Other menu items
        Program->>NavigationManager: NavigateTo(respective PageType)
        Note over Program: Currently redirects to Home for unimplemented pages
    end
    
    NavigationManager->>Program: NavigationRequested event
    Program->>Program: Handle navigation request
    
    Note over User: User taps back button
    User->>MenuPageView: Tap Back Button
    MenuPageView->>Program: BackButtonClicked event
    Program->>NavigationManager: NavigateBack()
    NavigationManager->>NavigationManager: Pop from navigation stack
    NavigationManager->>Program: NavigationRequested event
    Program->>NavigationManager: ShowPage(previousPage)
    NavigationManager->>MenuPageView: Remove from window
    NavigationManager->>RecipeHomePageView: Add to window
```

## Proposed Architecture Sequence Diagrams

### 1. Enhanced Application Startup with Dependency Injection

```mermaid
sequenceDiagram
    participant User
    participant Program
    participant ServiceContainer
    participant NavigationService
    participant SplashScreenViewModel
    participant SplashScreenView
    participant RecipeHomePageViewModel

    User->>Program: Launch App
    Program->>Program: OnCreate()
    Program->>ServiceContainer: RegisterServices()
    ServiceContainer->>ServiceContainer: Register all dependencies
    Program->>NavigationService: Resolve<INavigationService>()
    Program->>NavigationService: NavigateToAsync<SplashScreenViewModel>()
    
    NavigationService->>ServiceContainer: Resolve<SplashScreenViewModel>()
    ServiceContainer->>SplashScreenViewModel: new SplashScreenViewModel(dependencies)
    NavigationService->>SplashScreenView: CreateViewForViewModel(viewModel)
    NavigationService->>SplashScreenViewModel: OnNavigatedToAsync()
    NavigationService->>SplashScreenView: Add to window
    
    Note over SplashScreenViewModel: Timer completes
    
    SplashScreenViewModel->>NavigationService: NavigateToAsync<RecipeHomePageViewModel>()
    NavigationService->>ServiceContainer: Resolve<RecipeHomePageViewModel>()
    NavigationService->>RecipeHomePageViewModel: OnNavigatedToAsync()
    NavigationService->>NavigationService: ShowView(homePageView)
```

### 2. Recipe Data Retrieval with Use Cases and Caching

```mermaid
sequenceDiagram
    participant User
    participant RecipeHomePageView
    participant RecipeHomePageViewModel
    participant GetRecipesByCategoryUseCase
    participant CacheRepository
    participant RecipeRepository
    participant RecipeApiClient

    User->>RecipeHomePageView: Select Category
    RecipeHomePageView->>RecipeHomePageViewModel: CategoryChanged command
    RecipeHomePageViewModel->>RecipeHomePageViewModel: SetProperty(IsLoading, true)
    RecipeHomePageViewModel->>GetRecipesByCategoryUseCase: ExecuteAsync(request)
    
    GetRecipesByCategoryUseCase->>CacheRepository: GetAsync("recipes_category")
    
    alt Cache Hit
        CacheRepository-->>GetRecipesByCategoryUseCase: Return cached recipes
        GetRecipesByCategoryUseCase-->>RecipeHomePageViewModel: Result.Success(recipes)
    else Cache Miss
        GetRecipesByCategoryUseCase->>RecipeRepository: GetByCategoryAsync(category)
        RecipeRepository->>RecipeApiClient: FetchRecipesAsync(category)
        RecipeApiClient-->>RecipeRepository: Return API data
        RecipeRepository-->>GetRecipesByCategoryUseCase: Return domain entities
        GetRecipesByCategoryUseCase->>CacheRepository: SetAsync("recipes_category", recipes, expiration)
        GetRecipesByCategoryUseCase-->>RecipeHomePageViewModel: Result.Success(recipes)
    end
    
    RecipeHomePageViewModel->>RecipeHomePageViewModel: SetProperty(Recipes, recipes)
    RecipeHomePageViewModel->>RecipeHomePageViewModel: SetProperty(IsLoading, false)
    RecipeHomePageView->>RecipeHomePageView: Update UI via data binding
    RecipeHomePageView-->>User: Display updated recipes
```

### 3. Enhanced Navigation with State Management

```mermaid
sequenceDiagram
    participant User
    participant RecipeHomePageView
    participant RecipeHomePageViewModel
    participant NavigationService
    participant MenuPageViewModel
    participant MenuPageView
    participant EventBus

    User->>RecipeHomePageView: Tap Menu Button
    RecipeHomePageView->>RecipeHomePageViewModel: MenuCommand executed
    RecipeHomePageViewModel->>NavigationService: NavigateToAsync<MenuPageViewModel>()
    
    NavigationService->>NavigationService: SaveCurrentState()
    NavigationService->>NavigationService: Push to navigation stack
    NavigationService->>ServiceContainer: Resolve<MenuPageViewModel>()
    NavigationService->>MenuPageView: CreateViewForViewModel(menuViewModel)
    NavigationService->>MenuPageViewModel: OnNavigatedToAsync(parameter)
    
    MenuPageViewModel->>EventBus: Subscribe<UserProfileUpdatedEvent>()
    MenuPageViewModel->>GetUserProfileUseCase: ExecuteAsync()
    GetUserProfileUseCase-->>MenuPageViewModel: Return user profile
    MenuPageViewModel->>MenuPageViewModel: SetProperty(UserProfile, profile)
    
    NavigationService->>RecipeHomePageView: OnNavigatedFromAsync()
    NavigationService->>MenuPageView: Add to window
    NavigationService->>RecipeHomePageView: Remove from window
    MenuPageView-->>User: Display menu with user profile
```

### 4. Error Handling and Recovery Flow

```mermaid
sequenceDiagram
    participant User
    participant RecipeHomePageView
    participant RecipeHomePageViewModel
    participant GetRecipesByCategoryUseCase
    participant RecipeRepository
    participant ErrorHandlingService
    participant NotificationService

    User->>RecipeHomePageView: Select Category
    RecipeHomePageView->>RecipeHomePageViewModel: CategoryChanged command
    RecipeHomePageViewModel->>GetRecipesByCategoryUseCase: ExecuteAsync(request)
    GetRecipesByCategoryUseCase->>RecipeRepository: GetByCategoryAsync(category)
    
    RecipeRepository->>RecipeRepository: Network/Database error occurs
    RecipeRepository-->>GetRecipesByCategoryUseCase: Throw exception
    GetRecipesByCategoryUseCase->>ErrorHandlingService: HandleException(ex)
    ErrorHandlingService->>ErrorHandlingService: Log error details
    GetRecipesByCategoryUseCase-->>RecipeHomePageViewModel: Result.Failure(error)
    
    RecipeHomePageViewModel->>RecipeHomePageViewModel: SetProperty(HasError, true)
    RecipeHomePageViewModel->>RecipeHomePageViewModel: SetProperty(ErrorMessage, error.Message)
    RecipeHomePageViewModel->>NotificationService: ShowErrorNotification(error)
    
    RecipeHomePageView->>RecipeHomePageView: Update UI to show error state
    RecipeHomePageView-->>User: Display error message with retry option
    
    User->>RecipeHomePageView: Tap Retry Button
    RecipeHomePageView->>RecipeHomePageViewModel: RetryCommand executed
    RecipeHomePageViewModel->>RecipeHomePageViewModel: SetProperty(HasError, false)
    Note over RecipeHomePageViewModel: Retry the original operation
```

### 5. Plugin Architecture Integration Flow

```mermaid
sequenceDiagram
    participant Program
    participant PluginManager
    participant ExternalPlugin
    participant ServiceContainer
    participant EventBus
    participant RecipeHomePageViewModel

    Program->>PluginManager: LoadPluginsAsync(pluginDirectory)
    PluginManager->>PluginManager: Scan for plugin assemblies
    PluginManager->>ExternalPlugin: Load assembly and create instance
    PluginManager->>ExternalPlugin: Initialize(serviceContainer)
    
    ExternalPlugin->>ServiceContainer: RegisterService<ICustomService, CustomService>()
    ExternalPlugin->>EventBus: Subscribe<RecipeViewedEvent>(handler)
    ExternalPlugin->>PluginManager: Registration complete
    
    Note over Program: Application runs normally
    
    RecipeHomePageViewModel->>EventBus: PublishAsync<RecipeViewedEvent>(event)
    EventBus->>ExternalPlugin: Handle RecipeViewedEvent
    ExternalPlugin->>ExternalPlugin: Process recipe analytics
    ExternalPlugin->>ServiceContainer: Resolve<IAnalyticsService>()
    ExternalPlugin->>ExternalPlugin: Send analytics data
```

### 6. Virtual Scrolling Performance Optimization

```mermaid
sequenceDiagram
    participant User
    participant VirtualScrollView
    participant RecipeHomePageViewModel
    participant ItemViewPool
    participant RecipeCardView

    User->>VirtualScrollView: Scroll down
    VirtualScrollView->>VirtualScrollView: OnScrolling(scrollEventArgs)
    VirtualScrollView->>VirtualScrollView: CalculateVisibleRange()
    
    loop For each item going out of view
        VirtualScrollView->>RecipeCardView: Remove from visual tree
        VirtualScrollView->>ItemViewPool: ReturnToPool(recipeCardView)
    end
    
    loop For each new item coming into view
        VirtualScrollView->>ItemViewPool: GetFromPool<RecipeCardView>()
        alt Pool has available view
            ItemViewPool-->>VirtualScrollView: Return recycled view
        else Pool is empty
            ItemViewPool->>RecipeCardView: new RecipeCardView()
            ItemViewPool-->>VirtualScrollView: Return new view
        end
        
        VirtualScrollView->>RecipeHomePageViewModel: GetItemAt(index)
        RecipeHomePageViewModel-->>VirtualScrollView: Return recipe data
        VirtualScrollView->>RecipeCardView: UpdateData(recipe)
        VirtualScrollView->>VirtualScrollView: Add to visual tree
    end
    
    VirtualScrollView-->>User: Display updated visible items
```

## Key Improvements in Proposed Architecture

### 1. **Separation of Concerns**
- ViewModels handle presentation logic
- Use Cases encapsulate business logic
- Repositories manage data access
- Services provide cross-cutting concerns

### 2. **Async Operations**
- All data operations are asynchronous
- Non-blocking UI interactions
- Better error handling and recovery

### 3. **Dependency Injection**
- Loose coupling between components
- Easy testing and mocking
- Configurable service lifetimes

### 4. **Event-Driven Architecture**
- Decoupled communication via EventBus
- Plugin support through event subscriptions
- Better scalability and maintainability

### 5. **Performance Optimizations**
- Multi-level caching strategy
- Virtual scrolling for large lists
- Resource pooling and recycling
- Memory management improvements

### 6. **Error Handling**
- Consistent error handling patterns
- User-friendly error messages
- Automatic retry mechanisms
- Comprehensive logging

These sequence diagrams illustrate how the proposed architecture addresses the limitations of the current implementation while providing a foundation for scalable, maintainable, and performant application development.
