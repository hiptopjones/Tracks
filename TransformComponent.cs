using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal class TransformComponent : Component
    {
        public Vector2f Position { get; set; } = new Vector2f();
        public float Rotation { get; set; }
        public Vector2f Scale { get; set; } = new Vector2f(1, 1);

        public override void Reset()
        {
            Vector2f position = Position;
            position.X = 0;
            position.Y = 0;
            Position = position;

            Rotation = 0;

            Vector2f scale = Scale;
            scale.X = 1;
            scale.Y = 1;
            Scale = scale;
        }

        public override string ToString()
        {
            return $"[TransformComponent] Position({Position}) Rotation({Rotation}) Scale({Scale})";
        }
    }
}
