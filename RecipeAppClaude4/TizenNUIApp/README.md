# Tizen NUI Application

A Tizen NUI application for mobile devices using API version 7.0, optimized for 720x1280 resolution.

## Project Structure

```
TizenNUIApp/
├── TizenNUIApp.csproj          # Project file with Tizen.NET.Sdk
├── tizen-manifest.xml          # Tizen application manifest
├── TizenNUIApp.Package.targets # Package build targets
├── Program.cs                  # Main application entry point
├── shared/
│   └── res/
│       └── README.txt          # App icon placeholder
├── res/
│   └── images/
│       └── splash/
│           └── README.txt      # Splash images placeholder
└── README.md                   # This file
```

## Features

- **NUI Framework**: Uses Tizen NUI for native UI components
- **Mobile Optimized**: Designed for 720x1280 resolution
- **API Version 7.0**: Compatible with Tizen API version 7.0
- **Interactive UI**: Includes buttons, text labels, and colored views
- **Linear Layout**: Responsive layout management

## Prerequisites

1. **Tizen Studio** or **Visual Studio** with Tizen extension
2. **Tizen.NET.Sdk** version 1.1.9 or higher
3. **Tizen SDK** with API level 7.0
4. **Tizen device** or **emulator** for testing

## Setup Instructions

### 1. Install Required Images

Replace the placeholder files with actual PNG images:

- `shared/res/TizenNUIApp.png` - App icon (117x117 pixels)
- `res/images/splash/Rectangle.png` - Splash screen image
- `res/images/splash/Group.png` - Splash screen image
- `res/images/splash/Group_2.png` - Splash screen image

### 2. Build the Application

Using Tizen Studio:
```bash
tizen build-app -t tpk -s certificate_name -- TizenNUIApp
```

Using .NET CLI (if Tizen.NET.Sdk is configured):
```bash
dotnet build TizenNUIApp.csproj
```

### 3. Generate TPK File

The project is configured to automatically generate a .tpk file during build:
- The `TizenCreateTpkOnBuild` property is set to `true`
- Output will be in the `bin` directory

### 4. Install on Device

```bash
tizen install -n TizenNUIApp.tpk -t device_name
```

## Application Features

- **Title Display**: Shows "Tizen NUI Application"
- **Resolution Info**: Displays current resolution (720x1280)
- **Interactive Button**: Click to change title and button color
- **Colored Rectangle**: Visual element with rounded corners
- **Linear Layout**: Vertically centered layout with proper spacing

## Configuration Details

- **Target Frameworks**: tizen10.0, tizen7.0
- **API Version**: 7.0 (specified in manifest)
- **Package ID**: org.tizen.example.TizenNUIApp
- **Window Size**: 720x1280 pixels
- **Launch Mode**: Single instance

## Troubleshooting

1. **Build Errors**: Ensure Tizen.NET.Sdk is properly installed
2. **Missing Images**: Replace placeholder README files with actual PNG images
3. **Certificate Issues**: Configure proper signing certificate in Tizen Studio
4. **Device Connection**: Verify device is connected and in developer mode

## Development Notes

- The application uses NUI (Natural User Interface) framework
- Layout is optimized for mobile portrait orientation
- All UI elements are sized relative to the 720x1280 resolution
- The app includes basic interaction handling and state management

## Next Steps

1. Add actual image resources
2. Implement additional UI screens
3. Add application-specific functionality
4. Test on target Tizen devices
5. Optimize performance and memory usage
