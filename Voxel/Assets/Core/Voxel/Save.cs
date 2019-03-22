using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Voxel;
using Voxel.Blocks;

[Serializable]
public class Save
{
    public Dictionary<WorldPosition, Block> blocks = new Dictionary<WorldPosition, Block>();

    public Save(Chunk chunk)
    {
        for (int x = 0; x < Chunk.chunkSize; x++)
        {
            for (int y = 0; y < Chunk.chunkSize; y++)
            {
                for (int z = 0; z < Chunk.chunkSize; z++)
                {
                    if (!chunk.blocks[x, y, z].changed)
                        continue;

                    WorldPosition pos = new WorldPosition(x, y, z);
                    blocks.Add(pos, chunk.blocks[x, y, z]);
                }
            }
        }
    }
}