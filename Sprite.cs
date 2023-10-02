using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal class Sprite
    {
        public Texture Texture { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public Box2 Bounds { get; set; }

        public Sprite(Texture texture)
        {
            Texture = texture;
        }

        public void Draw()
        {
            SpriteRenderer spriteRenderer = ServiceLocator.Instance.GetService<SpriteRenderer>();
            spriteRenderer.Draw(this);
        }
    }
}
