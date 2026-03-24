using OpenCvSharp;

namespace BezierCsharp.Core
{
    public static class CannyConverter
    {
        public static Mat Convert(Mat src, bool useBilateralFilter, bool useL2gradient)
        {
            Mat edges = new();
            using (Mat grayscale = new())
            {
                Cv2.CvtColor(src, grayscale, ColorConversionCodes.BGR2GRAY);
                if (useBilateralFilter) // apply a bilateral filter to reduce noise while preserving edges, which can improve the results of Canny edge detection
                {
                    using Mat temp = new();
                    Cv2.BilateralFilter(grayscale, temp, 5, 50, 50);
                    temp.CopyTo(grayscale);
                    double mean = Cv2.Mean(grayscale)[0];
                    double lower = Math.Max(0, (1.0 - 0.33) * mean); // variable lower threshold based on the mean intensity of the image, with a lower bound of 0
                    double upper = Math.Min(255, (1.0 + 0.33) * mean);  

                    Cv2.Canny(grayscale, edges, lower, upper, apertureSize: 3, L2gradient: true);
                }
                else // if not using bilateral filter, use fixed thresholds for Canny edge detection (you can adjust these values as needed)
                {
                    Cv2.Canny(grayscale, edges, 30, 200);
                }
            }
            return edges;
        }
    }
}
