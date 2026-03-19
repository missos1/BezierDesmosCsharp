using BitmapToVector;
using OpenCvSharp;

namespace BezierCsharp.Core
{
    public static class BezierTrace 
    {
        public static PotraceState Trace(Mat edges)
        {
            int width = edges.Width;
            int height = edges.Height;

            var bitmap = PotraceBitmap.Create(width, height); // Create a PotraceBitmap with the same dimensions as the edge image

            IntPtr ptr = edges.Data;
            int step = (int)edges.Step();

            unsafe // AI told me to use unsafe code here to access the pixel data of the edge image directly for better performance but well idk tell me the real deal with issue or sth
            {
                byte* rowPtr = (byte*)ptr.ToPointer(); // Get a pointer to the pixel data of the edge image
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width;x++)
                    {
                        
                        if (rowPtr[x] > 128) // If the pixel value is greater than 128 (indicating an edge), set the corresponding pixel in the PotraceBitmap to black
                        {
                            bitmap.SetBlackUnsafe(x, y); // Set the corresponding pixel in the PotraceBitmap to black
                        }
                    }
                    rowPtr += step; // Move to the next row of pixel data
                }
            }

            return Potrace.Trace(new PotraceParam(), bitmap); // Trace the bitmap
        }
    }
}
