# Computer Vision Inference Platform - Installation Guide

## System Requirements

### Minimum Requirements
- **Operating System**: Windows 7 SP1 or later (64-bit recommended)
- **Memory**: 4 GB RAM minimum, 8 GB recommended
- **Storage**: 2 GB available space
- **.NET Framework**: 4.8 or later / .NET 8+
- **Graphics**: DirectX 9.0c compatible

### Recommended for Optimal Performance
- **Operating System**: Windows 10/11 (64-bit)
- **Memory**: 16 GB RAM or more
- **GPU**: NVIDIA GPU with CUDA support (GTX 1060 or better)
- **Storage**: 10 GB available space (for models and results)
- **CPU**: Intel i5 8th gen or AMD Ryzen 5 2600 or better

## Installation Methods

### Method 1: Standalone Executable (Recommended)
1. Download `CVInferencePlatform-Setup.exe` from releases
2. Run the installer as Administrator
3. Follow the installation wizard
4. Launch from Start Menu or Desktop shortcut

### Method 2: Portable Version
1. Download `CVInferencePlatform-Portable.zip`
2. Extract to desired location
3. Run `CVInferencePlatform.exe`

### Method 3: Build from Source
```bash
# Clone repository
git clone https://github.com/your-repo/cv-inference-platform.git
cd cv-inference-platform

# Windows (with Visual Studio)
build.bat

# Windows/Linux/macOS (with .NET 8)
./build.sh
```

## First-Time Setup

