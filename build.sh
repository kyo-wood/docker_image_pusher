#!/bin/bash

# Build script for Computer Vision Inference Platform (Linux/macOS)
# Supports .NET 8

echo "========================================"
echo "Computer Vision Inference Platform"
echo "Build Script (Linux/macOS)"
echo "========================================"
echo ""

# Check for .NET 8 SDK
if command -v dotnet &> /dev/null; then
    echo "[INFO] .NET 8 SDK detected: $(dotnet --version)"
else
    echo "[ERROR] .NET 8 SDK not found"
    echo "Please install .NET 8 SDK from: https://dotnet.microsoft.com/download/dotnet/8.0"
    exit 1
fi

echo ""
echo "Building with .NET 8..."
cd CVInferencePlatform

# Restore packages
echo "Restoring packages..."
dotnet restore
if [ $? -ne 0 ]; then
    echo "[ERROR] Failed to restore packages"
    exit 1
fi

# Build project
echo "Building project..."
dotnet build -c Release
if [ $? -ne 0 ]; then
    echo "[ERROR] Build failed"
    exit 1
fi

echo "[SUCCESS] Build completed successfully"
echo ""
echo "Output location: CVInferencePlatform/bin/Release/net8.0/"
echo ""
echo "To run the application:"
echo "  cd CVInferencePlatform"
echo "  dotnet run"
echo "  -- or --"
echo "  dotnet run -- --console"
echo ""
echo "========================================"
echo "Build process completed"
echo "========================================"