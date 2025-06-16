### Resolved Issues
- Fixed `ScrollableBase` API compatibility by using `ScrollView`
- Corrected `PivotPoint.Center` and `ParentOrigin.Center` usage with proper positioning
- Updated `FittingModeType.FillBounds` to `FittingModeType.ScaleToFill`
- Fixed `FittingModeType.Fill` compatibility issue by using `ScaleToFill`
- Removed `CornerRadius` Vector4 assignment that caused type conversion errors
- Resolved `Dispose()` method signature issues
- Commented out `BoxShadow` property that may not be available in all Tizen versions
- **Updated resource paths**: Changed from relative paths to proper Tizen resource paths using `Application.Current.DirectoryInfo.Resource` and `System.IO.Path.Combine()` similar to SplashScreen.cs
