using OpenTK.Graphics.OpenGL;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal class Texture
    {
        public int Handle { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        // https://github.com/opentk/LearnOpenTK/blob/master/Common/Texture.cs
        public static Texture LoadFromFile(string textureFilePath)
        {
            int textureHandle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);

            // Set texture origin to lower left to match OpenGL
            StbImage.stbi_set_flip_vertically_on_load(1);

            int imageWidth = 0;
            int imageHeight = 0;

            using (Stream stream = File.OpenRead(textureFilePath))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                imageWidth = image.Width;
                imageHeight = image.Height;

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            }

            // Set filters to use when scaling up and down
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);

            // Set texture to wrap in both directions
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            // Use a mipmap to avoid visual artifacts and improve performance
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            return new Texture
            {
                Handle = textureHandle,
                Width = imageWidth,
                Height = imageHeight
            };
        }
    }
}
