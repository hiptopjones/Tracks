using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal static class Easings
    {
        // https://gist.github.com/Kryzarel/bba64622057f21a1d6d44879f9cd7bd4
        public static float InBack(float t)
        {
            float s = 1.70158f;
            return t * t * ((s + 1) * t - s);
        }
    }
}
