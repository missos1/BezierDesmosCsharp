using BitmapToVector;

namespace BezierCsharp.Core
{
    public static class LaTexConverter
    {
        public static List<string> Convert(PotraceState state, float height)
        {
            List<string> latex = new List<string>();
            PotracePath currentPath = state.Plist;

            while (currentPath != null)                                                 // Potrace path is a linked list so  iterate through it until it reaches null
            {
                var curve = currentPath.Curve;                                          // Get the curve data for the current path

                PotraceDPoint start = curve.C[curve.N - 1][2];                          /* Start point is the last point of the last segment of the curve
                                                                                        (the end point of the last segment is the start point of the first segment) */
                for (int segment = 0; segment < curve.N; segment++)                     // Iterate through each segment of the curve
                {
                    int tag = curve.Tag[segment];                                       // Get the tag for the current segment (corner or curve)
                    PotraceDPoint[] segmentPoints = curve.C[segment];                    
                    double x0 = start.X; 
                    double y0 = -start.Y + height;                                      // Invert the y-coordinate and adjust for height to match the coordinate system used in Desmos (do the same for all y's)

                    if (tag == PotraceCurve.PotraceCorner)                              // Latex for corner segments
                    {
                        double x1 = segmentPoints[1].X, y1 = -segmentPoints[1].Y + height;
                        double x2 = segmentPoints[2].X, y2 = -segmentPoints[2].Y + height;
                                                                                        // No control points for corner segments
                        latex.Add($"((1-t){x0:0.#####} + {x1:0.#####}t, (1-t){y0:0.#####} + {y1:0.#####}t)");
                        latex.Add($"((1-t){x1:0.######} + {x2:0.#####}t, (1-t){y1:0.#####} + {y2:0.#####}t)");
                    }
                    else if (tag == PotraceCurve.PotraceCurveTo)                        // Latex for non corner segments (Bezier curves)
                    {
                        double x1 = segmentPoints[0].X, y1 = -segmentPoints[0].Y + height;
                        double x2 = segmentPoints[1].X, y2 = -segmentPoints[1].Y + height;
                        double x3 = segmentPoints[2].X, y3 = -segmentPoints[2].Y + height;

                        latex.Add($"((1-t)^3 {x0:0.#####} + 3(1-t)^2 {x1:0.#####}t + 3(1-t) {x2:0.#####}t^2 + {x3:0.#####}t^3, " +
                                  $"(1-t)^3 {y0:0.#####} + 3(1-t)^2 {y1:0.#####}t + 3(1-t) {y2:0.#####}t^2 + {y3:0.#####}t^3)");
                    }
                    start = segmentPoints[2];                                           // Update the start point for the next segment
                }
                currentPath = currentPath.Next;                                         // Move to the next path in the list
            }
            return latex;
        }
    }
}
