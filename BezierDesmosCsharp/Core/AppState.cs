namespace BezierCsharp.Core
{
    public class AppState
    {
        public bool UseBilateralFilter { get; set; } = true;
        public bool UseL2Gradient { get; set; } = true;
        public bool UseCanny { get; set; } = false;


        public string InputDir { get; set; } = string.Empty;
        public string OutputDir { get; set; } = string.Empty;

        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

        public AppState()
        {
            //InitializeDirectories();
        }

        public void InitializeDirectories()
        {
            var appDirectory = string.Empty;
#if DEBUG
            appDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine($"Running in DEBUG mode. Current directory: {appDirectory}");

#else
            appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine($"Running in RELEASE mode. Base directory: {appDirectory}");

#endif

            InputDir = Path.Combine(appDirectory, "Images", "Input");
            OutputDir = Path.Combine(appDirectory, "Images", "Output");

            if (!Directory.Exists(InputDir) || !Directory.Exists(OutputDir))
            {
                Console.WriteLine("Input & Output directory not found. Creating directories...");
                Directory.CreateDirectory(InputDir);

            }
        }
    }
}
