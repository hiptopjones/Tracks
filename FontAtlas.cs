using OpenTK.Mathematics;
using static Tracks.GameSettings;

namespace Tracks
{
    internal class FontAtlas
    {
        public TextureArray TextureArray { get; set; }

        public FontAtlas()
        {
            InitializeAtlas();
        }

        private void InitializeAtlas()
        {
            int debugFontTextureId = (int)TextureId.DebugFont;

            ResourceManager resourceManager = ServiceLocator.Instance.GetService<ResourceManager>();

            Vector2i glyphSize = GameSettings.DebugFontGlyphSize;
            int glyphCount = GameSettings.DebugFontGlyphCount;

            TextureArray = resourceManager.GetTextureArray(debugFontTextureId, glyphSize.X, glyphSize.Y, glyphCount);
        }

        public int GetGlyphIndex(char c)
        {
            // First glyph char is space
            int glyphIndex = c - ' ';
            return glyphIndex;
        }
    }
}
