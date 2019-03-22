using System.Collections.Generic;
using UnityEngine;

namespace Voxel
{
    public class MeshData
    {
        public List<Vector3> vertices = new List<Vector3>();
        public List<int> triangles = new List<int>();
        public List<Vector2> uv = new List<Vector2>();
        public List<Color> colors = new List<Color>();

        public List<Vector3> colliderVertices = new List<Vector3>();
        public List<int> colliderTriangles = new List<int>();

        public bool useRenderDataForCollider;

        public MeshData()
        {

        }

        public void AddQuadTriangles()
        {
            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 1);

            if (useRenderDataForCollider)
            {
                colliderTriangles.Add(colliderVertices.Count - 4);
                colliderTriangles.Add(colliderVertices.Count - 3);
                colliderTriangles.Add(colliderVertices.Count - 2);
                colliderTriangles.Add(colliderVertices.Count - 4);
                colliderTriangles.Add(colliderVertices.Count - 2);
                colliderTriangles.Add(colliderVertices.Count - 1);
            }
        }

        public void AddColor(Color color)
        {
            colors.Add(color);
        }

        public void AddVertex(Vector3 vertex)
        {
            vertices.Add(vertex);
            if (useRenderDataForCollider)
            {
                colliderVertices.Add(vertex);
            }
        }

        public void AddTriangle(int tri)
        {
            triangles.Add(tri);
            if (useRenderDataForCollider)
            {
                colliderTriangles.Add(tri - (vertices.Count - colliderVertices.Count));
            }
        }
    }
}
