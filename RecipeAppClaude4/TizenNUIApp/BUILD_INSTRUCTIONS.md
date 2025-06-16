# Tizen NUI Application Build Instructions

## Prerequisites

1. **Tizen Studio**: Download and install from https://developer.tizen.org/development/tizen-studio/download
2. **.NET SDK**: Install .NET 6.0 or later
3. **Tizen CLI**: Ensure Tizen CLI is added to your system PATH

## Build Issues and Solutions

### Issue 1: Missing Tizen SDK
If you get errors about missing Tizen.NET.Sdk, install Tizen Studio first.

### Issue 2: Missing Image Files
The original project referenced missing image files. This has been fixed by removing references to non-existent files.

### Issue 3: Build Configuration
The project has been updated with proper build configurations for both Tizen 7.0 and 10.0.

## Build Steps

### Option 1: Using the Build Script
```batch
build-debug.bat
```

### Option 2: Manual Build
```batch
# Restore packages
dotnet restore TizenNUIApp.csproj

# Build for specific framework
dotnet build TizenNUIApp.csproj --framework tizen10.0 --configuration Debug
dotnet build TizenNUIApp.csproj --framework tizen7.0 --configuration Debug

# Create TPK package (requires Tizen CLI)
tizen build-app -t tpk -s tizen-distributor-signer -- .
```

### Option 3: Using Original Build Script
```batch
build.bat
```

## Project Structure
- `Program.cs` - Main application code
- `TizenNUIApp.csproj` - Project configuration
- `tizen-manifest.xml` - Tizen application manifest
- `res/` - Resource files (images, etc.)
- `shared/res/` - Shared resources

## Troubleshooting

1. **Tizen CLI not found**: Install Tizen Studio and add to PATH
2. **Build fails**: Check if all required SDKs are installed
3. **Missing certificates**: Configure Tizen certificate for TPK creation
4. **Package restore fails**: Check internet connection and NuGet sources

## Files Modified
- Removed references to missing image files (Group.png, Group_2.png)
- Added build configuration properties
- Created NuGet.config for Tizen package sources
- Added global.json for SDK version specification
