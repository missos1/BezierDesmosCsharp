namespace BezierCsharp.Core
{
    public class ConsoleService
    {
        private readonly AppState _appState;
        private readonly ImagesProcessor _imagesProcessor;
        private readonly Dictionary<string, Action<string>> _settingCommands;
        
        public ConsoleService(AppState appState, ImagesProcessor imagesProcessor)
        {
            _appState = appState;
            _imagesProcessor = imagesProcessor;
            _settingCommands = new Dictionary<string, Action<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "takescreenshot", value => _appState.TakeScreenshot = bool.TryParse(value, out bool result) && result },
                { "l2grad", value => _appState.UseL2Gradient = bool.TryParse(value, out bool result) && result },
                { "bilateral", value => _appState.UseBilateralFilter = bool.TryParse(value, out bool result) && result },
                { "canny", value => _appState.UseCanny = bool.TryParse(value, out bool result) && result },
                { "inputdir", value => 
                    { 
                        if (Directory.Exists(value)) 
                        {
                            _appState.InputDir = value; 
                        }
                        else     
                        {                
                            Console.WriteLine($"Input directory '{value}' does not exist. Please provide a valid directory.");
                        }
                    } 
                },
                { "outputdir", value => 
                    { 
                        if (Directory.Exists(value)) 
                        {
                            _appState.OutputDir = value; 
                        }
                        else                     {
                            Console.WriteLine($"Output directory '{value}' does not exist. Please provide a valid directory.");
                        }
                    } 
                }
            };
        }

        public void UpdateFromCommand()
        {
            _appState.InitializeDirectories();

            Console.WriteLine($"==================================================================================================\n");
            Console.WriteLine("Welcome to BezierDesmosCsharp!".ToUpper());
            Console.WriteLine("Please make sure to put frame with format frame<index>.png (i.e frame0001.png) for the app to work!");
            Console.WriteLine("\n");
            Console.WriteLine($"==================================================================================================\n");
            Console.WriteLine("To see list of commands, enter 'help'. To process images, enter 'start': ");

            bool isProcessed = false;
            while (!isProcessed)
            {
                Console.Write("> ");
                string input = Console.ReadLine()?.Trim().ToLower() ?? string.Empty;
                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 1)
                {
                    ActionCommand(input);
                    if (parts[0].ToLower().Equals("start"))
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
                    Console.WriteLine("start - Process images and start desmos");
                    Console.WriteLine("help - Show this help message");
                    Console.Write('\n');
                    Console.WriteLine("You can also set the following variables before processing images:");
                    Console.Write('\n');
                    Console.WriteLine("inputdir - Set the input directory for images (default: ./Images/Input)");
                    Console.WriteLine("outputdir - Set the output directory for processed images (default: ./Images/Output)");
                    Console.WriteLine("takescreenshot - Set to true to take a screenshot of the Desmos graph after rendering (default: true)");
                    Console.WriteLine("(Note that the input and output directories must exist before setting them. The app will not create them for you, except for the default ones)");
                    Console.WriteLine("canny - Set to true to apply Canny edge detection (default: false)");
                    Console.WriteLine("bilateral - Set to true to apply bilateral filter in Canny edge detection (default: true)");
                    Console.WriteLine("l2grad - Set to true to use L2 gradient in Canny edge detection (default: true)");
                    Console.WriteLine("viewsettings - View current settings");
                    Console.Write('\n');
                    break;
                case "viewsettings":
                    Console.WriteLine("Current settings:");
                    Console.WriteLine($"inputdir: {_appState.InputDir}");
                    Console.WriteLine($"outputdir: {_appState.OutputDir}");
                    Console.WriteLine($"takescreenshot: {_appState.TakeScreenshot}");
                    Console.WriteLine($"canny: {_appState.UseCanny}");
                    Console.WriteLine($"bilateral: {_appState.UseBilateralFilter}");
                    Console.WriteLine($"l2grad: {_appState.UseL2Gradient}");
                    break;
                case "exit":
                    Console.WriteLine("Exiting the application.");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine($"Unknown command: {command}. Type 'help' for a list of commands.");
                    break;
            }
        }

        //private void response(string command, string value, Action<bool> setter, string suffix)
        //{
        //    if (bool.TryParse(value, out bool result))
        //    {
        //        setter(result);
        //        Console.WriteLine($"{command} set to {result}");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Invalid value for {command}. Please enter {suffix}.");
        //    }
        //}


        private void SettingCommand(string command, string value)
        {
            
            _settingCommands.TryGetValue(command, out Action<string>? action);
            try 
            {
                if (action != null)
                {
                    action(value);
                    Console.WriteLine($"Setting {command} to {value}");
                }
                else
                {
                    Console.WriteLine($"Unknown command: {command}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while processing command: {command}. Error: {ex.Message}");
            }
            Console.Write('\n');
        }

    }
}
