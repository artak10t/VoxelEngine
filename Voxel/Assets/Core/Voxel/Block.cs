using UnityEngine;
using System;
using System.Threading;

namespace Voxel.Blocks
{
    public enum Direction { North, East, South, West, Up, Down }

    [Serializable]
    public class Block
    {
        private const float tileSize = 0.25f;
        private Color ssaoColor = Color.gray;
        public bool changed = true;

        public Block()
        {
            if (DeveloperScreen.DebugSSAO)
                ssaoColor = Color.blue;
        }

        public virtual bool IsSolid(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return true;
                case Direction.East:
                    return true;
                case Direction.South:
                    return true;
                case Direction.West:
                    return true;
                case Direction.Up:
                    return true;
                case Direction.Down:
                    return true;
            }
            return false;
        }

        #region UV

        public virtual Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();
            tile.x = 0;
            tile.y = 0;
            return tile;
        }

        public virtual Vector2[] FaceUVs(Direction direction)
        {
            Vector2[] UVs = new Vector2[4];
            Tile tilePos = TexturePosition(direction);
            UVs[0] = new Vector2(tileSize * tilePos.x + tileSize, tileSize * tilePos.y);
            UVs[1] = new Vector2(tileSize * tilePos.x + tileSize, tileSize * tilePos.y + tileSize);
            UVs[2] = new Vector2(tileSize * tilePos.x, tileSize * tilePos.y + tileSize);
            UVs[3] = new Vector2(tileSize * tilePos.x, tileSize * tilePos.y);

            return UVs;
        }

        #endregion

        public virtual MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.Down))
            {
                meshData = FaceDataUp(chunk, x, y, z, meshData);
                CheckSSAO(chunk, meshData, x, y, z, Direction.Up);
            }

            if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.Up))
            {
                meshData = FaceDataDown(chunk, x, y, z, meshData);
                CheckSSAO(chunk, meshData, x, y, z, Direction.Down);
            }

            if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.South))
            {
                meshData = FaceDataNorth(chunk, x, y, z, meshData);
                CheckSSAO(chunk, meshData, x, y, z, Direction.North);
            }

            if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.North))
            {
                meshData = FaceDataSouth(chunk, x, y, z, meshData);
                CheckSSAO(chunk, meshData, x, y, z, Direction.South);
            }

            if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.West))
            {
                meshData = FaceDataEast(chunk, x, y, z, meshData);
                CheckSSAO(chunk, meshData, x, y, z, Direction.East);
            }

            if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.East))
            {
                meshData = FaceDataWest(chunk, x, y, z, meshData);
                CheckSSAO(chunk, meshData, x, y, z, Direction.West);
            }

            meshData.useRenderDataForCollider = true;
            return meshData;
        }

        #region BlockMesh

        protected virtual MeshData FaceDataUp(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.uv.AddRange(FaceUVs(Direction.Up));

            meshData.AddQuadTriangles();
            return meshData;
        }

        protected virtual MeshData FaceDataDown(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.uv.AddRange(FaceUVs(Direction.Down));

            meshData.AddQuadTriangles();
            return meshData;
        }

        protected virtual MeshData FaceDataNorth(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.uv.AddRange(FaceUVs(Direction.North));

            meshData.AddQuadTriangles();
            return meshData;
        }

        protected virtual MeshData FaceDataEast(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.uv.AddRange(FaceUVs(Direction.East));

            meshData.AddQuadTriangles();
            return meshData;
        }

        protected virtual MeshData FaceDataSouth(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.uv.AddRange(FaceUVs(Direction.South));

            meshData.AddQuadTriangles();
            return meshData;
        }

        protected virtual MeshData FaceDataWest(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.uv.AddRange(FaceUVs(Direction.West));

            meshData.AddQuadTriangles();
            return meshData;
        }

        #endregion

        private void AddSSAO(MeshData meshData, bool ssaoSouth, bool ssaoNorth, bool ssaoWest, bool ssaoEast, bool ssaoNorthWest, bool ssaoNorthEast, bool ssaoSouthWest, bool ssaoSouthEast)
        {
            if (ssaoEast || ssaoSouth || ssaoSouthWest)
                meshData.AddColor(ssaoColor);
            else
                meshData.AddColor(Color.white);

            if (ssaoSouth || ssaoWest || ssaoSouthEast)
                meshData.AddColor(ssaoColor);
            else
                meshData.AddColor(Color.white);

            if (ssaoNorth || ssaoWest || ssaoNorthEast)
                meshData.AddColor(ssaoColor);
            else
                meshData.AddColor(Color.white);

            if (ssaoNorth || ssaoEast || ssaoNorthWest)
                meshData.AddColor(ssaoColor);
            else
                meshData.AddColor(Color.white);
        }

        private void CheckSSAO(Chunk chunk, MeshData meshData, int x, int y, int z, Direction direction)
        {
            bool ssaoSouth, ssaoNorth, ssaoWest, ssaoEast, ssaoNorthWest, ssaoNorthEast, ssaoSouthWest, ssaoSouthEast;

            switch (direction)
            {
                case Direction.Up:

                    if (chunk.GetBlock(x, y + 1, z + 1).IsSolid(Direction.South))
                        ssaoSouth = true;
                    else ssaoSouth = false;

                    if (chunk.GetBlock(x, y + 1, z - 1).IsSolid(Direction.North))
                        ssaoNorth = true;
                    else ssaoNorth = false;

                    if (chunk.GetBlock(x + 1, y + 1, z).IsSolid(Direction.West))
                        ssaoWest = true;
                    else ssaoWest = false;

                    if (chunk.GetBlock(x - 1, y + 1, z).IsSolid(Direction.East))
                        ssaoEast = true;
                    else ssaoEast = false;

                    if (chunk.GetBlock(x - 1, y + 1, z + 1).IsSolid(Direction.North) || chunk.GetBlock(x - 1, y + 1, z + 1).IsSolid(Direction.East))
                        ssaoSouthWest = true;
                    else ssaoSouthWest = false;

                    if (chunk.GetBlock(x + 1, y + 1, z + 1).IsSolid(Direction.North) || chunk.GetBlock(x + 1, y + 1, z + 1).IsSolid(Direction.West))
                        ssaoSouthEast = true;
                    else ssaoSouthEast = false;

                    if (chunk.GetBlock(x - 1, y + 1, z - 1).IsSolid(Direction.South) || chunk.GetBlock(x - 1, y + 1, z - 1).IsSolid(Direction.East))
                        ssaoNorthWest = true;
                    else ssaoNorthWest = false;

                    if (chunk.GetBlock(x + 1, y + 1, z - 1).IsSolid(Direction.South) || chunk.GetBlock(x + 1, y + 1, z - 1).IsSolid(Direction.West))
                        ssaoNorthEast = true;
                    else ssaoNorthEast = false;

                    AddSSAO(meshData, ssaoSouth, ssaoNorth, ssaoWest, ssaoEast, ssaoNorthWest, ssaoNorthEast, ssaoSouthWest, ssaoSouthEast);
                    return;

                case Direction.Down:
                    if (chunk.GetBlock(x, y - 1, z - 1).IsSolid(Direction.South))
                        ssaoSouth = true;
                    else ssaoSouth = false;

                    if (chunk.GetBlock(x, y - 1, z + 1).IsSolid(Direction.North))
                        ssaoNorth = true;
                    else ssaoNorth = false;

                    if (chunk.GetBlock(x + 1, y - 1, z).IsSolid(Direction.West))
                        ssaoWest = true;
                    else ssaoWest = false;

                    if (chunk.GetBlock(x - 1, y - 1, z).IsSolid(Direction.East))
                        ssaoEast = true;
                    else ssaoEast = false;

                    if (chunk.GetBlock(x - 1, y - 1, z - 1).IsSolid(Direction.North) || chunk.GetBlock(x - 1, y - 1, z - 1).IsSolid(Direction.East))
                        ssaoSouthWest = true;
                    else ssaoSouthWest = false;

                    if (chunk.GetBlock(x + 1, y - 1, z - 1).IsSolid(Direction.North) || chunk.GetBlock(x + 1, y - 1, z - 1).IsSolid(Direction.West))
                        ssaoSouthEast = true;
                    else ssaoSouthEast = false;

                    if (chunk.GetBlock(x - 1, y - 1, z + 1).IsSolid(Direction.South) || chunk.GetBlock(x - 1, y - 1, z + 1).IsSolid(Direction.East))
                        ssaoNorthWest = true;
                    else ssaoNorthWest = false;

                    if (chunk.GetBlock(x + 1, y - 1, z + 1).IsSolid(Direction.South) || chunk.GetBlock(x + 1, y - 1, z + 1).IsSolid(Direction.West))
                        ssaoNorthEast = true;
                    else ssaoNorthEast = false;

                    AddSSAO(meshData, ssaoSouth, ssaoNorth, ssaoWest, ssaoEast, ssaoNorthWest, ssaoNorthEast, ssaoSouthWest, ssaoSouthEast);
                    return;

                case Direction.North:
                    if (chunk.GetBlock(x + 1, y, z + 1).IsSolid(Direction.South))
                        ssaoSouth = true;
                    else ssaoSouth = false;

                    if (chunk.GetBlock(x - 1, y, z + 1).IsSolid(Direction.North))
                        ssaoNorth = true;
                    else ssaoNorth = false;

                    if (chunk.GetBlock(x, y + 1, z + 1).IsSolid(Direction.West))
                        ssaoWest = true;
                    else ssaoWest = false;

                    if (chunk.GetBlock(x, y - 1, z + 1).IsSolid(Direction.East))
                        ssaoEast = true;
                    else ssaoEast = false;

                    if (chunk.GetBlock(x + 1, y - 1, z + 1).IsSolid(Direction.North) || chunk.GetBlock(x + 1, y - 1, z + 1).IsSolid(Direction.East))
                        ssaoSouthWest = true;
                    else ssaoSouthWest = false;

                    if (chunk.GetBlock(x + 1, y + 1, z + 1).IsSolid(Direction.North) || chunk.GetBlock(x + 1, y + 1, z + 1).IsSolid(Direction.West))
                        ssaoSouthEast = true;
                    else ssaoSouthEast = false;

                    if (chunk.GetBlock(x - 1, y - 1, z + 1).IsSolid(Direction.South) || chunk.GetBlock(x - 1, y - 1, z + 1).IsSolid(Direction.East))
                        ssaoNorthWest = true;
                    else ssaoNorthWest = false;

                    if (chunk.GetBlock(x - 1, y + 1, z + 1).IsSolid(Direction.South) || chunk.GetBlock(x - 1, y + 1, z + 1).IsSolid(Direction.West))
                        ssaoNorthEast = true;
                    else ssaoNorthEast = false;

                    AddSSAO(meshData, ssaoSouth, ssaoNorth, ssaoWest, ssaoEast, ssaoNorthWest, ssaoNorthEast, ssaoSouthWest, ssaoSouthEast);
                    return;

                case Direction.South:
                    if (chunk.GetBlock(x - 1, y, z - 1).IsSolid(Direction.South))
                        ssaoSouth = true;
                    else ssaoSouth = false;

                    if (chunk.GetBlock(x + 1, y, z - 1).IsSolid(Direction.North))
                        ssaoNorth = true;
                    else ssaoNorth = false;

                    if (chunk.GetBlock(x, y + 1, z - 1).IsSolid(Direction.West))
                        ssaoWest = true;
                    else ssaoWest = false;

                    if (chunk.GetBlock(x, y - 1, z - 1).IsSolid(Direction.East))
                        ssaoEast = true;
                    else ssaoEast = false;

                    if (chunk.GetBlock(x - 1, y - 1, z - 1).IsSolid(Direction.North) || chunk.GetBlock(x - 1, y - 1, z - 1).IsSolid(Direction.East))
                        ssaoSouthWest = true;
                    else ssaoSouthWest = false;

                    if (chunk.GetBlock(x - 1, y + 1, z - 1).IsSolid(Direction.North) || chunk.GetBlock(x - 1, y + 1, z - 1).IsSolid(Direction.West))
                        ssaoSouthEast = true;
                    else ssaoSouthEast = false;

                    if (chunk.GetBlock(x + 1, y - 1, z - 1).IsSolid(Direction.South) || chunk.GetBlock(x + 1, y - 1, z - 1).IsSolid(Direction.East))
                        ssaoNorthWest = true;
                    else ssaoNorthWest = false;

                    if (chunk.GetBlock(x + 1, y + 1, z - 1).IsSolid(Direction.South) || chunk.GetBlock(x + 1, y + 1, z - 1).IsSolid(Direction.West))
                        ssaoNorthEast = true;
                    else ssaoNorthEast = false;

                    AddSSAO(meshData, ssaoSouth, ssaoNorth, ssaoWest, ssaoEast, ssaoNorthWest, ssaoNorthEast, ssaoSouthWest, ssaoSouthEast);
                    return;

                case Direction.East:
                    if (chunk.GetBlock(x + 1, y, z - 1).IsSolid(Direction.South))
                        ssaoSouth = true;
                    else ssaoSouth = false;

                    if (chunk.GetBlock(x + 1, y, z + 1).IsSolid(Direction.North))
                        ssaoNorth = true;
                    else ssaoNorth = false;

                    if (chunk.GetBlock(x + 1, y + 1, z).IsSolid(Direction.West))
                        ssaoWest = true;
                    else ssaoWest = false;

                    if (chunk.GetBlock(x + 1, y - 1, z).IsSolid(Direction.East))
                        ssaoEast = true;
                    else ssaoEast = false;

                    if (chunk.GetBlock(x + 1, y - 1, z - 1).IsSolid(Direction.North) || chunk.GetBlock(x + 1, y - 1, z - 1).IsSolid(Direction.East))
                        ssaoSouthWest = true;
                    else ssaoSouthWest = false;

                    if (chunk.GetBlock(x + 1, y + 1, z - 1).IsSolid(Direction.North) || chunk.GetBlock(x + 1, y + 1, z - 1).IsSolid(Direction.West))
                        ssaoSouthEast = true;
                    else ssaoSouthEast = false;

                    if (chunk.GetBlock(x + 1, y - 1, z + 1).IsSolid(Direction.South) || chunk.GetBlock(x + 1, y - 1, z + 1).IsSolid(Direction.East))
                        ssaoNorthWest = true;
                    else ssaoNorthWest = false;

                    if (chunk.GetBlock(x + 1, y + 1, z + 1).IsSolid(Direction.South) || chunk.GetBlock(x + 1, y + 1, z + 1).IsSolid(Direction.West))
                        ssaoNorthEast = true;
                    else ssaoNorthEast = false;

                    AddSSAO(meshData, ssaoSouth, ssaoNorth, ssaoWest, ssaoEast, ssaoNorthWest, ssaoNorthEast, ssaoSouthWest, ssaoSouthEast);
                    return;

                case Direction.West:
                    if (chunk.GetBlock(x - 1, y, z + 1).IsSolid(Direction.South))
                        ssaoSouth = true;
                    else ssaoSouth = false;

                    if (chunk.GetBlock(x - 1, y, z - 1).IsSolid(Direction.North))
                        ssaoNorth = true;
                    else ssaoNorth = false;

                    if (chunk.GetBlock(x - 1, y + 1, z).IsSolid(Direction.West))
                        ssaoWest = true;
                    else ssaoWest = false;

                    if (chunk.GetBlock(x - 1, y - 1, z).IsSolid(Direction.East))
                        ssaoEast = true;
                    else ssaoEast = false;

                    if (chunk.GetBlock(x - 1, y - 1, z + 1).IsSolid(Direction.North) || chunk.GetBlock(x - 1, y - 1, z + 1).IsSolid(Direction.East))
                        ssaoSouthWest = true;
                    else ssaoSouthWest = false;

                    if (chunk.GetBlock(x - 1, y + 1, z + 1).IsSolid(Direction.North) || chunk.GetBlock(x - 1, y + 1, z + 1).IsSolid(Direction.West))
                        ssaoSouthEast = true;
                    else ssaoSouthEast = false;

                    if (chunk.GetBlock(x - 1, y - 1, z - 1).IsSolid(Direction.South) || chunk.GetBlock(x - 1, y - 1, z - 1).IsSolid(Direction.East))
                        ssaoNorthWest = true;
                    else ssaoNorthWest = false;

                    if (chunk.GetBlock(x - 1, y + 1, z - 1).IsSolid(Direction.South) || chunk.GetBlock(x - 1, y + 1, z - 1).IsSolid(Direction.West))
                        ssaoNorthEast = true;
                    else ssaoNorthEast = false;

                    AddSSAO(meshData, ssaoSouth, ssaoNorth, ssaoWest, ssaoEast, ssaoNorthWest, ssaoNorthEast, ssaoSouthWest, ssaoSouthEast);
                    return;
            }
        }

    }

    public struct Tile
    {
        public int x;
        public int y;
    }
}
