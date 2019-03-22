using UnityEngine;
using System;

namespace Voxel.Blocks
{

    [Serializable]
    public class Block_Stone : Block
    {
        public Block_Stone() : base()
        {

        }

        public override MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.useRenderDataForCollider = true;

            return base.BlockData(chunk, x, y, z, meshData);
        }

        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();
            tile.x = 0;
            tile.y = 1;
            return tile;
        }
    }
}

