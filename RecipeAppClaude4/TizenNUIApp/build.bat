@echo off
echo Building Tizen NUI Application...
echo.

REM Check if Tizen CLI is available
where tizen >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Tizen CLI not found. Please install Tizen Studio and add it to PATH.
    echo Download from: https://developer.tizen.org/development/tizen-studio/download
    pause
    exit /b 1
)

REM Clean previous builds
if exist "bin" rmdir /s /q "bin"
if exist "obj" rmdir /s /q "obj"

echo Cleaning completed.
echo.

REM Build the project
echo Building project...
dotnet build TizenNUIApp.csproj --configuration Release

if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Build failed. Please check the error messages above.
    pause
    exit /b 1
)

echo Build completed successfully.
echo.

REM Create TPK package
echo Creating TPK package...
tizen build-app -t tpk -s tizen-distributor-signer -- .

if %ERRORLEVEL% NEQ 0 (
    echo ERROR: TPK creation failed. Please ensure:
    echo 1. Tizen certificate is properly configured
    echo 2. All required image files are present
    echo 3. Tizen SDK is properly installed
    pause
    exit /b 1
)

echo.
echo SUCCESS: TPK file has been created!
echo Check the 'bin' directory for the generated .tpk file.
echo.
pause
