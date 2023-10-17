using OpenTK.Mathematics;

namespace Tracks
{
    internal class CatmullRomSplineGenerator
    {
        // https://www.habrador.com/tutorials/interpolation/1-catmull-rom-splines/
        public static Vector3[] GenerateSplinePoints(Vector3[] controlPoints, float splineResolution)
        {
            List<Vector3> splineVertices = new List<Vector3>();

            int numSteps = (int)MathF.Round(1 / splineResolution);

            // NOTE: This method discards the first and last point in the path, due to how the algorithm works
            for (int i = 1; i < controlPoints.Length - 2; i++)
            {
                Vector3 p0 = controlPoints[i - 1];
                Vector3 p1 = controlPoints[i];
                Vector3 p2 = controlPoints[i + 1];
                Vector3 p3 = controlPoints[i + 2];

                for (int j = 0; j < numSteps; j++)
                {
                    float t = j * splineResolution;

                    // Find the coordinate between the end points with a Catmull-Rom spline
                    Vector3 splinePosition = GetCatmullRomPosition(t, p0, p1, p2, p3);
                    splineVertices.Add(splinePosition);
                }
            }

            // Add the final p2 from the above loop
            splineVertices.Add(controlPoints[controlPoints.Length - 2]);

            return splineVertices.ToArray();
        }

        // Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
        // http://www.iquilezles.org/www/articles/minispline/minispline.htm
        private static Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
            Vector3 a = 2f * p1;
            Vector3 b = p2 - p0;
            Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
            Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

            //The cubic polynomial: a + b * t + c * t^2 + d * t^3
            Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

            return pos;
        }
    }
}