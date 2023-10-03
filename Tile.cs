using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tracks.GameSettings;

namespace Tracks
{
    internal class Tile
    {
        public TextureArray TextureArray { get; private set; }
        public int TileIndex { get; private set; }
        public Vector2 Position { get; private set; }

        // Optional
        public Vector2 Scale { get; set; } = Vector2.One;
        public Color4 Color { get; set; } = Color4.White;

        public Tile(TextureArray textureArray, int tileIndex, Vector2 position)
        {
            TextureArray = textureArray;
            TileIndex = tileIndex;
            Position = position;
        }

        public void Draw()
        {
            TileRenderer tileRenderer = ServiceLocator.Instance.GetService<TileRenderer>();
            tileRenderer.Draw(this);
        }
    }
}
