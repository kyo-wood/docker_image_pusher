using CVInferencePlatform.Core;
using CVInferencePlatform.Models;
using System;
using System.Collections.Generic;

namespace CVInferencePlatform.UI
{
    /// <summary>
    /// Main form logic (WinForms UI controller)
    /// In a full Windows environment, this would be paired with the actual WinForms designer
    /// </summary>
    public class MainFormLogic
    {
        private readonly InferencePlatform platform;
        private readonly List<string> logMessages;
        private InferenceConfig currentConfig;
        private PerformanceMetrics performanceMetrics;

        // UI State Properties
        public bool IsModelLoaded => platform.CurrentModel != null;
        public string CurrentModelName => platform.CurrentModel?.Name ?? "No model loaded";
        public List<string> LogMessages => logMessages;
        public PerformanceMetrics Performance => performanceMetrics;

        public MainFormLogic()
        {
            platform = new InferencePlatform();
            logMessages = new List<string>();
            currentConfig = new InferenceConfig();
            performanceMetrics = new PerformanceMetrics();
        }

        public void Initialize()
        {
            Log("Initializing Computer Vision Inference Platform UI...");
            
            // Initialize platform
            platform.Initialize();
            
            // Setup default configuration
            InitializeDefaultSettings();
            
            // Initialize performance monitoring
            InitializePerformanceMonitoring();
            
            Log("UI initialized successfully");
            Log("Platform ready for model loading and inference");
            
            // Display current status
            DisplayStatus();
        }

        #region Model Management

        public bool LoadModel(string modelPath)
        {
            Log($"Loading model: {modelPath}");
            
            var success = platform.LoadModel(modelPath);
            if (success)
            {
                Log($"Model loaded: {platform.CurrentModel?.Name}");
                UpdateModelDisplay();
                EnableInferenceControls();
            }
            else
            {
                Log("Failed to load model");
            }
            
            return success;
        }

        public void UnloadModel()
        {
            platform.UnloadModel();
            Log("Model unloaded");
            UpdateModelDisplay();
            DisableInferenceControls();
        }

        public void SaveModelToList(string modelName)
        {
            if (platform.CurrentModel != null)
            {
                // Save to favorites list
                Log($"Model saved to favorites: {modelName}");
            }
        }

        #endregion

        #region Inference Operations

        public InferenceResult? RunInference(string inputPath)
        {
            if (!IsModelLoaded)
            {
                Log("Cannot run inference: No model loaded");
                return null;
            }

            Log($"Starting inference on: {inputPath}");
            var result = platform.RunInference(inputPath, currentConfig);
            
            if (result != null)
            {
                Log($"Inference completed: {result.Detections.Count} detections, {result.InferenceTime}ms");
                UpdatePerformanceMetrics(result);
                return result;
            }
            else
            {
                Log("Inference failed");
                return null;
            }
        }

        public void UpdateInferenceConfig(double confidence, double iou, string device)
        {
            currentConfig.ConfidenceThreshold = confidence;
            currentConfig.IouThreshold = iou;
            currentConfig.Device = device;
            
            Log($"Configuration updated - Confidence: {confidence:F2}, IoU: {iou:F2}, Device: {device}");
        }

        #endregion

        #region UI Updates

        private void UpdateModelDisplay()
        {
            // In actual WinForms implementation, this would update UI controls
            Console.WriteLine($"Model Display Updated: {CurrentModelName}");
            
            if (platform.CurrentModel != null)
            {
                var model = platform.CurrentModel;
                Console.WriteLine($"  Format: {model.Format}");
                Console.WriteLine($"  Input Size: {model.InputSize}");
                Console.WriteLine($"  Classes: {model.NumClasses}");
                Console.WriteLine($"  Loaded: {model.LoadedAt:yyyy-MM-dd HH:mm:ss}");
            }
        }

        private void EnableInferenceControls()
        {
            // Enable start inference button and related controls
            Log("Inference controls enabled");
        }

        private void DisableInferenceControls()
        {
            // Disable start inference button and related controls
            Log("Inference controls disabled");
        }

        private void UpdatePerformanceMetrics(InferenceResult result)
        {
            performanceMetrics.TotalInferences++;
            performanceMetrics.LastUpdated = DateTime.Now;
            
            // Calculate average inference time
            if (performanceMetrics.TotalInferences == 1)
            {
                performanceMetrics.AverageInferenceTime = result.InferenceTime;
            }
            else
            {
                var total = performanceMetrics.AverageInferenceTime * (performanceMetrics.TotalInferences - 1) + result.InferenceTime;
                performanceMetrics.AverageInferenceTime = total / performanceMetrics.TotalInferences;
            }
            
            // Calculate FPS
            performanceMetrics.CurrentFPS = 1000.0 / result.InferenceTime;
            
            Console.WriteLine($"Performance: {performanceMetrics.CurrentFPS:F1} FPS, Avg: {performanceMetrics.AverageInferenceTime:F1}ms");
        }

        #endregion

        #region Helper Methods

        private void InitializeDefaultSettings()
        {
            currentConfig = new InferenceConfig
            {
                ConfidenceThreshold = 0.5,
                IouThreshold = 0.45,
                Device = "Auto",
                BatchSize = 1
            };
        }

        private void InitializePerformanceMonitoring()
        {
            performanceMetrics = new PerformanceMetrics
            {
                TotalInferences = 0,
                AverageInferenceTime = 0,
                CurrentFPS = 0,
                LastUpdated = DateTime.Now
            };
        }

        private void DisplayStatus()
        {
            Console.WriteLine("=== UI Status ===");
            Console.WriteLine($"Model Loaded: {IsModelLoaded}");
            Console.WriteLine($"Current Model: {CurrentModelName}");
            Console.WriteLine($"Confidence Threshold: {currentConfig.ConfidenceThreshold:F2}");
            Console.WriteLine($"IoU Threshold: {currentConfig.IouThreshold:F2}");
            Console.WriteLine($"Device: {currentConfig.Device}");
            Console.WriteLine();
        }

        private void Log(string message)
        {
            var timestamped = $"[{DateTime.Now:HH:mm:ss}] {message}";
            logMessages.Add(timestamped);
            
            // Keep only last 100 messages
            if (logMessages.Count > 100)
            {
                logMessages.RemoveAt(0);
            }
            
            Console.WriteLine(timestamped);
        }

        #endregion

        #region Keyboard Shortcuts (simulated)

        public void HandleKeyPress(string key)
        {
            switch (key.ToUpper())
            {
                case "F5":
                    if (IsModelLoaded)
                    {
                        Log("F5 pressed - Start inference shortcut");
                        // Would trigger inference in actual UI
                    }
                    break;
                    
                case "F6":
                    Log("F6 pressed - Pause inference shortcut");
                    // Would pause inference in actual UI
                    break;
                    
                case "F12":
                    Log("F12 pressed - Save results shortcut");
                    // Would save results in actual UI
                    break;
                    
                case "CTRL+L":
                    Log("Ctrl+L pressed - Load model shortcut");
                    // Would open file dialog in actual UI
                    break;
            }
        }

        #endregion
    }
}