using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Features;
using UnityEngine;

namespace Descending.Scene_Overworld
{
    public class WorldTile : MonoBehaviour
    {
        [SerializeField] private string _name = "";
        [SerializeField] private LayerMask _tileLayerMask;
        [SerializeField] private float _neighborRadius = 6f;
        [SerializeField] private Collider _collider = null;
        [SerializeField] private Transform _tileTransform = null;
        [SerializeField] private bool _isSpawnable = false;
        [SerializeField] private bool _isMovable = false;
        [SerializeField] private int _threatLevel = 0;

        [SerializeField] private WorldFeature _feature = null;
        [SerializeField] private int _x = -1;
        [SerializeField] private int _y = -1;
        [SerializeField] private List<WorldTile> _neighborTiles = null;

        public string Name => _name;
        public WorldFeature Feature => _feature;
        public int X => _x;
        public int Y => _y;
        public bool IsSpawnable => _isSpawnable;
        public bool IsMovable => _isMovable;
        public int ThreatLevel => _threatLevel;
        public List<WorldTile> NeighborTiles => _neighborTiles;

        public void Setup(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public void FindNeighbors()
        {
            _neighborTiles = new List<WorldTile>();

            Collider[] hitColliders = Physics.OverlapSphere(_tileTransform.position, _neighborRadius, _tileLayerMask);
            
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider == _collider) continue;
            
                WorldTile tile = _collider.GetComponentInParent<WorldTile>();
                _neighborTiles.Add(tile);
            }
            // for (int i = 0; i < (int)WorldDirections.Number; i++)
            // {
            //     WorldDirections direction = (WorldDirections)i;
            //     Vector2Int neighbor = GetNeighbor(this, direction);
            //     
            // }
        }

        public void Clicked()
        {
            string s = "";
            if (_feature != null)
            {
                s += _feature.name + ", " ;
            }

            s += _threatLevel;
            Debug.Log(s);
        }

        public void SetThreatLevel(int threatLevel)
        {
            _threatLevel = threatLevel;
        }

        public void SetFeature(WorldFeature feature)
        {
            _feature = feature;
        }

        private Vector2Int[,] _neighborsLookup =
        {
            // even rows 
            {
                new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, -1),
                new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1)
            },
            // odd rows
            {
                new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1),
                new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1)
            }
        };

        public enum WorldDirections
        {
            NorthWest, NorthEast, East, SouthEast, SouthWest, West, Number, None    
        }
        
        private Vector2Int GetNeighbor(WorldTile tile, WorldDirections direction)
        {
            int parity = tile.Y & 1;
            var diff = _neighborsLookup[parity,(int)direction];
            return new Vector2Int(tile.X + diff.x, tile.Y + diff.y);
        }
    }
}