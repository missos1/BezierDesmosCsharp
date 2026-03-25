namespace BezierCsharp.Core
{
    public class ConsoleService
    {
        private readonly AppState _appState;
        private readonly ImagesProcessor _imagesProcessor;

        public ConsoleService(AppState appState, ImagesProcessor imagesProcessor)
        {
            _appState = appState;
            _imagesProcessor = imagesProcessor;
        }

        public void UpdateFromCommand()
        {
            _appState.InitializeDirectories();

            Console.WriteLine($"==================================================================================================\n");
            Console.WriteLine("Welcome to BezierDesmosCsharp!".ToUpper());
            Console.WriteLine("Please make sure to put frame with format frame<index>.png (i.e frame0001.png) for the app to work!");
            Console.WriteLine("\n");
            Console.WriteLine($"==================================================================================================\n");

            bool isProcessed = false;
            while (!isProcessed)
            {
                Console.WriteLine("To see list of commands, enter 'help'. To process images, enter 'start': ");
                Console.Write("> ");
                string input = Console.ReadLine()?.Trim().ToLower() ?? string.Empty;
                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 1)
                {
                    ActionCommand(input);
                    if (parts[0] == "start")
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

        private void ActionCommand(string command)
        {
            switch (command)
            {
                case "start":
                    _imagesProcessor.ProcessImage();
                    Console.WriteLine("Image processing completed. Starting desmos...");
                    break;
                case "help":
                    Console.WriteLine("Available commands:");
                    Console.Write('\n');
                    Console.WriteLine("Start - Process images and start desmos");
                    Console.WriteLine("help - Show this help message");
                    Console.Write('\n');
                    Console.WriteLine("You can also set the following variables before processing images:");
                    Console.Write('\n');
                    Console.WriteLine("inputdir - Set the input directory for images (default: ./Images/Input)");
                    Console.WriteLine("outputdir - Set the output directory for processed images (default: ./Images/Output)");
                    Console.WriteLine("(Note that the input and output directories must exist before setting them. The app will not create them for you, except for the default ones)");
                    Console.WriteLine("canny - Set to true to apply Canny edge detection (default: false)");
                    Console.WriteLine("bilateral - Set to true to apply bilateral filter in Canny edge detection (default: true)");
                    Console.WriteLine("l2grad - Set to true to use L2 gradient in Canny edge detection (default: true)");
                    Console.Write('\n');
                    break;
            }
        }

        private void SettingCommand(string command, string value)
        {
            switch (command)
            {
                case "l2grad":
                    if (bool.TryParse(value, out bool l2Value))
                    {
                        _appState.UseL2Gradient = l2Value;
                        Console.WriteLine($"L2 gradient set to {_appState.UseL2Gradient}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid value for L2grad. Please enter true or false.");
                    }
                    break;
                case "bilateral":
                    if (bool.TryParse(value, out bool bilateralValue))
                    {
                        _appState.UseBilateralFilter = bilateralValue;
                        Console.WriteLine($"Bilateral filter set to {_appState.UseBilateralFilter}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid value for bilateral. Please enter true or false.");
                    }
                    break;
                case "canny":
                    if (bool.TryParse(value, out bool cannyValue))
                    {
                        _appState.UseCanny = cannyValue;
                        Console.WriteLine($"Canny edge detection set to {_appState.UseCanny}. Note that will take significantly longer to render due to Desmos bottlenecking.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid value for canny. Please enter true or false.");
                    }
                    break;
                case "inputdir":
                    if (Directory.Exists(value))
                    {
                        _appState.InputDir = value;
                        Console.WriteLine($"Input directory set to {_appState.InputDir}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid directory path. Please enter a valid path.");
                    }
                    break;
                case "outputdir":
                    if (Directory.Exists(value))
                    {
                        _appState.OutputDir = value;
                        Console.WriteLine($"Output directory set to {_appState.OutputDir}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid directory path. Please enter a valid path.");
                    }
                    break;
            }
            Console.Write('\n');
        }

    }
}
