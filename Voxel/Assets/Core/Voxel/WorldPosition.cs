using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Voxel
{
    [Serializable]
    public struct WorldPosition
    {
        public int x, y, z;

        public WorldPosition(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WorldPosition))
                return false;

            WorldPosition pos = (WorldPosition)obj;
            if (pos.x != x || pos.y != y || pos.z != z)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 47;
                hash = hash * 227 + x.GetHashCode();
                hash = hash * 227 + y.GetHashCode();
                hash = hash * 227 + z.GetHashCode();
                return hash;
            }
        }
    }
}
