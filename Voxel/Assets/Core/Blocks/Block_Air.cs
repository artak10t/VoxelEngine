using UnityEngine;
using System;

namespace Voxel.Blocks
{
    [Serializable]
    public class Block_Air : Block
    {
        public Block_Air() : base()
        {

        }

        public override MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            return meshData;
        }

        public override bool IsSolid(Direction direction)
        {
            return false;
        }
    }
}
