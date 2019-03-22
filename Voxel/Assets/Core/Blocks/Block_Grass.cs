using System;

namespace Voxel.Blocks
{
    [Serializable]
    public class Block_Grass : Block
    {
        private bool dirt = false;

        public Block_Grass() : base()
        {

        }
        public override MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.useRenderDataForCollider = true;

            if (chunk.GetBlock(x, y + 1, z).IsSolid(Direction.Down))
            {
                dirt = true;
            }

            return base.BlockData(chunk, x, y, z, meshData);
        }

        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();

            if (!dirt)
            {
                switch (direction)
                {
                    case Direction.Up:
                        tile.x = 2;
                        tile.y = 0;
                        return tile;

                    case Direction.Down:
                        tile.x = 1;
                        tile.y = 0;
                        return tile;
                }

                tile.x = 3;
                tile.y = 0;
            }
            else
            {
                tile.x = 1;
                tile.y = 0;
            }
            return tile;
        }
    }
}

