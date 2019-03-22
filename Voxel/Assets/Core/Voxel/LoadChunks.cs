using System.Collections.Generic;
using UnityEngine;
using Voxel;

public class LoadChunks : MonoBehaviour
{
    public World world;
    private int timer = 0;
    private List<WorldPosition> updateList = new List<WorldPosition>();
    private List<WorldPosition> buildList = new List<WorldPosition>();

    private static WorldPosition[] chunkPositions = 
        {
        new WorldPosition( 0, 0,  0), new WorldPosition(-1, 0,  0), new WorldPosition( 0, 0, -1), new WorldPosition( 0, 0,  1), new WorldPosition( 1, 0,  0),
        new WorldPosition(-1, 0, -1), new WorldPosition(-1, 0,  1), new WorldPosition( 1, 0, -1), new WorldPosition( 1, 0,  1), new WorldPosition(-2, 0,  0),
        new WorldPosition( 0, 0, -2), new WorldPosition( 0, 0,  2), new WorldPosition( 2, 0,  0), new WorldPosition(-2, 0, -1), new WorldPosition(-2, 0,  1),
        new WorldPosition(-1, 0, -2), new WorldPosition(-1, 0,  2), new WorldPosition( 1, 0, -2), new WorldPosition( 1, 0,  2), new WorldPosition( 2, 0, -1),
        new WorldPosition( 2, 0,  1), new WorldPosition(-2, 0, -2), new WorldPosition(-2, 0,  2), new WorldPosition( 2, 0, -2), new WorldPosition( 2, 0,  2),
        new WorldPosition(-3, 0,  0), new WorldPosition( 0, 0, -3), new WorldPosition( 0, 0,  3), new WorldPosition( 3, 0,  0), new WorldPosition(-3, 0, -1),
        new WorldPosition(-3, 0,  1), new WorldPosition(-1, 0, -3), new WorldPosition(-1, 0,  3), new WorldPosition( 1, 0, -3), new WorldPosition( 1, 0,  3),
        new WorldPosition( 3, 0, -1), new WorldPosition( 3, 0,  1), new WorldPosition(-3, 0, -2), new WorldPosition(-3, 0,  2), new WorldPosition(-2, 0, -3),
        new WorldPosition(-2, 0,  3), new WorldPosition( 2, 0, -3), new WorldPosition( 2, 0,  3), new WorldPosition( 3, 0, -2), new WorldPosition( 3, 0,  2),
        new WorldPosition(-4, 0,  0), new WorldPosition( 0, 0, -4), new WorldPosition( 0, 0,  4), new WorldPosition( 4, 0,  0), new WorldPosition(-4, 0, -1),
        new WorldPosition(-4, 0,  1), new WorldPosition(-1, 0, -4), new WorldPosition(-1, 0,  4), new WorldPosition( 1, 0, -4), new WorldPosition( 1, 0,  4),
        new WorldPosition( 4, 0, -1), new WorldPosition( 4, 0,  1), new WorldPosition(-3, 0, -3), new WorldPosition(-3, 0,  3), new WorldPosition( 3, 0, -3),
        new WorldPosition( 3, 0,  3), new WorldPosition(-4, 0, -2), new WorldPosition(-4, 0,  2), new WorldPosition(-2, 0, -4), new WorldPosition(-2, 0,  4),
        new WorldPosition( 2, 0, -4), new WorldPosition( 2, 0,  4), new WorldPosition( 4, 0, -2), new WorldPosition( 4, 0,  2), new WorldPosition(-5, 0,  0),
        new WorldPosition(-4, 0, -3), new WorldPosition(-4, 0,  3), new WorldPosition(-3, 0, -4), new WorldPosition(-3, 0,  4), new WorldPosition( 0, 0, -5),
        new WorldPosition( 0, 0,  5), new WorldPosition( 3, 0, -4), new WorldPosition( 3, 0,  4), new WorldPosition( 4, 0, -3), new WorldPosition( 4, 0,  3),
        new WorldPosition( 5, 0,  0), new WorldPosition(-5, 0, -1), new WorldPosition(-5, 0,  1), new WorldPosition(-1, 0, -5), new WorldPosition(-1, 0,  5),
        new WorldPosition( 1, 0, -5), new WorldPosition( 1, 0,  5), new WorldPosition( 5, 0, -1), new WorldPosition( 5, 0,  1), new WorldPosition(-5, 0, -2),
        new WorldPosition(-5, 0,  2), new WorldPosition(-2, 0, -5), new WorldPosition(-2, 0,  5), new WorldPosition( 2, 0, -5), new WorldPosition( 2, 0,  5),
        new WorldPosition( 5, 0, -2), new WorldPosition( 5, 0,  2), new WorldPosition(-4, 0, -4), new WorldPosition(-4, 0,  4), new WorldPosition( 4, 0, -4),
        new WorldPosition( 4, 0,  4), new WorldPosition(-5, 0, -3), new WorldPosition(-5, 0,  3), new WorldPosition(-3, 0, -5), new WorldPosition(-3, 0,  5),
        new WorldPosition( 3, 0, -5), new WorldPosition( 3, 0,  5), new WorldPosition( 5, 0, -3), new WorldPosition( 5, 0,  3), new WorldPosition(-6, 0,  0),
        new WorldPosition( 0, 0, -6), new WorldPosition( 0, 0,  6), new WorldPosition( 6, 0,  0), new WorldPosition(-6, 0, -1), new WorldPosition(-6, 0,  1),
        new WorldPosition(-1, 0, -6), new WorldPosition(-1, 0,  6), new WorldPosition( 1, 0, -6), new WorldPosition( 1, 0,  6), new WorldPosition( 6, 0, -1),
        new WorldPosition( 6, 0,  1), new WorldPosition(-6, 0, -2), new WorldPosition(-6, 0,  2), new WorldPosition(-2, 0, -6), new WorldPosition(-2, 0,  6),
        new WorldPosition( 2, 0, -6), new WorldPosition( 2, 0,  6), new WorldPosition( 6, 0, -2), new WorldPosition( 6, 0,  2), new WorldPosition(-5, 0, -4),
        new WorldPosition(-5, 0,  4), new WorldPosition(-4, 0, -5), new WorldPosition(-4, 0,  5), new WorldPosition( 4, 0, -5), new WorldPosition( 4, 0,  5),
        new WorldPosition( 5, 0, -4), new WorldPosition( 5, 0,  4), new WorldPosition(-6, 0, -3), new WorldPosition(-6, 0,  3), new WorldPosition(-3, 0, -6),
        new WorldPosition(-3, 0,  6), new WorldPosition( 3, 0, -6), new WorldPosition( 3, 0,  6), new WorldPosition( 6, 0, -3), new WorldPosition( 6, 0,  3),
        new WorldPosition(-7, 0,  0), new WorldPosition( 0, 0, -7), new WorldPosition( 0, 0,  7), new WorldPosition( 7, 0,  0), new WorldPosition(-7, 0, -1),
        new WorldPosition(-7, 0,  1), new WorldPosition(-5, 0, -5), new WorldPosition(-5, 0,  5), new WorldPosition(-1, 0, -7), new WorldPosition(-1, 0,  7),
        new WorldPosition( 1, 0, -7), new WorldPosition( 1, 0,  7), new WorldPosition( 5, 0, -5), new WorldPosition( 5, 0,  5), new WorldPosition( 7, 0, -1),
        new WorldPosition( 7, 0,  1), new WorldPosition(-6, 0, -4), new WorldPosition(-6, 0,  4), new WorldPosition(-4, 0, -6), new WorldPosition(-4, 0,  6),
        new WorldPosition( 4, 0, -6), new WorldPosition( 4, 0,  6), new WorldPosition( 6, 0, -4), new WorldPosition( 6, 0,  4), new WorldPosition(-7, 0, -2),
        new WorldPosition(-7, 0,  2), new WorldPosition(-2, 0, -7), new WorldPosition(-2, 0,  7), new WorldPosition( 2, 0, -7), new WorldPosition( 2, 0,  7),
        new WorldPosition( 7, 0, -2), new WorldPosition( 7, 0,  2), new WorldPosition(-7, 0, -3), new WorldPosition(-7, 0,  3), new WorldPosition(-3, 0, -7),
        new WorldPosition(-3, 0,  7), new WorldPosition( 3, 0, -7), new WorldPosition( 3, 0,  7), new WorldPosition( 7, 0, -3), new WorldPosition( 7, 0,  3),
        new WorldPosition(-6, 0, -5), new WorldPosition(-6, 0,  5), new WorldPosition(-5, 0, -6), new WorldPosition(-5, 0,  6), new WorldPosition( 5, 0, -6),
        new WorldPosition( 5, 0,  6), new WorldPosition( 6, 0, -5), new WorldPosition( 6, 0,  5)
        };

