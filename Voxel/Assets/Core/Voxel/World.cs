using System.Collections.Generic;
using UnityEngine;
using Voxel.Blocks;

namespace Voxel
{
    public class World : MonoBehaviour
    {
        public string worldName = "world";
        public GameObject chunkPrefab;
        public Dictionary<WorldPosition, Chunk> chunks = new Dictionary<WorldPosition, Chunk>();

        public void CreateChunk(int x, int y, int z)
        {
            WorldPosition worldPosition = new WorldPosition(x, y, z);
            GameObject newChunkObject = Instantiate(chunkPrefab, new Vector3(x, y, z), Quaternion.Euler(Vector3.zero)) as GameObject;
            Chunk newChunk = newChunkObject.GetComponent<Chunk>();
            newChunk.position = worldPosition;
            newChunk.world = this;
            chunks.Add(worldPosition, newChunk);

            var terrainGen = new TerrainGen();
            newChunk = terrainGen.ChunkGen(newChunk);

            newChunk.SetBlocksUnmodified();
            Serialization.Load(newChunk);
        }

        public void DestroyChunk(int x, int y, int z)
        {
            Chunk chunk = null;
            if (chunks.TryGetValue(new WorldPosition(x, y, z), out chunk))
            {
                Serialization.SaveChunk(chunk);
                Object.Destroy(chunk.gameObject);
                chunks.Remove(new WorldPosition(x, y, z));
            }
        }

        public Chunk GetChunk(int x, int y, int z)
        {
            WorldPosition pos = new WorldPosition();
            float multiple = Chunk.chunkSize;
            pos.x = Mathf.FloorToInt(x / multiple) * Chunk.chunkSize;
            pos.y = Mathf.FloorToInt(y / multiple) * Chunk.chunkSize;
            pos.z = Mathf.FloorToInt(z / multiple) * Chunk.chunkSize;
            Chunk containerChunk = null;
            chunks.TryGetValue(pos, out containerChunk);

            return containerChunk;
        }

        public Block GetBlock(int x, int y, int z)
        {
            Chunk containerChunk = GetChunk(x, y, z);
            if (containerChunk != null)
            {
                Block block = containerChunk.GetBlock(x - containerChunk.position.x, y - containerChunk.position.y, z - containerChunk.position.z);
                return block;
            }
            else
            {
                return new Block_Air();
            }

        }

        public void SetBlock(int x, int y, int z, Block block)
        {
            Chunk chunk = GetChunk(x, y, z);

            if (chunk != null)
            {
                chunk.SetBlock(x - chunk.position.x, y - chunk.position.y, z - chunk.position.z, block);
                chunk.update = true;

                UpdateIfEqual(x - chunk.position.x, 0, new WorldPosition(x - 1, y, z));
                UpdateIfEqual(x - chunk.position.x, Chunk.chunkSize - 1, new WorldPosition(x + 1, y, z));
                UpdateIfEqual(y - chunk.position.y, 0, new WorldPosition(x, y - 1, z));
                UpdateIfEqual(y - chunk.position.y, Chunk.chunkSize - 1, new WorldPosition(x, y + 1, z));
                UpdateIfEqual(z - chunk.position.z, 0, new WorldPosition(x, y, z - 1));
                UpdateIfEqual(z - chunk.position.z, Chunk.chunkSize - 1, new WorldPosition(x, y, z + 1));
            }
        }

        void UpdateIfEqual(int value1, int value2, WorldPosition pos)
        {
            if (value1 == value2)
            {
                Chunk chunk = GetChunk(pos.x, pos.y, pos.z);
                if (chunk != null)
                    chunk.update = true;
            }
        }
    }
}
