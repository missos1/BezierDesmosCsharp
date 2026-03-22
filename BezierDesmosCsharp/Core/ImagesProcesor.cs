using BitmapToVector;
using OpenCvSharp;

namespace BezierCsharp.Core
{
    public static class ImagesProcesor
    {
        public static void ProcessImage()
        {
            var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

            Parallel.ForEach(Directory.GetFiles(ConsoleVariable.inputDir), (options), (inputImage) =>
            {   
                try
                {
                    using Mat image = Cv2.ImRead(inputImage);
                    Mat edges = image;

                    if (ConsoleVariable.UseCanny)
                    {
                        // Console.WriteLine($"Processing image: {inputImage} with Canny edge detection.");
                        edges = CannyConverter.Convert(image, ConsoleVariable.UseGaussBlurr, ConsoleVariable.UseBilateralFilter);
                    }
                    else
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
