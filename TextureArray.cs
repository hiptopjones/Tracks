using NLog;
using OpenTK.Graphics.OpenGL;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal class TextureArray
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public int Handle { get; private set; }

        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public int TileCount { get; private set; }

        // Requires that the texture is packed (no unaccounted space on rows)
        public static TextureArray LoadFromFile(string textureFilePath, int tilePixelWidth, int tilePixelHeight, int tileCount)
        {
            int textureHandle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2DArray, textureHandle);

            // Set texture origin to lower left to match OpenGL
            StbImage.stbi_set_flip_vertically_on_load(1);

            using (Stream stream = File.OpenRead(textureFilePath))
            {
                List<byte[]> tiles = new List<byte[]>();

                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                byte[] imageBytes = image.Data;

                int imagePixelWidth = image.Width;
                int imagePixelHeight = image.Height;

                int tileColumns = imagePixelWidth / tilePixelWidth;
                int tileRows = imagePixelHeight / tilePixelHeight;

                for (int tileIndex = 0; tileIndex < tileCount; tileIndex++)
                {
                    byte[] tileBytes = new byte[tilePixelWidth * tilePixelHeight * 4]; // 4 bytes for RGBA pixels

                    int tileX = tileIndex % tileColumns;
                    int tileY = tileIndex / tileColumns;

                    // Indicates the location in the image buffer of the first pixel in this tile
                    int imagePixelOffset = (tileY * imagePixelWidth * tilePixelHeight + tileX * tilePixelWidth);

                    int tileByteIndex = 0;
                    for (int tilePixelY = 0; tilePixelY < tilePixelHeight; tilePixelY++)
                    {
                        for (int tilePixelX = 0; tilePixelX < tilePixelWidth; tilePixelX++)
                        {
                            int imagePixelIndex = imagePixelOffset + tilePixelY * imagePixelWidth + tilePixelX;
                            int imageByteIndex = imagePixelIndex * 4;  // 4 bytes for RGBA pixels

                            tileBytes[tileByteIndex++] = imageBytes[imageByteIndex + 0];
                            tileBytes[tileByteIndex++] = imageBytes[imageByteIndex + 1];
                            tileBytes[tileByteIndex++] = imageBytes[imageByteIndex + 2];
                            tileBytes[tileByteIndex++] = imageBytes[imageByteIndex + 3];
                        }
                    }

                    tiles.Add(tileBytes);
                }

                // NOTE: Could probably create the array a different / better way, but this was easier to debug and get working
                GL.TexStorage3D(TextureTarget3d.Texture2DArray, 1, SizedInternalFormat.Rgba8, tilePixelWidth, tilePixelHeight, tileCount);

                for (int tileIndex = 0; tileIndex < tileCount; tileIndex++)
                {
                    GL.TexSubImage3D(TextureTarget.Texture2DArray, 0, 0, 0, tileIndex, tilePixelWidth, tilePixelHeight, 1, PixelFormat.Rgba, PixelType.UnsignedByte, tiles[tileIndex]);
                }
            }

            // Set filters to use when scaling up and down
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);

            // Set texture to wrap in both directions
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.BindTexture(TextureTarget.Texture2DArray, 0);

            return new TextureArray
            {
                Handle = textureHandle,
                TileWidth = tilePixelWidth,
                TileHeight = tilePixelHeight,
                TileCount = tileCount
            };
        }
    }
}
