using CVInferencePlatform.Core;
using CVInferencePlatform.UI;
using CVInferencePlatform.Tests;
using System;

namespace CVInferencePlatform
{
    /// <summary>
    /// Main entry point for the Computer Vision Inference Platform
    /// </summary>
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("=== Computer Vision Inference Platform ===");
            Console.WriteLine("Version 1.0.0");
            Console.WriteLine();

            // Check for test mode
            if (args.Length > 0 && args[0] == "--test")
            {
                // Run comprehensive tests
                var tester = new PlatformTest();
                tester.RunAllTests();
                return;
            }

            // Initialize platform
            var platform = new InferencePlatform();
            
            if (args.Length > 0 && args[0] == "--console")
            {
                // Console mode for demonstration
                RunConsoleDemo(platform);
            }
            else
            {
                // GUI mode (would launch WinForms in Windows environment)
                Console.WriteLine("GUI Mode: This would launch the WinForms interface in a Windows environment.");
                Console.WriteLine("Available modes:");
                Console.WriteLine("  dotnet run -- --console  (Console demo)");
                Console.WriteLine("  dotnet run -- --test     (Run test suite)");
                Console.WriteLine();
                Console.WriteLine("Platform Features:");
                Console.WriteLine("- Model Management (PyTorch .pt, ONNX .onnx, TensorRT .engine)");
                Console.WriteLine("- Multi-format model loading and inference");
                Console.WriteLine("- Object detection, image classification, semantic segmentation");
                Console.WriteLine("- CUDA acceleration support with CPU fallback");
                Console.WriteLine("- Real-time camera input processing");
                Console.WriteLine("- Configurable confidence and IoU thresholds");
                Console.WriteLine("- Performance monitoring and logging");
                Console.WriteLine();
                
                // Simulate GUI initialization
                var mainForm = new MainFormLogic();
                mainForm.Initialize();
            }
        }

        static void RunConsoleDemo(InferencePlatform platform)
        {
            Console.WriteLine("Running Console Demo...");
            Console.WriteLine();

            // Demonstrate platform capabilities
            platform.Initialize();
            platform.ShowStatus();
            
            Console.WriteLine();
            Console.WriteLine("Demo completed. In a full Windows environment, this would launch");
            Console.WriteLine("a complete WinForms interface with all specified features.");
        }
    }
}
