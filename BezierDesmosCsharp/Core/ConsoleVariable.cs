namespace BezierCsharp.Core
{
    public static class ConsoleVariable
    {
        public static bool UseGaussBlurr = false;
        public static bool UseBilateralFilter = true;
        public static bool UseCanny = false;

        private static string appDirectory = "";
        public static string inputDir = "";
        public static string outputDir = "";
        

        public static void UpdateFromCommand()
        {
        #if DEBUG
            appDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine($"Running in DEBUG mode. Current directory: {appDirectory}");

        #else
            appDirectory = AppDomain.CurrentDomain.BaseDirectory;

        #endif

            inputDir = Path.Combine(appDirectory, "Images", "Input");
            outputDir = Path.Combine(appDirectory, "Images", "Output");

            if (!Directory.Exists(inputDir) || !Directory.Exists(outputDir))
            {
                Console.WriteLine("Input & Output directory not found. Creating directories...");
                Directory.CreateDirectory(inputDir);
                Directory.CreateDirectory(outputDir);
            }
            Console.WriteLine($"==================================================================================================\n");
            Console.WriteLine("Welcome to BezierDesmosCsharp!".ToUpper());
            Console.WriteLine("Please make sure to put frame with format frame<index>.png (i.e frame0001.png) for the app to work!");
            Console.WriteLine("\n");
            Console.WriteLine($"==================================================================================================\n");

            bool isProcessed = false;
            while (!isProcessed)
            {
                Console.WriteLine("To see list of commands, enter 'help'. To process images, enter 1: ");
                Console.Write("> ");
                string input = Console.ReadLine()?.Trim().ToLower() ?? string.Empty;
                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 1)
                {
                    ActionCommand(input);
                    if (parts[0] == "1")
                    {
                        isProcessed = true;
                    }
                }
                else if (parts.Length == 2)
                {
                    SettingCommand(parts[0], parts[1]);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a command or set a variable.");
                }
            }


        }

        private static void ActionCommand(string command)
        {
            switch (command)
            {
                case "1":
                    ImagesProcesor.ProcessImage();
                    Console.WriteLine("Image processing completed. Starting desmos...");
                    break;
                case "help":
                    Console.WriteLine("Available commands:");
                    Console.Write('\n');
                    Console.WriteLine("1 - Process images and start desmos");
                    Console.WriteLine("help - Show this help message");
                    Console.Write('\n');
                    Console.WriteLine("You can also set the following variables before processing images:");
                    Console.Write('\n');
                    Console.WriteLine("blur - Set to true to apply Gaussian blur (default: false)");
                    Console.WriteLine("bilateral - Set to true to apply bilateral filter (default: true)");
                    Console.WriteLine("canny - Set to true to apply Canny edge detection (default: false)");
                    Console.Write('\n');
                    break;
            }
        }

        private static void SettingCommand(string command, string value)
        {
            switch (command)
            {
                case "blur":
                    if (bool.TryParse(value, out bool blurValue))
                    {
                        UseGaussBlurr = blurValue;
                        Console.WriteLine($"Gaussian blur set to {UseGaussBlurr}");
                        
                    }
                    else
                    {
                        Console.WriteLine("Invalid value for blur. Please enter true or false.");
                    }
                    break;
                case "bilateral":
                    if (bool.TryParse(value, out bool bilateralValue))
                    {
                        UseBilateralFilter = bilateralValue;
                        Console.WriteLine($"Bilateral filter set to {UseBilateralFilter}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid value for bilateral. Please enter true or false.");
                    }
                    break;
                case "canny":
                    if (bool.TryParse(value, out bool cannyValue))
                    {
                        UseCanny = cannyValue;
                        Console.WriteLine($"Canny edge detection set to {UseCanny}. Note that will take significantly longer to render due to Desmos bottlenecking.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid value for canny. Please enter true or false.");
                    }
                    break;
            }
            Console.Write('\n');
        }

    }
}
