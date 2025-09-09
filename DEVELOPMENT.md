# Development Documentation

## Environment Configuration

### Prerequisites for Development

#### Windows Development Environment
- **Visual Studio 2019/2022**: Community Edition or higher
- **.NET Framework 4.8 SDK**: For WinForms development
- **.NET 8 SDK**: For modern cross-platform development
- **Git**: Version control
- **NVIDIA CUDA Toolkit** (Optional): For GPU acceleration testing

#### Alternative Development (Cross-platform)
- **Visual Studio Code**: With C# extension
- **.NET 8 SDK**: For core functionality
- **Mono** (Linux/macOS): For .NET Framework compatibility

### Environment Setup

#### Windows Setup
```powershell
# Install .NET Framework 4.8 Developer Pack
# Download from: https://dotnet.microsoft.com/download/dotnet-framework/net48

# Install .NET 8 SDK
winget install Microsoft.DotNet.SDK.8

# Install Visual Studio with workloads
# - .NET desktop development
# - Windows Forms App (.NET Framework)

# Clone repository
git clone https://github.com/your-repo/cv-inference-platform.git
cd cv-inference-platform
```

#### Cross-platform Setup
```bash
# Install .NET 8 SDK
# Ubuntu/Debian
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0

# Clone and build
git clone https://github.com/your-repo/cv-inference-platform.git
cd cv-inference-platform/CVInferencePlatform
dotnet build
```

## Module Descriptions

### Core Module (`Core/`)

#### InferencePlatform.cs
**Purpose**: Central coordinator for the entire platform
- Manages component lifecycle
- Coordinates between model management and inference
- Provides unified API for UI interactions
- Handles error propagation and logging

```csharp
public class InferencePlatform
{
    // Key Methods:
    public void Initialize()                    // Platform initialization
    public bool LoadModel(string modelPath)    // Model loading
    public InferenceResult RunInference(...)   // Execute inference
    public void ShowStatus()                   // Status reporting
}
```

#### ModelManager.cs
**Purpose**: Model loading, validation, and metadata management
- Supports multiple model formats (.pt, .onnx, .engine)
- Validates model integrity and compatibility
- Extracts and manages model metadata
- Maintains saved models library

```csharp
public class ModelManager
{
    // Key Methods:
    public ModelInfo LoadModel(string path)           // Load and validate model
    public void SaveModelToList(string name, ...)     // Save to favorites
    public void ValidateModel(ModelInfo model)        // Model validation
}
```

#### InferenceEngine.cs
**Purpose**: Core inference execution and device management
- Multi-device support (CPU, CUDA)
- Inference optimization and batching
- Performance monitoring
- Device capability detection

```csharp
public class InferenceEngine
{
    // Key Methods:
    public InferenceResult RunInference(...)      // Execute inference
    public void DetectAvailableDevices()          // Device detection
    public void WarmupModel(ModelInfo model)      // Performance optimization
}
```

#### ConfigurationManager.cs
**Purpose**: Settings and preferences management
- Inference parameter configuration
- Application settings persistence
- Default value management
- Configuration validation

### Models Module (`Models/`)

#### DataModels.cs
**Purpose**: Core data structures and type definitions

**ModelInfo Class**:
```csharp
public class ModelInfo
{
    public string Name { get; set; }           // Model display name
    public string Path { get; set; }           // File system path
    public string Format { get; set; }         // Model format (.pt, .onnx, etc.)
    public string InputSize { get; set; }      // Expected input dimensions
    public int NumClasses { get; set; }        // Number of output classes
    public ModelType Type { get; set; }        // Model type (detection, classification)
}
```

**InferenceResult Class**:
```csharp
public class InferenceResult
{
    public List<Detection> Detections { get; set; }    // Detected objects
    public int InferenceTime { get; set; }             // Processing time (ms)
    public string ModelUsed { get; set; }              // Model identifier
    public DateTime ProcessedAt { get; set; }          // Timestamp
}
```

**Detection Class**:
```csharp
public class Detection
{
    public string ClassName { get; set; }              // Object class name
    public double Confidence { get; set; }             // Detection confidence
    public BoundingBox BoundingBox { get; set; }       // Object location
    public int ClassId { get; set; }                   // Class identifier
}
```

### UI Module (`UI/`)

#### MainFormLogic.cs
**Purpose**: UI controller and event management
- Handles all UI interactions
- Manages form state and updates
- Implements keyboard shortcuts
- Coordinates with core platform

**Key Responsibilities**:
- Model management UI operations
- Inference control and monitoring  
- Performance metrics display
- User input validation
- Error message presentation

### Utils Module (`Utils/`)

#### Utilities.cs
**Purpose**: Helper functions and common operations

**ImageProcessor Class**:
- Image loading and preprocessing
- Result visualization and annotation
- Image format conversion
- Drawing operations for bounding boxes

**FileUtils Class**:
- File format validation
- Path management
- Batch file operations
- Output directory management

**Logger Class**:
- Structured logging with timestamps
- Multiple output targets (console, file)
- Log level management
- Error reporting

**PerformanceMonitor Class**:
- System resource monitoring
- Performance metrics calculation
- Memory usage tracking
- FPS calculation

## Interface Documentation

