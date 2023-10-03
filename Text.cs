using OpenTK.Mathematics;

namespace Tracks
{
    internal class Text
    {
        public string Message { get; private set; }

        private List<Tile> Tiles { get; set; } = new List<Tile>();
        public Text(string message, Vector2 position, FontAtlas fontAtlas)
            : this(message, position, fontAtlas, Color4.White)
        {
        }

        public Text(string message, Vector2 position, FontAtlas fontAtlas, Color4 color)
        {
            for (int i = 0; i < message.Length; i++)
            {
                // Pushes the characters closer together
                float squeezeFactor = 0.8f;

                Vector2 characterPosition = new Vector2(fontAtlas.TextureArray.TileWidth * i * squeezeFactor, 0);
                Tile tile = new Tile(fontAtlas.TextureArray, fontAtlas.GetGlyphIndex(message[i]), position + characterPosition);
                tile.Color = color;

                Tiles.Add(tile);
            }
        }

        public void Draw()
        {
            foreach (Tile tile in Tiles)
            {
                tile.Draw();
            }
        }
    }
}
