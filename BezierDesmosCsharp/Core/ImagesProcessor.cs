using BitmapToVector;
using OpenCvSharp;

namespace BezierCsharp.Core
{
    public class ImagesProcessor
    {
        private readonly AppState _appState;

        public ImagesProcessor(AppState appState)
        {
            _appState = appState;
        }

        public void ProcessImage()
        {
            var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }; // 

            Parallel.ForEach(Directory.GetFiles(_appState.InputDir), (options), (inputImage) =>
            {   
                try
                {
                    using Mat image = Cv2.ImRead(inputImage);
                    _appState.ImageHeight = image.Height;                                                             // this is not thread safe, but we assume all images have the same dimensions, so it should be fine
                    _appState.ImageWidth = image.Width;                                                               // unless some funky image is in the input directory which what are you doing
                    Mat edges = image;

                    if (_appState.UseCanny)                                                                           // if Canny is enabled, we need to convert the image to edges,
                    {                                                                                                 // otherwise we can just use the original image for tracing
                        // Console.WriteLine($"Processing image: {inputImage} with Canny edge detection.");
                        edges = CannyConverter.Convert(image, _appState.UseBilateralFilter, _appState.UseL2Gradient); // this makes it works better for color images
                    }
                    else                                                                                              // if Canny is not enabled, we need to convert the image to grayscale                                                                                                  // and then threshold it to get a binary image for tracing 
                    {
                        using Mat gray = new();
                        Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
                        Cv2.Threshold(gray, edges, 127, 255, ThresholdTypes.Binary);
                    }

                    PotraceState state = BezierTrace.Trace(edges);
                    List<string> latexStrings = LaTexConverter.Convert(state, edges.Height);

                    string fileName = Path.GetFileName(inputImage);
                    if (int.TryParse(fileName.AsSpan(5, 4), out int frameNum)) 
                    {
                        LatexCache.AddLatexStrings(frameNum, latexStrings);
                    }
                }
                catch (Exception ex) 
                { 
                    Console.WriteLine($"Error processing image {inputImage}: {ex.Message}");
                }
            });
            Console.WriteLine($"Successfully parsed {LatexCache.GetFrameCount()} frames.");
        }
    }
}
