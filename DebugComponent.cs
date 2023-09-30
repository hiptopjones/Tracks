using SFML.Graphics;
using SFML.System;

namespace Tracks
{
    internal class DebugComponent : Component
    {
        public override void Update(float deltaTime)
        {
            FloatRect rect = new FloatRect(Owner.Transform.Position, new Vector2f(100, 100));
            Debug.DrawRect(rect, Color.Blue);

            Debug.DrawText(Owner.Name, Owner.Transform.Position, Color.Red);
        }
    }
}