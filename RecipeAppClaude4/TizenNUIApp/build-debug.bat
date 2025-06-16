@echo off
echo Building Tizen NUI Application (Debug Mode)...
echo.

echo Restoring NuGet packages...
dotnet restore TizenNUIApp.csproj
echo.

echo Building for Tizen 10.0...
dotnet build TizenNUIApp.csproj --framework tizen10.0 --configuration Debug --verbosity detailed
echo.

echo Building for Tizen 7.0...
dotnet build TizenNUIApp.csproj --framework tizen7.0 --configuration Debug --verbosity detailed
echo.

echo Checking build output...
if exist "bin\Debug\tizen10.0\TizenNUIApp.dll" (
    echo SUCCESS: Tizen 10.0 build completed - TizenNUIApp.dll found
) else (
    echo ERROR: Tizen 10.0 build failed - TizenNUIApp.dll not found
)

if exist "bin\Debug\tizen7.0\TizenNUIApp.dll" (
    echo SUCCESS: Tizen 7.0 build completed - TizenNUIApp.dll found
) else (
    echo ERROR: Tizen 7.0 build failed - TizenNUIApp.dll not found
)

echo.
echo Build process completed.
pause
