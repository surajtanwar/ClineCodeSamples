# Tizen NUI Application - Setup and Troubleshooting Guide

## Quick Start

1. **Run Diagnostics First**:
   ```batch
   diagnose-build.bat
   ```

2. **Build the Application**:
   ```batch
   build-fixed.bat
   ```

## Prerequisites Installation

### 1. Install .NET SDK
- Download and install .NET 6.0 or later from https://dotnet.microsoft.com/download
- Verify installation: `dotnet --version`

### 2. Install Tizen Studio
- Download from: https://developer.tizen.org/development/tizen-studio/download
- Install with the following components:
  - Tizen SDK Tools
  - Tizen Native CLI
  - Certificate Manager
  - Package Manager

### 3. Configure Environment
- Add Tizen CLI to system PATH:
  - Default location: `C:\tizen-studio\tools\ide\bin`
- Verify: `tizen version`

### 4. Install Tizen.NET.Sdk
Option A - Via Visual Studio:
- Install "Visual Studio Tools for Tizen" extension

Option B - Via dotnet CLI:
```batch
dotnet new -i Tizen.ProjectTemplates
```

## Build Process

### Method 1: Using Fixed Build Script (Recommended)
```batch
build-fixed.bat
```

### Method 2: Manual Build Steps
```batch
# Clean previous builds
dotnet clean TizenNUIApp.csproj

# Restore packages
dotnet restore TizenNUIApp.csproj

# Build for Tizen 7.0
dotnet build TizenNUIApp.csproj --framework tizen7.0 --configuration Debug

# Build for Tizen 10.0
dotnet build TizenNUIApp.csproj --framework tizen10.0 --configuration Debug
```

### Method 3: Create TPK Package
```batch
# After successful build
tizen build-app -t tpk -s tizen-distributor-signer -- .
```

## Common Issues and Solutions

### Issue 1: "Tizen.NET.Sdk not found"
**Symptoms**: Package restore fails, SDK not recognized
**Solutions**:
1. Install Tizen Studio completely
2. Install Visual Studio Tizen extension
3. Add Tizen NuGet source manually:
   ```batch
   dotnet nuget add source https://tizen.myget.org/F/dotnet/api/v3/index.json -n tizen
   ```

### Issue 2: "Build succeeded but no output files"
**Symptoms**: Build reports success but bin/Debug folders are empty
**Solutions**:
1. Check if Tizen SDK is properly installed
2. Verify target frameworks are supported
3. Try building single framework:
   ```batch
   dotnet build TizenNUIApp.csproj --framework tizen7.0
   ```

### Issue 3: "TPK creation failed"
**Symptoms**: DLL builds successfully but TPK creation fails
**Solutions**:
1. Install Tizen certificate:
   - Open Tizen Studio → Tools → Certificate Manager
   - Create new certificate profile
2. Use correct signer profile:
   ```batch
   tizen build-app -t tpk -s [your-certificate-profile] -- .
   ```

### Issue 4: "Command 'tizen' not found"
**Symptoms**: Tizen CLI commands don't work
**Solutions**:
1. Add to PATH: `C:\tizen-studio\tools\ide\bin`
2. Restart command prompt/IDE
3. Verify: `where tizen`

### Issue 5: Network/Proxy Issues
**Symptoms**: Package restore fails with network errors
**Solutions**:
1. Configure proxy in NuGet.config
2. Check firewall settings
3. Try different NuGet sources

## Project Structure Explained

```
TizenNUIApp/
├── Program.cs                    # Main application code
├── TizenNUIApp.csproj           # Project configuration
├── TizenNUIApp.Standalone.csproj # Windows Forms version
├── tizen-manifest.xml           # App manifest
├── NuGet.config                 # Package sources
├── global.json                  # SDK version
├── build-fixed.bat              # Improved build script
├── diagnose-build.bat           # Diagnostic tool
├── res/                         # Resources
│   └── images/splash/
│       └── Rectangle.png
└── shared/res/                  # Shared resources
    └── TizenNUIApp.png
```

## Verification Steps

After successful build, verify:

1. **Check Output Files**:
   ```
   bin/Debug/tizen7.0/TizenNUIApp.dll
   bin/Debug/tizen10.0/TizenNUIApp.dll
   ```

2. **Test on Emulator**:
   - Start Tizen emulator
   - Install TPK: `tizen install -n TizenNUIApp.tpk -t [device-id]`
   - Launch app

3. **Test on Device**:
   - Enable developer mode on device
   - Connect via SDB
   - Install and test

## Advanced Configuration

### Custom Build Configuration
Edit `TizenNUIApp.csproj` to modify:
- Target frameworks
- API versions
- Build properties
- Resource inclusion

### Certificate Management
```batch
# List certificates
tizen security-profiles list

# Create new certificate
tizen certificate -a MyApp -p MyPassword -c US -s State -ct City -o Organization -n MyName -e my@email.com

# Create distributor certificate
tizen certificate -a MyApp -f MyApp -p MyPassword
```

## Getting Help

1. **Run Diagnostics**: `diagnose-build.bat`
2. **Check Logs**: Look in `obj/` directory for detailed logs
3. **Tizen Documentation**: https://docs.tizen.org/
4. **Community Forums**: https://developer.tizen.org/forums

## Success Indicators

✅ Package restore completes without errors
✅ Build produces DLL files in bin/Debug/
✅ TPK file created (if certificates configured)
✅ Application runs on emulator/device

If you see all these indicators, your build is successful!
