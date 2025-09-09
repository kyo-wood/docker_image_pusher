@echo off
REM Build script for Computer Vision Inference Platform
REM Supports both .NET Framework 4.8 and .NET 8

echo ========================================
echo Computer Vision Inference Platform
echo Build Script
echo ========================================
echo.

REM Check for .NET 8 SDK
dotnet --version >nul 2>&1
if %errorlevel% equ 0 (
    echo [INFO] .NET 8 SDK detected
    goto build_dotnet8
) else (
    echo [WARNING] .NET 8 SDK not found
    goto check_framework
)

:build_dotnet8
echo.
echo Building with .NET 8...
cd CVInferencePlatform
dotnet restore
if %errorlevel% neq 0 (
    echo [ERROR] Failed to restore packages
    goto error
)

dotnet build -c Release
if %errorlevel% neq 0 (
    echo [ERROR] Build failed
    goto error
)

echo [SUCCESS] Build completed successfully
echo.
echo Output location: CVInferencePlatform\bin\Release\net8.0\
echo.
echo To run the application:
echo   cd CVInferencePlatform
echo   dotnet run
echo   -- or --
echo   dotnet run -- --console
goto end

:check_framework
echo.
echo Checking for .NET Framework 4.8...
REM Check for MSBuild
where msbuild >nul 2>&1
if %errorlevel% equ 0 (
    echo [INFO] MSBuild detected
    goto build_framework
) else (
    echo [ERROR] Neither .NET 8 SDK nor MSBuild found
    echo Please install one of the following:
    echo   - .NET 8 SDK: https://dotnet.microsoft.com/download/dotnet/8.0
    echo   - Visual Studio with .NET Framework 4.8 support
    goto error
)

:build_framework
echo.
echo Building with .NET Framework 4.8...
msbuild CVInferencePlatform.sln /p:Configuration=Release /p:Platform="Any CPU"
if %errorlevel% neq 0 (
    echo [ERROR] Build failed
    goto error
)

echo [SUCCESS] Build completed successfully
echo.
echo Output location: CVInferencePlatform\bin\Release\
goto end

:error
echo.
echo [ERROR] Build process failed
echo Please check the error messages above
exit /b 1

:end
echo.
echo ========================================
echo Build process completed
echo ========================================
pause