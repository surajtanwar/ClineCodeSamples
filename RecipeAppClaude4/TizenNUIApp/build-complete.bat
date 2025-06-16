@echo off
echo ========================================
echo Tizen NUI Application Complete Build
echo ========================================
echo.

REM Check .NET SDK
echo Checking .NET SDK...
dotnet --version
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: .NET SDK not found. Please install .NET 6.0 or later.
    pause
    exit /b 1
)
echo .NET SDK found.
echo.

REM Check Tizen CLI (optional)
echo Checking Tizen CLI...
where tizen >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo WARNING: Tizen CLI not found. TPK creation will be skipped.
    echo Install Tizen Studio from: https://developer.tizen.org/development/tizen-studio/download
    set TIZEN_CLI_AVAILABLE=false
) else (
    echo Tizen CLI found.
    set TIZEN_CLI_AVAILABLE=true
)
echo.

REM Clean previous builds
echo Cleaning previous builds...
if exist "bin" rmdir /s /q "bin"
if exist "obj" rmdir /s /q "obj"
echo Clean completed.
echo.

REM Restore NuGet packages
echo Restoring NuGet packages...
dotnet restore TizenNUIApp.csproj --force --verbosity normal
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Package restore failed.
    echo This might indicate missing Tizen SDK or network issues.
    echo Please install Tizen Studio and ensure internet connectivity.
    pause
    exit /b 1
)
echo Package restore completed.
echo.

REM Build for Tizen 10.0
echo Building for Tizen 10.0...
dotnet build TizenNUIApp.csproj --framework tizen10.0 --configuration Debug --no-restore
set TIZEN10_BUILD_RESULT=%ERRORLEVEL%

REM Build for Tizen 7.0
echo Building for Tizen 7.0...
dotnet build TizenNUIApp.csproj --framework tizen7.0 --configuration Debug --no-restore
set TIZEN7_BUILD_RESULT=%ERRORLEVEL%

echo.
echo ========================================
echo Build Results Summary
echo ========================================

REM Check build results
if %TIZEN10_BUILD_RESULT% EQU 0 (
    echo [SUCCESS] Tizen 10.0 build completed
    if exist "bin\Debug\tizen10.0\TizenNUIApp.dll" (
        echo [VERIFIED] TizenNUIApp.dll found for Tizen 10.0
    ) else (
        echo [WARNING] TizenNUIApp.dll not found for Tizen 10.0
    )
) else (
    echo [FAILED] Tizen 10.0 build failed
)

if %TIZEN7_BUILD_RESULT% EQU 0 (
    echo [SUCCESS] Tizen 7.0 build completed
    if exist "bin\Debug\tizen7.0\TizenNUIApp.dll" (
        echo [VERIFIED] TizenNUIApp.dll found for Tizen 7.0
    ) else (
        echo [WARNING] TizenNUIApp.dll not found for Tizen 7.0
    )
) else (
    echo [FAILED] Tizen 7.0 build failed
)

echo.

REM Create TPK if Tizen CLI is available and builds succeeded
if "%TIZEN_CLI_AVAILABLE%"=="true" (
    if %TIZEN10_BUILD_RESULT% EQU 0 (
        echo Creating TPK package...
        tizen build-app -t tpk -s tizen-distributor-signer -- .
        if %ERRORLEVEL% EQU 0 (
            echo [SUCCESS] TPK package created successfully
        ) else (
            echo [WARNING] TPK creation failed - check certificate configuration
        )
    ) else (
        echo [SKIPPED] TPK creation skipped due to build failures
    )
) else (
    echo [SKIPPED] TPK creation skipped - Tizen CLI not available
)

echo.
echo ========================================
echo Build Process Completed
echo ========================================

if %TIZEN10_BUILD_RESULT% EQU 0 (
    if %TIZEN7_BUILD_RESULT% EQU 0 (
        echo Result: All builds successful!
        exit /b 0
    ) else (
        echo Result: Tizen 10.0 successful, Tizen 7.0 failed
        exit /b 1
    )
) else (
    if %TIZEN7_BUILD_RESULT% EQU 0 (
        echo Result: Tizen 7.0 successful, Tizen 10.0 failed
        exit /b 1
    ) else (
        echo Result: All builds failed
        exit /b 1
    )
)

pause
