using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Tiles
{
    [System.Serializable]
    public class PathNode
    {
        private MapPosition mapPosition;
        private int _gCost;
        private int _hCost;
        private int _fCost;
        private PathNode _previousNode;
        private bool _isWalkable = true;

        public MapPosition MapPosition => mapPosition;
        public int GCost => _gCost;
        public int HCost => _hCost;
        public int FCost => _fCost;
        public PathNode PreviousNode => _previousNode;
        public bool IsWalkable => _isWalkable;

        public PathNode(MapPosition mapPosition)
        {
            this.mapPosition = mapPosition;
        }

        public override string ToString()
        {
            return mapPosition.ToString();
        }

        public void CalculateFCost()
        {
            _fCost = _gCost + _hCost;
        }

        public void SetHCost(int hCost)
        {
            _hCost = hCost;
        }

        public void SetGCost(int gCost)
        {
            _gCost = gCost;
        }

        public void ResetPreviousNode()
        {
            _previousNode = null;
        }

        public void SetPreviousNode(PathNode previousNode)
        {
            _previousNode = previousNode;
        }

        public void SetIsWalkable(bool isWalkable)
        {
            _isWalkable = isWalkable;
        }
    }
}