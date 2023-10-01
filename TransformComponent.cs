using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal class TransformComponent : Component
    {
        public Vector3 Position { get; set; } = new Vector3();
        public Vector3 Rotation { get; set; } = new Vector3();
        public Vector3 Scale { get; set; } = new Vector3(1, 1, 1);

        public override void Reset()
        {
            Position = new Vector3();
            Rotation = new Vector3();
            Scale = new Vector3();
        }

        public override string ToString()
        {
            return $"[TransformComponent] Position({Position}) Rotation({Rotation}) Scale({Scale})";
        }
    }
}
