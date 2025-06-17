using System;
using System.IO;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;
using Tizen.NUI.Components;

namespace TizenNUIApp
{
    /// <summary>
    /// Optimized splash screen component with improved error handling, resource management, and maintainability.
    /// Implements proper disposal patterns and configuration-driven approach.
    /// </summary>
    public class SplashScreen : View
    {
        #region Constants
        private const int TARGET_WIDTH = 720;
        private const int TARGET_HEIGHT = 1280;
        private const int DESIGN_WIDTH = 375;
        private const int DESIGN_HEIGHT = 667;
        private const int SPLASH_DURATION_MS = 2000;
        private const string SPLASH_IMAGE_FOLDER = "splash";
        #endregion

        #region Fields
        private Timer splashTimer;
        private ScalingInfo scalingInfo;
        private bool disposed = false;
        private readonly object lockObject = new object();
        #endregion

        #region Events
        /// <summary>
        /// Raised when the splash screen duration has completed.
        /// </summary>
        public event EventHandler SplashCompleted;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the SplashScreen class.
        /// </summary>
        public SplashScreen()
        {
            try
            {
                InitializeSplashScreen();
                StartSplashTimer();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to initialize splash screen: {ex.Message}");
                // Fallback: Complete splash immediately
                OnSplashCompleted();
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Stops the splash screen and triggers completion immediately.
        /// </summary>
        public void CompleteSplash()
        {
            if (disposed) return;

            lock (lockObject)
            {
                StopTimer();
                OnSplashCompleted();
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initializes the splash screen with improved error handling.
        /// </summary>
        private void InitializeSplashScreen()
        {
            Console.WriteLine("[INFO] Initializing splash screen");
            
            SetupContainer();
            scalingInfo = CalculateScaling();
            CreateBackgroundElements();
            
            Console.WriteLine("[INFO] Splash screen initialized successfully");
        }

        /// <summary>
        /// Sets up the main container properties.
        /// </summary>
        private void SetupContainer()
        {
            this.Size2D = new Size2D(TARGET_WIDTH, TARGET_HEIGHT);
            this.BackgroundColor = Color.White;
            this.Position2D = new Position2D(0, 0);
            
            Console.WriteLine($"[DEBUG] Container setup: Size={TARGET_WIDTH}x{TARGET_HEIGHT}");
        }

        /// <summary>
        /// Calculates scaling factors for responsive design.
        /// </summary>
        private ScalingInfo CalculateScaling()
        {
            var scaling = new ScalingInfo
            {
                HorizontalFactor = (float)TARGET_WIDTH / DESIGN_WIDTH,
                VerticalFactor = (float)TARGET_HEIGHT / DESIGN_HEIGHT
            };

            Console.WriteLine($"[DEBUG] Scaling calculated: H={scaling.HorizontalFactor:F2}, V={scaling.VerticalFactor:F2}");
            return scaling;
        }

        /// <summary>
        /// Creates and positions all background elements with error handling.
        /// </summary>
        private void CreateBackgroundElements()
        {
            var elements = GetSplashElements();
            
            foreach (var element in elements)
            {
                try
                {
                    var imageView = CreateScaledImageView(element);
                    if (imageView != null)
                    {
                        this.Add(imageView);
                        Console.WriteLine($"[DEBUG] Added splash element: {element.ImageName}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[WARN] Failed to create splash element {element.ImageName}: {ex.Message}");
                    // Continue with other elements
                }
            }
        }

        /// <summary>
        /// Gets the configuration for splash screen elements.
        /// </summary>
        private SplashElement[] GetSplashElements()
        {
            return new[]
            {
                new SplashElement
                {
                    ImageName = "Rectangle.png",
                    Position = new Position2D(0, 0),
                    Size = new Size2D(375, 667)
                },
                new SplashElement
                {
                    ImageName = "Group.png",
                    Position = new Position2D(91, 111)
                },
                new SplashElement
                {
                    ImageName = "Group_2.png",
                    Position = new Position2D(93, 365)
                }
            };
        }

        /// <summary>
        /// Creates a scaled ImageView for a splash element with error handling.
        /// </summary>
        private ImageView CreateScaledImageView(SplashElement element)
        {
            try
            {
                string resourcePath = Application.Current.DirectoryInfo.Resource;
                var imagePath = System.IO.Path.Combine(resourcePath, "images", SPLASH_IMAGE_FOLDER, element.ImageName);
                
                if (!System.IO.File.Exists(imagePath))
                {
                    Console.WriteLine($"[WARN] Image not found: {imagePath}");
                    return CreateFallbackImageView(element);
                }

                var imageView = new ImageView
                {
                    ResourceUrl = imagePath,
                    Position2D = new Position2D(
                        (int)(element.Position.X * scalingInfo.HorizontalFactor),
                        (int)(element.Position.Y * scalingInfo.VerticalFactor)
                    ),
                    WidthResizePolicy = ResizePolicyType.UseNaturalSize,
                    HeightResizePolicy = ResizePolicyType.UseNaturalSize
                };

                // Apply size if specified
                if (element.Size != null)
                {
                    imageView.Size2D = new Size2D(
                        (int)(element.Size.Width * scalingInfo.HorizontalFactor),
                        (int)(element.Size.Height * scalingInfo.VerticalFactor)
                    );
                    imageView.WidthResizePolicy = ResizePolicyType.Fixed;
                    imageView.HeightResizePolicy = ResizePolicyType.Fixed;
                }

                return imageView;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to create ImageView for {element.ImageName}: {ex.Message}");
                return CreateFallbackImageView(element);
            }
        }

        /// <summary>
        /// Creates a fallback ImageView when the original image fails to load.
        /// </summary>
        private ImageView CreateFallbackImageView(SplashElement element)
        {
            try
            {
                // Create a simple colored view as fallback
                var fallbackView = new ImageView
                {
                    BackgroundColor = new Color(0.9f, 0.9f, 0.9f, 1.0f), // Light gray
                    Position2D = new Position2D(
                        (int)(element.Position.X * scalingInfo.HorizontalFactor),
                        (int)(element.Position.Y * scalingInfo.VerticalFactor)
                    ),
                    Size2D = element.Size != null ? 
                        new Size2D(
                            (int)(element.Size.Width * scalingInfo.HorizontalFactor),
                            (int)(element.Size.Height * scalingInfo.VerticalFactor)
                        ) : 
                        new Size2D(50, 50), // Default size
                    WidthResizePolicy = ResizePolicyType.Fixed,
                    HeightResizePolicy = ResizePolicyType.Fixed
                };

                Console.WriteLine($"[INFO] Created fallback view for {element.ImageName}");
                return fallbackView;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to create fallback image: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Starts the splash screen timer with error handling.
        /// </summary>
        private void StartSplashTimer()
        {
            if (disposed)
            {
                Console.WriteLine("[WARN] Cannot start timer on disposed SplashScreen");
                return;
            }

            lock (lockObject)
            {
                if (splashTimer != null)
                {
                    Console.WriteLine("[WARN] Splash timer is already running");
                    return;
                }

                try
                {
                    splashTimer = new Timer(SPLASH_DURATION_MS);
                    splashTimer.Tick += OnSplashTimerTick;
                    splashTimer.Start();
                    Console.WriteLine($"[INFO] Splash timer started for {SPLASH_DURATION_MS}ms");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to start splash timer: {ex.Message}");
                    // Fallback: Complete splash immediately
                    OnSplashCompleted();
                }
            }
        }

        /// <summary>
        /// Handles the splash timer tick event with error handling.
        /// </summary>
        private bool OnSplashTimerTick(object sender, Timer.TickEventArgs e)
        {
            try
            {
                Console.WriteLine("[INFO] Splash timer completed");
                StopTimer();
                OnSplashCompleted();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error in splash timer tick: {ex.Message}");
                OnSplashCompleted();
            }

            return false; // Don't repeat the timer
        }

        /// <summary>
        /// Stops and disposes the splash timer safely.
        /// </summary>
        private void StopTimer()
        {
            lock (lockObject)
            {
                if (splashTimer != null)
                {
                    try
                    {
                        splashTimer.Stop();
                        splashTimer.Dispose();
                        splashTimer = null;
                        Console.WriteLine("[DEBUG] Splash timer stopped and disposed");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[WARN] Error stopping splash timer: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Triggers the splash completed event safely.
        /// </summary>
        private void OnSplashCompleted()
        {
            try
            {
                Console.WriteLine("[INFO] Splash screen completed");
                SplashCompleted?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error in splash completed event: {ex.Message}");
            }
        }
        #endregion

        #region Disposal
        /// <summary>
        /// Disposes the splash screen and all its resources safely.
        /// </summary>
        protected override void Dispose(DisposeTypes type)
        {
            if (!disposed)
            {
                lock (lockObject)
                {
                    try
                    {
                        StopTimer();
                        
                        // Dispose all child views
                        foreach (View child in Children)
                        {
                            child?.Dispose();
                        }
                        
                        disposed = true;
                        Console.WriteLine("[INFO] SplashScreen disposed successfully");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERROR] Error during SplashScreen disposal: {ex.Message}");
                    }
                }
            }
            
            base.Dispose(type);
        }
        #endregion
    }

    #region Supporting Classes
    /// <summary>
    /// Represents scaling information for responsive design.
    /// </summary>
    public class ScalingInfo
    {
        public float HorizontalFactor { get; set; }
        public float VerticalFactor { get; set; }
    }

    /// <summary>
    /// Represents a splash screen element configuration.
    /// </summary>
    public class SplashElement
    {
        public string ImageName { get; set; }
        public Position2D Position { get; set; }
        public Size2D Size { get; set; }
    }
    #endregion
}
