using System;

namespace Voxel.Blocks
{
    [Serializable]
    public class Block_Oak_Leaves : Block
    {

        public Block_Oak_Leaves() : base()
        {

        }
        public override MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.useRenderDataForCollider = true;

            return base.BlockData(chunk, x, y, z, meshData);
        }

        public override bool IsSolid(Direction direction)
        {
            return false;
        }

        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();

            tile.x = 3;
            tile.y = 1;

            return tile;
        }
    }
}

