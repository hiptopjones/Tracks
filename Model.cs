using Assimp;
using NLog;
using OpenTK.Mathematics;
using AssimpFace = Assimp.Face;
using AssimpMaterial = Assimp.Material;
using AssimpMesh = Assimp.Mesh;
using AssimpNode = Assimp.Node;
using AssimpScene = Assimp.Scene;

namespace Tracks
{
    internal class Model
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public List<Mesh> Meshes { get; set; } = new List<Mesh>();

        public static Model LoadFromFile(string modelFilePath)
        {
            return ModelLoader.LoadFromFile(modelFilePath);
        }

        public static Model CreateFromMeshes(List<Mesh> meshes)
        {
            return new Model
            {
                Meshes = meshes
            };
        }

        public static Model CreateFromMesh(Mesh mesh)
        {
            return new Model
            {
                Meshes = new List<Mesh> { mesh }
            };
        }

        public void Draw(ShaderProgram shaderProgram, Matrix4 modelTransform)
        {
            foreach (Mesh mesh in Meshes)
            {
                // Always this order in OpenTK
                // vertex * (S * R * T) * (S * R * T) * (S * R * T) * . . . 
                // vertex * Child       * Parent    *   Grandparent * (Model * View * Projection)
                Matrix4 meshTransform = mesh.Transform * modelTransform;
                shaderProgram.SetUniform("model", meshTransform);

                mesh.Draw(shaderProgram);
            }
        }
    }
}
