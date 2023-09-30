using SFML.Graphics;
using SFML.System;

namespace Tracks
{
    internal class DebugComponent : Component
    {
        public Vector2f Extents { get; set; }

        public override void Update(float deltaTime)
        {
            FloatRect rect = new FloatRect(Owner.Transform.Position - Extents / 2, Extents);
            Debug.DrawRect(rect, Color.Magenta);

            Debug.DrawText(Owner.Name, Owner.Transform.Position - Extents / 2, Color.Magenta);
        }
    }
}