### Core Platform Interface

#### Loading Models
```csharp
// Basic model loading
var platform = new InferencePlatform();
platform.Initialize();
bool success = platform.LoadModel("path/to/model.pt");

// Check model information
if (platform.CurrentModel != null)
{
    var model = platform.CurrentModel;
    Console.WriteLine($"Model: {model.Name}");
    Console.WriteLine($"Format: {model.Format}");
    Console.WriteLine($"Input Size: {model.InputSize}");
    Console.WriteLine($"Classes: {model.NumClasses}");
}
```

#### Configuring Inference
```csharp
// Create inference configuration
var config = new InferenceConfig
{
    ConfidenceThreshold = 0.6,     // Detection confidence threshold
    IouThreshold = 0.4,            // Non-maximum suppression threshold
    Device = "CUDA:0",             // Inference device
    BatchSize = 1,                 // Batch processing size
    MaxDetections = 100,           // Maximum detections per image
    EnableNMS = true               // Enable non-maximum suppression
};
```

#### Running Inference
```csharp
// Single image inference
var result = platform.RunInference("image.jpg", config);

// Process results
if (result != null)
{
    Console.WriteLine($"Inference Time: {result.InferenceTime}ms");
    Console.WriteLine($"Detections: {result.Detections.Count}");
    
    foreach (var detection in result.Detections)
    {
        Console.WriteLine($"  {detection.ClassName}: {detection.Confidence:F3} " +
                         $"at ({detection.BoundingBox.X}, {detection.BoundingBox.Y})");
    }
}
```

### UI Integration Interface

#### MainFormLogic Usage
```csharp
// Initialize UI controller
var mainForm = new MainFormLogic();
mainForm.Initialize();

// Load model through UI
bool success = mainForm.LoadModel("model.onnx");

// Update configuration
mainForm.UpdateInferenceConfig(
    confidence: 0.7,
    iou: 0.5,
    device: "Auto"
);

// Run inference
var result = mainForm.RunInference("input.jpg");

// Handle keyboard shortcuts
mainForm.HandleKeyPress("F5");  // Start inference
mainForm.HandleKeyPress("F12"); // Save results
```

### Utility Interfaces

#### Image Processing
```csharp
// Load and resize image
var image = ImageProcessor.LoadImage("input.jpg");
var resized = ImageProcessor.ResizeImage(image, 640, 640);

// Draw detection results
var annotated = ImageProcessor.DrawDetections(image, inferenceResult);

// Save results
ImageProcessor.SaveImage(annotated, "output.jpg");
```

#### File Management
```csharp
// Validate file types
bool isImage = FileUtils.IsImageFile("photo.jpg");     // true
bool isVideo = FileUtils.IsVideoFile("video.mp4");     // true
bool isModel = FileUtils.IsModelFile("model.onnx");    // true

// Generate output paths
string outputPath = FileUtils.GenerateOutputPath("input.jpg", "result");
// Result: "result_20240109_143052.jpg"
```

#### Logging
```csharp
// Configure logging
Logger.SetLogFile("application.log");

// Log messages
Logger.Log("Application started", LogLevel.Info);
Logger.LogError("Model loading failed", exception);

// Clear log
Logger.ClearLog();
```

### Extension Points

#### Custom Model Support
```csharp
// Extend ModelManager for new formats
public class CustomModelManager : ModelManager
{
    protected override ModelInfo LoadCustomFormat(string path)
    {
        // Implement custom model loading logic
        return new ModelInfo { /* ... */ };
    }
}
```

#### Custom Inference Backend
```csharp
// Extend InferenceEngine for new backends
public class CustomInferenceEngine : InferenceEngine
{
    protected override InferenceResult RunCustomInference(
        ModelInfo model, 
        string input, 
        InferenceConfig config)
    {
        // Implement custom inference logic
        return new InferenceResult { /* ... */ };
    }
}
```

### Error Handling

#### Exception Hierarchy
```csharp
// Model-related exceptions
public class ModelLoadException : Exception { }
public class ModelValidationException : Exception { }

// Inference-related exceptions  
public class InferenceException : Exception { }
public class DeviceException : Exception { }

// UI-related exceptions
public class UIException : Exception { }
```

#### Error Recovery
```csharp
try
{
    platform.LoadModel("model.pt");
}
catch (ModelLoadException ex)
{
    Logger.LogError("Model loading failed", ex);
    // Show user-friendly error message
    // Attempt fallback or recovery
}
catch (Exception ex)
{
    Logger.LogError("Unexpected error", ex);
    // Handle gracefully, ensure app stability
}
```

### Performance Considerations

#### Memory Management
- Dispose of images after processing
- Unload models when not needed
- Monitor memory usage with PerformanceMonitor
- Implement proper cleanup in Dispose methods

#### Threading
- Use async/await for long-running operations
- Implement cancellation tokens for user-initiated stops
- Update UI on main thread only
- Background processing for batch operations

#### GPU Optimization
- Warm up models after loading
- Batch multiple inputs when possible
- Monitor GPU memory usage
- Implement fallback to CPU when needed

This documentation provides a comprehensive guide for developers working with the Computer Vision Inference Platform, covering all major components, interfaces, and usage patterns.