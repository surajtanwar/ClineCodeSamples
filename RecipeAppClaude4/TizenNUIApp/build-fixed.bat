@echo off
echo Building Tizen NUI Application (Fixed Version)
echo ==============================================
echo.

echo Step 1: Cleaning previous build artifacts...
if exist "bin" rmdir /s /q "bin"
if exist "obj" rmdir /s /q "obj"
echo Clean completed.
echo.

echo Step 2: Restoring NuGet packages...
dotnet restore TizenNUIApp.csproj --verbosity normal
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Package restore failed!
    echo This usually means:
    echo - Tizen.NET.Sdk is not available
    echo - Network connectivity issues
    echo - NuGet sources not configured properly
    goto :error
)
echo Package restore successful.
echo.

echo Step 3: Building for Tizen 7.0...
dotnet build TizenNUIApp.csproj --framework tizen7.0 --configuration Debug --verbosity normal --no-restore
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Tizen 7.0 build failed!
    goto :error
)
echo Tizen 7.0 build successful.
echo.

echo Step 4: Building for Tizen 10.0...
dotnet build TizenNUIApp.csproj --framework tizen10.0 --configuration Debug --verbosity normal --no-restore
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Tizen 10.0 build failed!
    goto :error
)
echo Tizen 10.0 build successful.
echo.

echo Step 5: Verifying build outputs...
if exist "bin\Debug\tizen7.0\TizenNUIApp.dll" (
    echo SUCCESS: Tizen 7.0 - TizenNUIApp.dll found
) else (
    echo WARNING: Tizen 7.0 - TizenNUIApp.dll not found
)

if exist "bin\Debug\tizen10.0\TizenNUIApp.dll" (
    echo SUCCESS: Tizen 10.0 - TizenNUIApp.dll found
) else (
    echo WARNING: Tizen 10.0 - TizenNUIApp.dll not found
)
echo.

echo Step 6: Creating TPK package (if Tizen CLI available)...
where tizen >nul 2>&1
if %ERRORLEVEL% EQU 0 (
    echo Tizen CLI found, attempting to create TPK...
    tizen build-app -t tpk -s tizen-distributor-signer -- .
    if %ERRORLEVEL% EQU 0 (
        echo TPK creation successful!
    ) else (
        echo TPK creation failed - this is normal if certificates are not configured
    )
) else (
    echo Tizen CLI not found - skipping TPK creation
    echo To create TPK: Install Tizen Studio and add to PATH
)
echo.

echo BUILD COMPLETED SUCCESSFULLY!
echo.
echo Next steps:
echo 1. If TPK creation failed, configure Tizen certificates
echo 2. Test the application on Tizen device or emulator
echo 3. Check bin\Debug\tizen7.0\ or bin\Debug\tizen10.0\ for output files
goto :end

:error
echo.
echo BUILD FAILED!
echo.
echo Troubleshooting steps:
echo 1. Install Tizen Studio from https://developer.tizen.org/development/tizen-studio/download
echo 2. Install Visual Studio Tizen extension or ensure Tizen.NET.Sdk is available
echo 3. Check internet connection for NuGet package downloads
echo 4. Run 'diagnose-build.bat' for detailed diagnostics
echo.

:end
pause
