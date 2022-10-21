using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Tiles
{
    [System.Serializable]
    public struct MapPosition : IEquatable<MapPosition>
    {
        public int X;
        public int Y;

        public MapPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return "X: " + X + " Y: " + Y;
        }

        public static bool operator ==(MapPosition a, MapPosition b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(MapPosition a, MapPosition b)
        {
            return !(a == b);
        }

        public static MapPosition operator +(MapPosition a, MapPosition b)
        {
            return new MapPosition(a.X + b.X, a.Y + b.Y);
        }

        public static MapPosition operator -(MapPosition a, MapPosition b)
        {
            return new MapPosition(a.X - b.X, a.Y - b.Y);
        }
        
        public bool Equals(MapPosition other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is MapPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
