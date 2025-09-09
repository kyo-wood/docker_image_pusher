using System;
using System.Collections.Generic;

namespace CVInferencePlatform.Models
{
    /// <summary>
    /// Input source types for the platform
    /// </summary>
    public enum InputSourceType
    {
        ImageFile,
        VideoFile,
        Camera,
        Folder,
        Stream
    }

    /// <summary>
    /// Model information and metadata
    /// </summary>
    public class ModelInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public string InputSize { get; set; } = string.Empty;
        public int NumClasses { get; set; }
        public DateTime LoadedAt { get; set; }
        public string Description { get; set; } = string.Empty;
        public ModelType Type { get; set; } = ModelType.ObjectDetection;
    }

    /// <summary>
    /// Inference result containing detections and metadata
    /// </summary>
    public class InferenceResult
    {
        public string InputPath { get; set; } = string.Empty;
        public string ModelUsed { get; set; } = string.Empty;
        public List<Detection> Detections { get; set; } = new();
        public int InferenceTime { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string OutputPath { get; set; } = string.Empty;
    }

    /// <summary>
    /// Individual detection result
    /// </summary>
    public class Detection
    {
        public string ClassName { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public BoundingBox BoundingBox { get; set; } = new();
        public int ClassId { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; } = new();
    }

    /// <summary>
    /// Bounding box coordinates
    /// </summary>
    public class BoundingBox
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int Right => X + Width;
        public int Bottom => Y + Height;
        public int CenterX => X + Width / 2;
        public int CenterY => Y + Height / 2;
        public int Area => Width * Height;

        public override string ToString()
        {
            return $"({X}, {Y}, {Width}, {Height})";
        }
    }

    /// <summary>
    /// Supported model types
    /// </summary>
    public enum ModelType
    {
        ObjectDetection,
        ImageClassification,
        SemanticSegmentation,
        InstanceSegmentation,
        KeypointDetection
    }

    /// <summary>
    /// Performance metrics for monitoring
    /// </summary>
    public class PerformanceMetrics
    {
        public double AverageInferenceTime { get; set; }
        public double CurrentFPS { get; set; }
        public int TotalInferences { get; set; }
        public DateTime LastUpdated { get; set; }
        public double MemoryUsage { get; set; }
        public double GPUUtilization { get; set; }
    }

    /// <summary>
    /// Input source information
    /// </summary>
    public class InputSource
    {
        public string Path { get; set; } = string.Empty;
        public InputSourceType Type { get; set; }
        public bool IsActive { get; set; }
        public Dictionary<string, object> Properties { get; set; } = new();
    }

    /// <summary>
    /// Processing pipeline stage
    /// </summary>
    public class ProcessingStage
    {
        public string Name { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public TimeSpan Duration { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
    }
}