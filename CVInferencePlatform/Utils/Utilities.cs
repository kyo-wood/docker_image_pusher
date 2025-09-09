using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using CVInferencePlatform.Models;

namespace CVInferencePlatform.Utils
{
    /// <summary>
    /// Image processing utilities for the computer vision platform
    /// </summary>
    public static class ImageProcessor
    {
        /// <summary>
        /// Load and preprocess image for inference
        /// </summary>
        public static Image LoadImage(string imagePath)
        {
            if (!File.Exists(imagePath))
                throw new FileNotFoundException($"Image file not found: {imagePath}");

            return Image.FromFile(imagePath);
        }

        /// <summary>
        /// Resize image to target dimensions
        /// </summary>
        public static Image ResizeImage(Image image, int width, int height)
        {
            var bitmap = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return bitmap;
        }

        /// <summary>
        /// Draw detection results on image
        /// </summary>
        public static Image DrawDetections(Image originalImage, InferenceResult result)
        {
            var bitmap = new Bitmap(originalImage);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var font = new Font("Arial", 12, FontStyle.Bold);
                var textBrush = new SolidBrush(Color.Red);
                var boxPen = new Pen(Color.Red, 3);

                foreach (var detection in result.Detections)
                {
                    var box = detection.BoundingBox;
                    
                    // Draw bounding box
                    graphics.DrawRectangle(boxPen, box.X, box.Y, box.Width, box.Height);
                    
                    // Draw label with confidence
                    var label = $"{detection.ClassName} ({detection.Confidence:F2})";
                    var labelSize = graphics.MeasureString(label, font);
                    var labelRect = new RectangleF(box.X, box.Y - labelSize.Height - 2, 
                                                  labelSize.Width + 4, labelSize.Height + 2);
                    
                    graphics.FillRectangle(textBrush, labelRect);
                    graphics.DrawString(label, font, Brushes.White, box.X + 2, box.Y - labelSize.Height);
                }

                font.Dispose();
                textBrush.Dispose();
                boxPen.Dispose();
            }

            return bitmap;
        }

        /// <summary>
        /// Save image with results
        /// </summary>
        public static void SaveImage(Image image, string outputPath)
        {
            var directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var format = GetImageFormat(outputPath);
            image.Save(outputPath, format);
        }

        /// <summary>
        /// Get image format based on file extension
        /// </summary>
        private static ImageFormat GetImageFormat(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" => ImageFormat.Jpeg,
                ".png" => ImageFormat.Png,
                ".bmp" => ImageFormat.Bmp,
                ".gif" => ImageFormat.Gif,
                ".tiff" => ImageFormat.Tiff,
                _ => ImageFormat.Png
            };
        }
    }

    /// <summary>
    /// File system utilities
    /// </summary>
    public static class FileUtils
    {
        public static readonly string[] SupportedImageExtensions = 
        {
            ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff"
        };

        public static readonly string[] SupportedVideoExtensions = 
        {
            ".mp4", ".avi", ".mov", ".mkv", ".wmv", ".flv"
        };

        public static readonly string[] SupportedModelExtensions = 
        {
            ".pt", ".onnx", ".engine"
        };

        /// <summary>
        /// Check if file is a supported image format
        /// </summary>
        public static bool IsImageFile(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            return Array.Exists(SupportedImageExtensions, ext => ext == extension);
        }

        /// <summary>
        /// Check if file is a supported video format
        /// </summary>
        public static bool IsVideoFile(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            return Array.Exists(SupportedVideoExtensions, ext => ext == extension);
        }

        /// <summary>
        /// Check if file is a supported model format
        /// </summary>
        public static bool IsModelFile(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            return Array.Exists(SupportedModelExtensions, ext => ext == extension);
        }

        /// <summary>
        /// Generate unique output filename
        /// </summary>
        public static string GenerateOutputPath(string basePath, string prefix = "result")
        {
            var directory = Path.GetDirectoryName(basePath) ?? "";
            var extension = Path.GetExtension(basePath);
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            
            return Path.Combine(directory, $"{prefix}_{timestamp}{extension}");
        }

        /// <summary>
        /// Ensure directory exists
        /// </summary>
        public static void EnsureDirectoryExists(string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }

    /// <summary>
    /// Logging utilities
    /// </summary>
    public static class Logger
    {
        private static readonly object lockObject = new object();
        private static string logFilePath = "CVInferencePlatform.log";

        /// <summary>
        /// Set log file path
        /// </summary>
        public static void SetLogFile(string path)
        {
            logFilePath = path;
            FileUtils.EnsureDirectoryExists(path);
        }

        /// <summary>
        /// Log message with timestamp
        /// </summary>
        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var logEntry = $"[{timestamp}] [{level}] {message}";

            // Console output
            Console.WriteLine(logEntry);

            // File output
            lock (lockObject)
            {
                try
                {
                    File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                }
                catch
                {
                    // Ignore file logging errors
                }
            }
        }

        /// <summary>
        /// Log error with exception details
        /// </summary>
        public static void LogError(string message, Exception? exception = null)
        {
            var fullMessage = exception != null 
                ? $"{message}: {exception.Message}\n{exception.StackTrace}"
                : message;
                
            Log(fullMessage, LogLevel.Error);
        }

        /// <summary>
        /// Clear log file
        /// </summary>
        public static void ClearLog()
        {
            lock (lockObject)
            {
                try
                {
                    File.WriteAllText(logFilePath, "");
                }
                catch
                {
                    // Ignore file clearing errors
                }
            }
        }
    }

    /// <summary>
    /// Performance monitoring utilities
    /// </summary>
    public static class PerformanceMonitor
    {
        /// <summary>
        /// Get current memory usage in MB
        /// </summary>
        public static double GetMemoryUsageMB()
        {
            var process = System.Diagnostics.Process.GetCurrentProcess();
            return process.WorkingSet64 / (1024.0 * 1024.0);
        }

        /// <summary>
        /// Get CPU usage percentage (simplified)
        /// </summary>
        public static double GetCpuUsage()
        {
            // Simplified CPU usage - in real implementation would use performance counters
            return Environment.ProcessorCount * 10.0; // Placeholder
        }

        /// <summary>
        /// Format performance metrics for display
        /// </summary>
        public static string FormatPerformanceMetrics(PerformanceMetrics metrics)
        {
            return $"FPS: {metrics.CurrentFPS:F1} | " +
                   $"Avg: {metrics.AverageInferenceTime:F1}ms | " +
                   $"Total: {metrics.TotalInferences} | " +
                   $"Memory: {GetMemoryUsageMB():F1}MB";
        }
    }
}

/// <summary>
/// Log levels for the application
/// </summary>
public enum LogLevel
{
    Debug,
    Info,
    Warning,
    Error
}