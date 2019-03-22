using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using Voxel.Blocks;

namespace Voxel
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {
        public Block[,,] blocks = new Block[chunkSize,chunkSize,chunkSize];
        public static int chunkSize = 16;
        public bool update = false;
        public bool rendered;
        public World world;
        public WorldPosition position;

        private MeshFilter meshFilter;
        private MeshCollider meshCollider;

        void Start()
        {
            meshFilter = gameObject.GetComponent<MeshFilter>();
            meshCollider = gameObject.GetComponent<MeshCollider>();
        }

        void Update()
        {
            if (update)
            {
                update = false;
                UpdateChunk();
                rendered = true;
            }
        }

        public Block GetBlock(int x, int y, int z)
        {
                if (InRange(x) && InRange(y) && InRange(z))
                    return blocks[x, y, z];

                return world.GetBlock(position.x + x, position.y + y, position.z + z);
        }

        public void SetBlock(int x, int y, int z, Block block)
        {
                if (InRange(x) && InRange(y) && InRange(z))
                {
                    blocks[x, y, z] = block;
                }
                else
                {
                    world.SetBlock(position.x + x, position.y + y, position.z + z, block);
                }
        }

        public void SetBlocksUnmodified()
        {
            foreach (Block block in blocks)
            {
                block.changed = false;
            }
        }

        private void UpdateChunk()
        {
            MeshData meshData = new MeshData();

            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        meshData = blocks[x, y, z].BlockData(this, x, y, z, meshData);
                    }
                }
            }
            RenderMesh(meshData);
        }

        private void RenderMesh(MeshData meshData)
        {
            meshFilter.mesh.Clear();
            meshFilter.mesh.vertices = meshData.vertices.ToArray();
            meshFilter.mesh.triangles = meshData.triangles.ToArray();
            meshFilter.mesh.colors = meshData.colors.ToArray();

            meshFilter.mesh.uv = meshData.uv.ToArray();
            meshFilter.mesh.RecalculateNormals();

            meshCollider.sharedMesh = null;
            Mesh mesh = new Mesh();
            mesh.vertices = meshData.colliderVertices.ToArray();
            mesh.triangles = meshData.colliderTriangles.ToArray();
            mesh.RecalculateNormals();

            meshCollider.sharedMesh = mesh;

        }

        public static bool InRange(int index)
        {
            if (index < 0 || index >= chunkSize)
                return false;

            return true;
        }

    }
}