using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Features;
using UnityEngine;
using Random = UnityEngine.Random;

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
        [SerializeField] private bool _isWater = false;
        [SerializeField] private int _threatLevel = 0;

        [SerializeField] private WorldFeature _feature = null;
        [SerializeField] private int _x = -1;
        [SerializeField] private int _y = -1;
        [SerializeField] private List<WorldTile> _neighborTiles = null;
        [SerializeField] private List<GameObject> _tileProps = null;
        [SerializeField] private List<GameObject> _centerProps = null;
        [SerializeField] private int _minPropSpawnChance = 50;
        [SerializeField] private int _maxPropSpawnChance = 100;

        public string Name => _name;
        public WorldFeature Feature => _feature;
        public int X => _x;
        public int Y => _y;
        public bool IsSpawnable => _isSpawnable;
        public bool IsMovable => _isMovable;
        public bool IsWater => _isWater;
        public int ThreatLevel => _threatLevel;
        public List<WorldTile> NeighborTiles => _neighborTiles;

        public void Setup(int x, int y)
        {
            _x = x;
            _y = y;
            Randomize();
        }

        public void FindNeighbors(WorldTile[,] tiles)
        {
            _neighborTiles = new List<WorldTile>();

            Collider[] hitColliders = Physics.OverlapSphere(_tileTransform.position, _neighborRadius, _tileLayerMask);
            
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider == _collider) continue;
            
                WorldTile tile = _collider.GetComponentInParent<WorldTile>();
                if (tile._isWater == false)
                {
                    _neighborTiles.Add(tile);
                }
            }
            // for (int i = 0; i < (int)WorldDirections.Number; i++)
            // {
            //     WorldDirections direction = (WorldDirections)i;
            //     Vector2Int neighbor = GetNeighbor(this, direction);
            //     Debug.Log(neighbor.x + " " + neighbor.y);
            //     int neighborX = _x + neighbor.x, neighborY = _y + neighbor.y;
            //     Debug.Log(neighborX + " " + neighborY);
            //     if (neighborX >= 0 && neighborX < tiles.GetLength(0) && neighborY >= 0 && neighborY < tiles.GetLength(1))
            //     {
            //         WorldTile neighborTile = tiles[_x + neighbor.x, _y + neighbor.y];
            //         _neighborTiles.Add(neighborTile);
            //     }
            // }
            
            //Debug.Log("Tile " + name + " neighbor tiles: " + _neighborTiles.Count);
        }

        public void Clicked()
        {
            string s = "";
            if (_feature != null)
            {
                s += _feature.name + ", " ;
            }

            s += _threatLevel;
        }

        public void SetThreatLevel(int threatLevel)
        {
            _threatLevel = threatLevel;
        }

        public void SetFeature(WorldFeature feature)
        {
            _feature = feature;
            if (_feature != null)
            {
                ClearCenterProps();
            }
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
            Vector2Int diff = _neighborsLookup[parity,(int)direction];
            return new Vector2Int(tile.X + diff.x, tile.Y + diff.y);
        }

        private void Randomize()
        {
            transform.Rotate(Vector3.up, Random.Range(0, 6) * 60);

            int propSpawnChance = Random.Range(_minPropSpawnChance, _maxPropSpawnChance + 1);
            
            for (int i = 0; i < _tileProps.Count; i++)
            {
                if (Random.Range(0, 100) < propSpawnChance)
                {
                    _tileProps[i].SetActive(true);
                }
                else
                {
                    _tileProps[i].SetActive(false);
                }
            }
        }

        private void ClearCenterProps()
        {
            for (int i = 0; i < _centerProps.Count; i++)
            {
                _centerProps[i].SetActive(false);
            }
        }
    }
}