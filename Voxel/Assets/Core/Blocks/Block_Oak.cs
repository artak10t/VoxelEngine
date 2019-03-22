using System;

namespace Voxel.Blocks
{
    [Serializable]
    public class Block_Oak : Block
    {

        public Block_Oak() : base()
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
            switch (direction)
            {
                case Direction.Up:
                    tile.x = 2;
                    tile.y = 1;
                    return tile;

                case Direction.Down:
                    tile.x = 2;
                    tile.y = 1;
                    return tile;
            }

            tile.x = 1;
            tile.y = 1;

            return tile;
        }
    }
}

