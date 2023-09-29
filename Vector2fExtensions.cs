using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    public static class Vector2fExtensions
    {
        public const float EPSILON = 0.00001f;

        public static float Magnitude(this Vector2f vector)
        {
            return MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static Vector2f Normalize(this Vector2f vector)
        {
            float magnitude = vector.Magnitude();
            if (magnitude > EPSILON)
            {
                return new Vector2f(vector.X / magnitude, vector.Y / magnitude);
            }

            return new Vector2f(0, 0);
        }

        public static float DistanceTo(this Vector2f start, Vector2f end)
        {
            return (end - start).Magnitude();
        }
    }
}