    private void Update()
    {
        if (DeleteChunks())
            return;

        FindChunksToLoad();
        LoadAndRenderChunks();
    }

    private void FindChunksToLoad()
    {
        WorldPosition playerPos = new WorldPosition(
            Mathf.FloorToInt(transform.position.x / Chunk.chunkSize) * Chunk.chunkSize,
            Mathf.FloorToInt(transform.position.y / Chunk.chunkSize) * Chunk.chunkSize,
            Mathf.FloorToInt(transform.position.z / Chunk.chunkSize) * Chunk.chunkSize
            );

        if (updateList.Count == 0)
        {
            for (int i = 0; i < chunkPositions.Length; i++)
            {
                WorldPosition newChunkPos = new WorldPosition(chunkPositions[i].x * Chunk.chunkSize + playerPos.x, 0, chunkPositions[i].z * Chunk.chunkSize + playerPos.z);
                Chunk newChunk = world.GetChunk(newChunkPos.x, newChunkPos.y, newChunkPos.z);
                if (newChunk != null
                    && (newChunk.rendered || updateList.Contains(newChunkPos)))
                    continue;

                for (int y = -8; y < 8; y++)
                {
                    for (int x = newChunkPos.x - Chunk.chunkSize; x <= newChunkPos.x + Chunk.chunkSize; x += Chunk.chunkSize)
                    {
                        for (int z = newChunkPos.z - Chunk.chunkSize; z <= newChunkPos.z + Chunk.chunkSize; z += Chunk.chunkSize)
                        {
                            buildList.Add(new WorldPosition(x, y * Chunk.chunkSize, z));
                        }
                    }
                    updateList.Add(new WorldPosition(newChunkPos.x, y * Chunk.chunkSize, newChunkPos.z));
                }
                return;
            }
        }
    }

