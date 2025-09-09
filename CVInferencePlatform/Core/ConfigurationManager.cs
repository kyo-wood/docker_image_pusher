using System;

namespace CVInferencePlatform.Core
{
    /// <summary>
    /// Configuration manager for inference settings and application preferences
    /// </summary>
    public class ConfigurationManager
    {
        public InferenceConfig DefaultInferenceConfig { get; private set; }
        public ApplicationSettings ApplicationSettings { get; private set; }

        public ConfigurationManager()
        {
            DefaultInferenceConfig = new InferenceConfig();
            ApplicationSettings = new ApplicationSettings();
        }

        public void LoadDefaultSettings()
        {
            // Load default inference configuration
            DefaultInferenceConfig = new InferenceConfig
            {
                ConfidenceThreshold = 0.5,
                IouThreshold = 0.45,
                Device = "Auto",
                BatchSize = 1,
                MaxDetections = 100
            };

            // Load application settings
            ApplicationSettings = new ApplicationSettings
            {
                LogLevel = LogLevel.Info,
                SaveResultsAutomatically = false,
                DefaultInputSource = CVInferencePlatform.Models.InputSourceType.ImageFile,
                MaxLogMessages = 1000,
                PerformanceMonitoring = true
            };
        }

        public void SaveSettings()
        {
            // Save current settings to configuration file
            // In a full implementation, this would write to JSON/XML config
        }

        public void LoadSettings()
        {
            // Load settings from configuration file
            // In a full implementation, this would read from JSON/XML config
        }

        public void ResetToDefaults()
        {
            LoadDefaultSettings();
        }
    }

    /// <summary>
    /// Inference configuration parameters
    /// </summary>
    public class InferenceConfig
    {
        public double ConfidenceThreshold { get; set; } = 0.5;
        public double IouThreshold { get; set; } = 0.45;
        public string Device { get; set; } = "Auto";
        public int BatchSize { get; set; } = 1;
        public int MaxDetections { get; set; } = 100;
        public bool EnableNMS { get; set; } = true;
    }

    /// <summary>
    /// Application-wide settings
    /// </summary>
    public class ApplicationSettings
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Info;
        public bool SaveResultsAutomatically { get; set; } = false;
        public string DefaultOutputPath { get; set; } = "Results";
        public CVInferencePlatform.Models.InputSourceType DefaultInputSource { get; set; } = CVInferencePlatform.Models.InputSourceType.ImageFile;
        public int MaxLogMessages { get; set; } = 1000;
        public bool PerformanceMonitoring { get; set; } = true;
        public bool ShowConfidenceInResults { get; set; } = true;
        public bool ShowClassNamesInResults { get; set; } = true;
    }

    /// <summary>
    /// Logging levels
    /// </summary>
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }
}