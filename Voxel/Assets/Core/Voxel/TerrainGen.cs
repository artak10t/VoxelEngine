using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxel;
using Voxel.Blocks;
using Noise;

public class TerrainGen
{
    float stoneBaseHeight = -48;
    float stoneBaseNoise = 0.01f;
    float stoneBaseNoiseHeight = 8;
    float stoneMountainHeight = 48;
    float stoneMountainFrequency = 0.008f;
    float stoneMinHeight = -12;

    float dirtBaseHeight = 5;
    float dirtNoise = 0.02f;
    float dirtNoiseHeight = 3;

    float caveFrequency = 0.025f;
    int caveSize = 7;

    float treeFrequency = 0.2f;
    int treeDensity = 3;

    public Chunk ChunkGen(Chunk chunk)
    {
        for (int x = chunk.position.x - 3; x < chunk.position.x + Chunk.chunkSize + 3; x++)
        {
            for (int z = chunk.position.z - 3; z < chunk.position.z + Chunk.chunkSize + 3; z++)
            {
                chunk = ChunkColumnGen(chunk, x, z);
            }
        }
        return chunk;
    }

    public Chunk ChunkColumnGen(Chunk chunk, int x, int z)
    {
        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        stoneHeight += GetSimplexNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        if (stoneHeight < stoneMinHeight)
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);

        stoneHeight += GetSimplexNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));
        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += GetSimplexNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        for (int y = chunk.position.y - 8; y < chunk.position.y + Chunk.chunkSize; y++)
        {
            int caveChance = GetSimplexNoise(x, y, z, caveFrequency, 75);
            if (y <= stoneHeight && caveSize < caveChance)
            {
                SetBlock(x, y, z, new Block_Stone(), chunk);
            }
            else if (y <= dirtHeight && caveSize < caveChance)
            {
                SetBlock(x, y, z, new Block_Grass(), chunk);

                if (y == dirtHeight && GetSimplexNoise(x, 0, z, treeFrequency, 100) < treeDensity)
                    CreateTree(x, y + 1, z, chunk);
            }
            else
            {
                SetBlock(x, y, z, new Block_Air(), chunk);
            }
        }
        return chunk;
    }

    void CreateTree(int x, int y, int z, Chunk chunk)
    {
        for (int xi = -2; xi <= 2; xi++)
        {
            for (int yi = 4; yi <= 8; yi++)
            {
                for (int zi = -2; zi <= 2; zi++)
                {
                    SetBlock(x + xi, y + yi, z + zi, new Block_Oak_Leaves(), chunk, true);
                }
            }
        }

        for (int yt = 0; yt < 6; yt++)
        {
            SetBlock(x, y + yt, z, new Block_Oak(), chunk, true);
        }
    }

    public static void SetBlock(int x, int y, int z, Block block, Chunk chunk, bool replaceBlocks = false)
    {
        x -= chunk.position.x;
        y -= chunk.position.y;
        z -= chunk.position.z;
        if (Chunk.InRange(x) && Chunk.InRange(y) && Chunk.InRange(z))
        {
            if (replaceBlocks || chunk.blocks[x, y, z] == null)
                chunk.SetBlock(x, y, z, block);
        }
    }

    public int GetSimplexNoise(int x, int y, int z, float scale, int max)
    {
        return Mathf.FloorToInt((SimplexNoise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }
}
