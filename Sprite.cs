using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tracks.GameSettings;

namespace Tracks
{
    internal class Sprite
    {
        public Texture Texture { get; private set; }

        public Vector2 Position { get; private set; }
        public Vector2 Scale { get; private set; }
        public Vector2 Origin { get; private set; }
        public Box2 Bounds { get; private set; }

        public Sprite(Texture texture)
            : this(texture, Vector2.Zero)
        {

        }

        public Sprite(Texture texture, Vector2 position)
            : this(texture, position, Vector2.One)
        {
        }

        public Sprite(Texture texture, Vector2 position, Vector2 scale)
            : this(texture, position, scale, Vector2.Zero)
        {
        }

        public Sprite(Texture texture, Vector2 position, Vector2 scale, Vector2 origin)
            : this(texture, position, scale, origin, new Box2(0, 0, texture.Width - 1, texture.Height - 1))
        {
        }

        public Sprite(Texture texture, Vector2 position, Vector2 scale, Vector2 origin, Box2 bounds)
        {
            Texture = texture;
            Position = position;
            Scale = scale;
            Origin = origin;
            Bounds = bounds;
        }

        public void Draw()
        {
            SpriteRenderer spriteRenderer = ServiceLocator.Instance.GetService<SpriteRenderer>();
            spriteRenderer.Draw(this);
        }
    }
}