    private void BuildChunk(WorldPosition pos)
    {
        if (world.GetChunk(pos.x, pos.y, pos.z) == null)
            world.CreateChunk(pos.x, pos.y, pos.z);
    }

    private bool DeleteChunks()
    {
        if (timer == 10)
        {
            var chunksToDelete = new List<WorldPosition>();
            foreach (var chunk in world.chunks)
            {
                float distance = Vector3.Distance(
                    new Vector3(chunk.Value.position.x, 0, chunk.Value.position.z),
                    new Vector3(transform.position.x, 0, transform.position.z));
                if (distance > 256)
                    chunksToDelete.Add(chunk.Key);
            }
            foreach (var chunk in chunksToDelete)
                world.DestroyChunk(chunk.x, chunk.y, chunk.z);
            timer = 0;
            return true;
        }
        timer++;
        return false;
    }

    private void LoadAndRenderChunks()
    {
        if (buildList.Count != 0)
        {
            for (int i = 0; i < buildList.Count && i < 8; i++)
            {
                BuildChunk(buildList[0]);
                buildList.RemoveAt(0);
            }
            return;
        }
        if (updateList.Count != 0)
        {
            Chunk chunk = world.GetChunk(updateList[0].x, updateList[0].y, updateList[0].z);
            if (chunk != null)
                chunk.update = true;
            updateList.RemoveAt(0);
        }
    }
}
