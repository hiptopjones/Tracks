using OpenTK.Mathematics;
using AssimpColor4D = Assimp.Color4D;
using AssimpMatrix4x4 = Assimp.Matrix4x4;
using AssimpVector3D = Assimp.Vector3D;

namespace Tracks
{
    internal static class ConversionExtensions
    {
        public static Vector3 ToVector3(this AssimpVector3D source)
        {
            return new Vector3(source.X, source.Y, source.Z);
        }

        public static Color4 ToColor4(this AssimpColor4D source)
        {
            return new Color4(source.R, source.G, source.B, source.A);
        }

        public static Matrix4 ToMatrix4(this AssimpMatrix4x4 source)
        {
            return new Matrix4(
                source.A1, source.B1, source.C1, source.D1,
                source.A2, source.B2, source.C2, source.D2,
                source.A3, source.B3, source.C3, source.D3,
                source.A4, source.B4, source.C4, source.D4);
        }

        public static float[] ToFloatArray(this List<Vertex> vertexStructs)
        {
            List<float> vertexFloats = new List<float>();

            foreach (Vertex vertexStruct in vertexStructs)
            {
                vertexFloats.Add(vertexStruct.Position.X);
                vertexFloats.Add(vertexStruct.Position.Y);
                vertexFloats.Add(vertexStruct.Position.Z);

                vertexFloats.Add(vertexStruct.TexCoords.X);
                vertexFloats.Add(vertexStruct.TexCoords.Y);
            }

            return vertexFloats.ToArray();
        }
    }
}
