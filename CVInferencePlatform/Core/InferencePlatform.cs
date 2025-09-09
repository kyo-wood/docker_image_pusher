using CVInferencePlatform.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace CVInferencePlatform.Core
{
    /// <summary>
    /// Core inference platform managing model loading, configuration, and inference operations
    /// </summary>
    public class InferencePlatform
    {
        private readonly ModelManager modelManager;
        private readonly InferenceEngine inferenceEngine;
        private readonly ConfigurationManager configManager;
        private readonly List<string> logMessages;

        public bool IsInitialized { get; private set; }
        public ModelInfo? CurrentModel { get; private set; }
        
        public InferencePlatform()
        {
            modelManager = new ModelManager();
            inferenceEngine = new InferenceEngine();
            configManager = new ConfigurationManager();
            logMessages = new List<string>();
        }

        public void Initialize()
        {
            Log("Initializing Computer Vision Inference Platform...");
            
            try
            {
                // Initialize components
                modelManager.Initialize();
                inferenceEngine.Initialize();
                configManager.LoadDefaultSettings();
                
                Log("Platform initialized successfully");
                Log($"Supported formats: {string.Join(", ", modelManager.SupportedFormats)}");
                Log($"Available devices: {string.Join(", ", inferenceEngine.AvailableDevices)}");
                
                IsInitialized = true;
            }
            catch (Exception ex)
            {
                Log($"Initialization failed: {ex.Message}");
                IsInitialized = false;
            }
        }

        public bool LoadModel(string modelPath)
        {
            if (!IsInitialized)
            {
                Log("Platform not initialized");
                return false;
            }

            try
            {
                Log($"Loading model: {Path.GetFileName(modelPath)}");
                
                // Validate model file
                if (!File.Exists(modelPath))
                {
                    Log($"Model file not found: {modelPath}");
                    return false;
                }

                var modelInfo = modelManager.LoadModel(modelPath);
                if (modelInfo != null)
                {
                    CurrentModel = modelInfo;
                    Log($"Model loaded successfully: {modelInfo.Name}");
                    Log($"Format: {modelInfo.Format}, Input Size: {modelInfo.InputSize}");
                    return true;
                }
                else
                {
                    Log("Failed to load model");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log($"Error loading model: {ex.Message}");
                return false;
            }
        }

        public void UnloadModel()
        {
            if (CurrentModel != null)
            {
                modelManager.UnloadModel();
                CurrentModel = null;
                Log("Model unloaded");
            }
        }

        public InferenceResult? RunInference(string inputPath, InferenceConfig config)
        {
            if (CurrentModel == null)
            {
                Log("No model loaded");
                return null;
            }

            try
            {
                Log($"Running inference on: {Path.GetFileName(inputPath)}");
                Log($"Confidence: {config.ConfidenceThreshold}, IoU: {config.IouThreshold}");
                
                var result = inferenceEngine.RunInference(CurrentModel, inputPath, config);
                
                if (result != null)
                {
                    Log($"Inference completed - Detections: {result.Detections.Count}, Time: {result.InferenceTime}ms");
                    return result;
                }
                else
                {
                    Log("Inference failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log($"Inference error: {ex.Message}");
                return null;
            }
        }

        public void ShowStatus()
        {
            Console.WriteLine("=== Platform Status ===");
            Console.WriteLine($"Initialized: {IsInitialized}");
            Console.WriteLine($"Current Model: {CurrentModel?.Name ?? "None"}");
            Console.WriteLine($"Supported Formats: {string.Join(", ", modelManager.SupportedFormats)}");
            Console.WriteLine($"Available Devices: {string.Join(", ", inferenceEngine.AvailableDevices)}");
            Console.WriteLine();
            
            Console.WriteLine("=== Recent Log Messages ===");
            foreach (var message in logMessages.TakeLast(10))
            {
                Console.WriteLine(message);
            }
        }

        private void Log(string message)
        {
            var timestamped = $"[{DateTime.Now:HH:mm:ss}] {message}";
            logMessages.Add(timestamped);
            Console.WriteLine(timestamped);
        }
    }
}