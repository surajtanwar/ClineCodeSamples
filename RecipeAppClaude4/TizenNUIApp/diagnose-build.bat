@echo off
echo Tizen NUI Application Build Diagnostics
echo ========================================
echo.

echo Checking .NET SDK...
dotnet --version
echo.

echo Checking available SDKs...
dotnet --list-sdks
echo.

echo Checking NuGet sources...
dotnet nuget list source
echo.

echo Attempting to restore packages...
dotnet restore TizenNUIApp.csproj --verbosity detailed
echo.

echo Attempting to build (with detailed output)...
dotnet build TizenNUIApp.csproj --verbosity detailed
echo.

echo Checking if Tizen CLI is available...
where tizen
if %ERRORLEVEL% NEQ 0 (
    echo WARNING: Tizen CLI not found in PATH
    echo Please install Tizen Studio and add Tizen CLI to PATH
) else (
    echo Tizen CLI found
    tizen version
)
echo.

echo Build diagnostics completed.
echo.
echo If build failed, common issues:
echo 1. Tizen Studio not installed
echo 2. Tizen.NET.Sdk not available
echo 3. Missing certificates for TPK signing
echo 4. Incorrect API version configuration
echo.
pause