### 1. CUDA Setup (Optional, for GPU acceleration)
- Download and install [NVIDIA CUDA Toolkit 11.8+](https://developer.nvidia.com/cuda-downloads)
- Download and install [cuDNN 8.6+](https://developer.nvidia.com/cudnn)
- Restart your computer after installation

### 2. Model Downloads
The platform supports various pre-trained models:

#### YOLO Models (Recommended)
```
# YOLOv8 models (Ultralytics)
- yolov8n.pt (6.2 MB) - Nano, fastest
- yolov8s.pt (21.5 MB) - Small, balanced
- yolov8m.pt (49.7 MB) - Medium, accurate
- yolov8l.pt (83.7 MB) - Large, very accurate
- yolov8x.pt (136.7 MB) - Extra large, most accurate

# Download from: https://github.com/ultralytics/ultralytics
```

#### ONNX Models
```
# Convert PyTorch to ONNX for better performance
yolo export model=yolov8n.pt format=onnx
```

### 3. Directory Structure Setup
Create the following folders in your installation directory:
```
CVInferencePlatform/
├── Models/           # Store your .pt, .onnx, .engine files
├── Input/           # Input images and videos
├── Output/          # Processed results
├── Logs/            # Application logs
└── Config/          # Configuration files
```

## Configuration

### Application Settings
Settings are stored in `Config/settings.json`:

```json
{
  "inference": {
    "confidenceThreshold": 0.5,
    "iouThreshold": 0.45,
    "device": "Auto",
    "batchSize": 1,
    "maxDetections": 100
  },
  "application": {
    "logLevel": "Info",
    "autoSaveResults": false,
    "defaultOutputPath": "Output",
    "performanceMonitoring": true
  }
}
```

### Model Configuration
Saved models are stored in `Config/saved_models.json`:

```json
{
  "savedModels": {
    "YOLOv8n": {
      "path": "Models/yolov8n.pt",
      "description": "Fast detection model",
      "lastUsed": "2024-01-09T10:30:00Z"
    }
  }
}
```

## Usage Guide

### Basic Workflow
1. **Launch Application**
   - Double-click desktop icon or start from menu
   - Wait for initialization (displays available devices)

2. **Load a Model**
   - Click "Load Model" or use Ctrl+L
   - Browse to your model file (.pt, .onnx, .engine)
   - Wait for model validation and loading

3. **Configure Settings**
   - Adjust confidence threshold (0.1 - 1.0)
   - Set IoU threshold (0.1 - 1.0)
   - Choose device (Auto/GPU/CPU)

4. **Select Input**
   - Choose input type: Image File, Video File, or Camera
   - Browse and select your input source

5. **Run Inference**
   - Click "Start Inference" or press F5
   - View real-time results in the preview window
   - Monitor performance metrics in status bar

6. **Save Results**
   - Click "Save Results" or press F12
   - Choose output format and location

### Keyboard Shortcuts
- **F5**: Start/Resume inference
- **F6**: Pause inference
- **F12**: Save current results
- **Ctrl+L**: Load model
- **Ctrl+O**: Open input file
- **Ctrl+S**: Save configuration
- **Ctrl+Q**: Quit application
- **F1**: Show help

### Batch Processing
1. Select "Folder" as input source
2. Choose folder containing images/videos
3. Configure output settings
4. Start batch processing
5. Monitor progress in log window

## Troubleshooting

### Common Issues

#### "Model Loading Failed"
**Symptoms**: Error dialog when loading model
**Solutions**:
- Verify file format is supported (.pt, .onnx, .engine)
- Check file is not corrupted
- Ensure sufficient free memory (>2GB)
- Try a smaller model first

#### "CUDA Not Available"
**Symptoms**: Only CPU option in device dropdown
**Solutions**:
- Install NVIDIA graphics drivers
- Install CUDA Toolkit 11.8+
- Verify GPU compatibility
- Restart application after driver installation

#### "Out of Memory Error"
**Symptoms**: Application crashes during inference
**Solutions**:
- Close other applications
- Use smaller model (e.g., YOLOv8n instead of YOLOv8x)
- Reduce input image resolution
- Switch to CPU mode temporarily

#### "Slow Performance"
**Symptoms**: Low FPS, long inference times
**Solutions**:
- Enable GPU acceleration
- Use ONNX or TensorRT formats
- Reduce input resolution
- Close unnecessary background applications
- Check thermal throttling

#### "UI Freezing"
**Symptoms**: Interface becomes unresponsive
**Solutions**:
- Wait for current operation to complete
- Use Task Manager to check CPU/memory usage
- Restart application if necessary
- Check Windows Event Viewer for errors

### Performance Optimization

#### GPU Optimization
- Use latest NVIDIA drivers
- Enable GPU scheduling in Windows settings
- Monitor GPU memory usage
- Use appropriate batch sizes

#### Model Optimization
- Convert PyTorch models to ONNX for better performance
- Use TensorRT engines for NVIDIA GPUs
- Choose appropriate model size for your use case

#### System Optimization
- Close unnecessary applications
- Disable Windows visual effects
- Set application to "High Priority" in Task Manager
- Ensure adequate cooling for sustained performance

## Support and Assistance

### Getting Help
- **Documentation**: Check the included PDF manual
- **Video Tutorials**: Available on our YouTube channel
- **Community Forum**: Post questions and share tips
- **Email Support**: support@cv-inference-platform.com

### Reporting Issues
When reporting bugs, please include:
- Windows version and build number
- Application version
- Model file details
- Steps to reproduce the issue
- Error messages or logs
- System specifications

### Log Files
Log files are automatically created in the `Logs/` folder:
- `application.log`: General application logs
- `inference.log`: Detailed inference operations
- `performance.log`: Performance metrics and benchmarks

Send these files when requesting support for faster diagnosis.

## Updates and Maintenance

### Automatic Updates
- Enable automatic update checking in settings
- Application will notify when updates are available
- Download and install updates for latest features and bug fixes

### Manual Updates
1. Download latest version from website
2. Close current application
3. Run new installer (will preserve settings)
4. Restart application

### Backup and Restore
Important files to backup:
- `Config/settings.json` - Your preferences
- `Config/saved_models.json` - Saved model list
- `Models/` folder - Your model files

## Advanced Configuration

### Command Line Options
```cmd
CVInferencePlatform.exe [options]
  --config <path>     Use custom configuration file
  --model <path>      Auto-load model on startup
  --input <path>      Set default input path
  --gpu <id>          Force specific GPU device
  --log-level <level> Set logging level (Debug/Info/Warning/Error)
  --batch             Enable batch processing mode
  --no-gui            Run in console mode
```

### Environment Variables
```cmd
# CUDA configuration
set CUDA_VISIBLE_DEVICES=0
set CUDA_DEVICE_ORDER=PCI_BUS_ID

# Application settings
set CV_PLATFORM_CONFIG=C:\Custom\config.json
set CV_PLATFORM_MODELS=C:\Models
set CV_PLATFORM_LOG_LEVEL=Debug
```

This comprehensive installation guide ensures users can successfully set up and use the Computer Vision Inference Platform in various configurations and environments.