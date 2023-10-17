using OpenTK.Mathematics;

namespace Tracks
{
    // Extracted from https://github.com/hiptopjones/Roads/blob/main/Assets/Scripts/RoadMesh.cs
    internal class RoadMeshGenerator
    {
        private Vector3[] RoadWaypoints { get; set; }
        private float HalfRoadWidth { get; set; }
        private int NumSplineSteps { get; set; }

        public class SplinePoint
        {
            public Vector3 Position { get; set; }
            public Vector3 Tangent { get; set; }
            public Vector3 Normal { get; set; }
        }

        public RoadMeshGenerator(Vector3[] roadWaypoints, float halfRoadWidth, int numSplineSteps, int numTrimmedWaypoints)
        {
            RoadWaypoints = roadWaypoints;
            HalfRoadWidth = halfRoadWidth;
            NumSplineSteps = numSplineSteps;
        }

        public Mesh CreateMesh()
        {
            if (RoadWaypoints == null || RoadWaypoints.Length == 0)
            {
                throw new Exception("No waypoints provided");
            }

            // Create a spline from the waypoints
            Vector3[] splineVertices = CatmullRomSplineGenerator.GenerateSplinePoints(RoadWaypoints, 1f / NumSplineSteps);

            // Conform the spline to the terrain
            SplinePoint[] splinePoints = ConformRoadSpline(splineVertices);

            // Extrude a mesh along the spline to create a road surface
            Mesh roadMesh = GenerateRoadMesh(splinePoints);

            return roadMesh;
       }

        private SplinePoint[] ConformRoadSpline(Vector3[] splineVertices)
        {
            SplinePoint[] splinePoints = new SplinePoint[splineVertices.Length];

            for (int i = 0; i < splineVertices.Length; i++)
            {
                Vector3 splineVertex = splineVertices[i];

                SplinePoint splinePoint = new SplinePoint
                {
                    Position = splineVertex
                };

                splinePoints[i] = splinePoint;
            }

            // Now update the tangents and normals
            for (int i = 0; i < splinePoints.Length; i++)
            {
                if (i == 0) // First point
                {
                    // Use direction from P(i) to P(i+1)
                    splinePoints[i].Tangent = (splinePoints[i + 1].Position - splinePoints[i].Position).Normalized();
                }
                else if (i == splinePoints.Length - 1) // Last point
                {
                    // Use direction from P(i-1) to P(i)
                    splinePoints[i].Tangent = (splinePoints[i].Position - splinePoints[i - 1].Position).Normalized();
                }
                else // All other points
                {
                    // Use direction from P(i-1) to P(i+1)
                    splinePoints[i].Tangent = (splinePoints[i + 1].Position - splinePoints[i - 1].Position).Normalized();
                }

                // Normal is always calculated to the right
                splinePoints[i].Normal = Vector3.Cross(Vector3.UnitY, splinePoints[i].Tangent).Normalized();
            }

            return splinePoints;
        }

        private Mesh GenerateRoadMesh(SplinePoint[] splinePoints)
        {
            List<Vector3> roadVertices = new List<Vector3>();
            List<int> roadIndices = new List<int>();
            List<Vector2> roadUvs = new List<Vector2>();

            foreach (SplinePoint splinePoint in splinePoints)
            {
                roadVertices.Add(splinePoint.Position + splinePoint.Normal * HalfRoadWidth);
                roadVertices.Add(splinePoint.Position);
                roadVertices.Add(splinePoint.Position - splinePoint.Normal * HalfRoadWidth);
            }

            // Build triangles using the vertex list as if it was a grid
            int width = 3; // Number of points we add in each loop above
            int height = roadVertices.Count / width;

            for (int X = 0; X < width; X++)
            {
                for (int Y = 0; Y < height; Y++)
                {
                    // No triangles on the last row/column of vertices
                    if (X < width - 1 && Y < height - 1)
                    {
                        // Consistent triangle winding is important for consistent face culling

                        int vertexIndex = Y * width + X;

                        // Left triangle of quad
                        roadIndices.Add(vertexIndex);
                        roadIndices.Add(vertexIndex + width + 1);
                        roadIndices.Add(vertexIndex + width);

                        // Right triangle of quad
                        roadIndices.Add(vertexIndex);
                        roadIndices.Add(vertexIndex + 1);
                        roadIndices.Add(vertexIndex + width + 1);
                    }

                    // UVs array needs to be the same size as vertices array
                    roadUvs.Add(new Vector2((float)X / width, (float)Y / height));
                }
            }

            List<ColorMap> colorMaps = new List<ColorMap>
            {
                new ColorMap
                {
                    Name = "diffuse",
                    Color = Color4.DarkGray,
                    Texture = null
                }
            };

            Mesh roadMesh = new Mesh(
                "Road",
                roadVertices.Select(x => new Vertex { Position = x }).ToList(),
                roadIndices,
                colorMaps,
                Matrix4.Identity);

            return roadMesh;
        }
    }
}
