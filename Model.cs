using Assimp;
using OpenTK.Mathematics;
using System.Drawing;
using AssimpFace = Assimp.Face;
using AssimpMesh = Assimp.Mesh;
using AssimpNode = Assimp.Node;
using AssimpScene = Assimp.Scene;
using AssimpVector2D = Assimp.Vector2D;
using AssimpVector3D = Assimp.Vector3D;

namespace Tracks
{
    internal class Model
    {
        public List<Mesh> Meshes { get; set; } = new List<Mesh>();

        public static Model LoadFromFile(string modelFilePath)
        {
            AssimpContext importer = new AssimpContext();
            AssimpScene scene = importer.ImportFile(modelFilePath, PostProcessSteps.Triangulate);

            List<Mesh> meshes = new List<Mesh>();
            ProcessNode(scene, scene.RootNode, meshes);

            return new Model
            {
                Meshes = meshes
            };
        }

        public void Draw(ShaderProgram shaderProgram)
        {
            foreach (Mesh mesh in Meshes)
            {
                mesh.Draw(shaderProgram);
            }
        }

        private static void ProcessNode(AssimpScene scene, AssimpNode node, List<Mesh> meshes)
        {
            for (int i = 0; i < node.MeshCount; i++)
            {
                int meshIndex = node.MeshIndices[i];
                AssimpMesh mesh = scene.Meshes[meshIndex];

                meshes.Add(ProcessMesh(mesh));
            }

            for (int i = 0; i < node.ChildCount; i++)
            {
                ProcessNode(scene, node.Children[i], meshes);
            }
        }

        private static Mesh ProcessMesh(AssimpMesh mesh)
        {
            List<Vertex> vertices = new List<Vertex>();
            List<int> indices = new List<int>();
            List<Texture> textures = new List<Texture>();

            for (int i = 0; i < mesh.VertexCount; i++)
            {
                Vertex vertex = new Vertex
                {
                    Position = ToVector3(mesh.Vertices[i])
                };

                if (mesh.HasTextureCoords(0))
                {
                    vertex.TexCoords = ToVector3(mesh.TextureCoordinateChannels[0][i]).Xy;
                }

                vertices.Add(vertex);
            }

            for (int i = 0; i < mesh.FaceCount; i++)
            {
                AssimpFace face = mesh.Faces[i];
                indices.AddRange(face.Indices);
            }

            return new Mesh(vertices, indices, textures);
        }

        private static Vector3 ToVector3(AssimpVector3D source)
        {
            return new Vector3(source.X, source.Y, source.Z);
        }
    }
}
