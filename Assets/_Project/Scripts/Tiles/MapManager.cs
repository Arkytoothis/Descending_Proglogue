using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Interactables;
using Descending.Units;
using UnityEngine;

namespace Descending.Tiles
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }

        public event EventHandler OnAnyUnitMoved;
        
        [SerializeField] private GameObject _tileDebugPrefab = null;
        [SerializeField] private Transform _tilesParent = null;
        
        [SerializeField] private int _width = 10;
        [SerializeField] private int _height = 10;
        [SerializeField] private float _cellSize = 2f;
        [SerializeField] private LayerMask _defaultObstaclesLayer;
        
        private TileMap<Tile> _tileMap = null;
        public int Width => _tileMap.Width;
        public int Height => _tileMap.Height;
        public float CellSize => _tileMap.CellSize;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple MapManagers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            
            _tileMap = new TileMap<Tile>(_width, _height, _cellSize, (TileMap<Tile> g, MapPosition gridPosition) => new Tile(g, gridPosition));
            //_tileMap.CreateDebugObjects(_tileDebugPrefab, _tilesParent);
        }

        public void Setup()
        {
            PathfindingManager.Instance.Setup(_width, _height, _cellSize);
        }

        public void AddUnitAtGridPosition(MapPosition mapPosition, Unit unit)
        {
            //Debug.Log("Ading Unit: " + unit.GetName() + " to MapPosition " + mapPosition.ToString());
            _tileMap.GetGridObject(mapPosition).AddUnit(unit);
        }

        public List<Unit> GetUnitListAtGridPosition(MapPosition mapPosition)
        {
            return _tileMap.GetGridObject(mapPosition).GetUnitList();
        }

        public void RemoveUnitAtGridPosition(MapPosition mapPosition, Unit unit)
        {
            _tileMap.GetGridObject(mapPosition).RemoveUnit(unit);
        }
        
        public MapPosition GetGridPosition(Vector3 worldPosition)
        {
            return _tileMap.GetGridPosition(worldPosition);
        }

        public Vector3 GetWorldPosition(MapPosition mapPosition)
        {
            return _tileMap.GetWorldPosition(mapPosition);
        }

        public void UnitMovedGridPosition(Unit unit, MapPosition from, MapPosition to)
        {
            RemoveUnitAtGridPosition(from, unit);
            AddUnitAtGridPosition(to, unit);

            Tile tile = _tileMap.GetGridObject(to);
            
            if (tile.GetCoinValue() > 0)
            {
                ResourcesManager.Instance.AddCoins(tile.GetCoinValue());
                tile.ClearCoins();
            }

            if (tile.GetGemValue() > 0)
            {
                ResourcesManager.Instance.AddGems(tile.GetGemValue());
                tile.ClearGems();
            }
            
            OnAnyUnitMoved.Invoke(this, EventArgs.Empty);
        }

        public bool IsValidGridPosition(MapPosition mapPosition)
        {
            return _tileMap.IsValidGridPosition(mapPosition);
        }

        public bool HasAnyUnit(MapPosition mapPosition)
        {
            return _tileMap.GetGridObject(mapPosition).HasAnyUnit();
        }

        public Unit GetUnitAtGridPosition(MapPosition mapPosition)
        {
            Tile tile = _tileMap.GetGridObject(mapPosition);
            
            return tile.GetUnit();
        }

        public IInteractable GetInteractableAtGridPosition(MapPosition mapPosition)
        {
            Tile tile = _tileMap.GetGridObject(mapPosition);
            return tile.Interactable;
        }

        public void SetInteractableAtGridPosition(MapPosition mapPosition, IInteractable interactable)
        {
            Tile tile = _tileMap.GetGridObject(mapPosition);
            tile.SetInteractable(interactable);
        }

        public void SetDamageableAtGridPosition(MapPosition mapPosition, IDamageable damageable)
        {
            Tile tile = _tileMap.GetGridObject(mapPosition);
            tile.SetDamageable(damageable);
        }

        public bool Linecast(MapPosition startPosition, MapPosition targetPosition)
        {
            Vector3 startWorldPosition = GetWorldPosition(startPosition);
            Vector3 targetWorldPosition = GetWorldPosition(targetPosition);
            Vector3 testDirection = (startWorldPosition - targetWorldPosition).normalized;
            
            if (Physics.Raycast(targetWorldPosition + Vector3.up * 1.7f, testDirection, Vector3.Distance(targetWorldPosition, startWorldPosition), _defaultObstaclesLayer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Tile GetTile(MapPosition mapPosition)
        {
            if (IsValidGridPosition(mapPosition))
            {
                return _tileMap.GetGridObject(mapPosition);
            }
            else
            {
                return null;
            }
        }
    }
}
