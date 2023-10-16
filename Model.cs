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
    // Lots of help from
    //  * https://learnopengl.com/Model-Loading/Assimp
    //  * https://github.com/Gargaj/Foxotron (e.g. color map)
    internal class Model
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public List<Mesh> Meshes { get; set; } = new List<Mesh>();

        public static Model LoadFromFile(string modelFilePath)
        {
            AssimpContext importer = new AssimpContext();
            AssimpScene scene = importer.ImportFile(
                modelFilePath,
                PostProcessSteps.Triangulate | PostProcessSteps.JoinIdenticalVertices);

            List<Mesh> meshes = new List<Mesh>();
            ProcessNode(scene, scene.RootNode, meshes);

            return new Model
            {
                Meshes = meshes
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

        private static void ProcessNode(AssimpScene scene, AssimpNode node, List<Mesh> meshes)
        {
            for (int i = 0; i < node.MeshCount; i++)
            {
                int meshIndex = node.MeshIndices[i];
                AssimpMesh mesh = scene.Meshes[meshIndex];

                meshes.Add(ProcessMesh(scene, node, mesh));
            }

            for (int i = 0; i < node.ChildCount; i++)
            {
                ProcessNode(scene, node.Children[i], meshes);
            }
        }

        private static Mesh ProcessMesh(AssimpScene scene, AssimpNode node, AssimpMesh mesh)
        {
            string name = $"{node.Name}.{mesh.Name}";

            List<Vertex> vertices = new List<Vertex>();
            List<int> indices = new List<int>();
            List<ColorMap> colorMaps = new List<ColorMap>();

            for (int i = 0; i < mesh.VertexCount; i++)
            {
                Vertex vertex = new Vertex
                {
                    Position = mesh.Vertices[i].ToVector3()
                };

                if (mesh.HasTextureCoords(0))
                {
                    vertex.TexCoords = mesh.TextureCoordinateChannels[0][i].ToVector3().Xy;
                }

                vertices.Add(vertex);
            }

            for (int i = 0; i < mesh.FaceCount; i++)
            {
                AssimpFace face = mesh.Faces[i];
                indices.AddRange(face.Indices);
            }

            int materialIndex = mesh.MaterialIndex;
            AssimpMaterial material = scene.Materials[materialIndex];

            ColorMap diffuseColorMap = new ColorMap
            {
                Name = "diffuse",
                Color = material.ColorDiffuse.ToColor4(),
                Texture = LoadTexture(material.TextureDiffuse)
            };

            colorMaps.Add(diffuseColorMap);

            Matrix4 transform = GetExpandedTransform(node, Matrix4.Identity);

            return new Mesh(name, vertices, indices, colorMaps, transform);
        }

        // Always this order in OpenTK
        // vertex * (S * R * T) * (S * R * T) * (S * R * T) * . . . 
        // vertex * Child       * Parent    *   Grandparent * (Model * View * Projection)
        private static Matrix4 GetExpandedTransform(AssimpNode node, Matrix4 transform)
        {
            transform *= node.Transform.ToMatrix4();

            if (node.Parent != null)
            {
                transform = GetExpandedTransform(node.Parent, transform);
            }

            return transform;
        }

        private static Texture LoadTexture(TextureSlot textureSlot)
        {
            Texture texture = null;

            if (!string.IsNullOrEmpty(textureSlot.FilePath))
            {
                try
                {
                    texture = Texture.LoadFromFile(textureSlot.FilePath);
                }
                catch (Exception ex)
                {
                    // Some models have bad texture references, so don't take down the program
                    Logger.Error(ex, $"Error loading texture: '{textureSlot.FilePath}'");
                }
            }

            return texture;
        }
    }
}
