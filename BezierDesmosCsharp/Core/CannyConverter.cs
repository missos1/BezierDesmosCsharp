using OpenCvSharp;

namespace BezierCsharp.Core
{
    public static class CannyConverter
    {
        public static Mat Convert(Mat src, bool useGaussBlurr, bool useBilateralFilter)
        {
            Mat edges = new();

            using (Mat grayscale = new())
            {
                Cv2.CvtColor(src, grayscale, ColorConversionCodes.BGR2GRAY);
                if (useBilateralFilter)
                {
                    using (Mat temp = new())
                    {

                        Cv2.BilateralFilter(grayscale, temp, 5, 50, 50);
                        temp.CopyTo(grayscale);
                    }
                }
                else if (useGaussBlurr)
                {
                    Cv2.GaussianBlur(grayscale, grayscale, new Size(5, 5), 1.5);
                }

                double mean = Cv2.Mean(grayscale)[0];
                double lower = Math.Max(0, (1.0 - 0.33) * mean);
                double upper = Math.Min(255, (1.0 + 0.33) * mean);  

                Cv2.Canny(grayscale, edges, lower, upper, apertureSize: 3, L2gradient: true);
            }
            return edges;
        }
    }
}
