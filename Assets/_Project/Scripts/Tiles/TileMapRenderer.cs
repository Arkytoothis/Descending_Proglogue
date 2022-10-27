using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using UnityEngine;

namespace Descending.Tiles
{
    public class TileMapRenderer : MonoBehaviour
    {
        [System.Serializable] public struct TileData
        {
            public TileColors TileColor;
            public Material BorderMaterial;
            public Material BackgroundMaterial;
        }
        
        public enum TileColors { Black, White, Blue, Green, Red, Yellow }
        
        public static TileMapRenderer Instance { get; private set; }
        
        [SerializeField] private GameObject _tileVisualPrefab = null;
        [SerializeField] private Transform _tilesParent = null;
        [SerializeField] private List<TileData> _tileDataList = null;

        private TileRenderer[,] _tileRenderers = null;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple Tile Map Renderers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }
        
        public void Setup()
        {
            _tileRenderers = new TileRenderer[MapManager.Instance.Width, MapManager.Instance.Height];
            
            for (int x = 0; x < MapManager.Instance.Width; x++)
            {
                for (int y = 0; y < MapManager.Instance.Height; y++)
                {
                    MapPosition mapPosition = new MapPosition(x, y);
                    GameObject clone = Instantiate(_tileVisualPrefab, MapManager.Instance.GetWorldPosition(mapPosition), Quaternion.identity);
                    clone.transform.SetParent(_tilesParent);
                    
                    TileRenderer tileRenderer = clone.GetComponent<TileRenderer>();
                    _tileRenderers[x, y] = tileRenderer;
                }
            }

            ActionManager.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
            MapManager.Instance.OnAnyUnitMoved += LevelGrid_OnAnyUnitMoved;
            //UpdateGridVisuals();
        }

        private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
        {
            UpdateGridVisuals();
        }

        private void LevelGrid_OnAnyUnitMoved(object sender, EventArgs e)
        {
            UpdateGridVisuals();
        }
        
        public void HideAllGridPositions()
        {
            for (int x = 0; x < MapManager.Instance.Width; x++)
            {
                for (int y = 0; y < MapManager.Instance.Height; y++)
                {
                    _tileRenderers[x,y].Hide();
                }
            }
        }

        public void ShowGridPositionList(List<MapPosition> gridPositions, TileColors borderColor, TileColors backgroundColor)
        {
            foreach (MapPosition gridPosition in gridPositions)
            {
                _tileRenderers[gridPosition.X, gridPosition.Y].Show(GetBorderMaterial(borderColor), GetBackgroundMaterial(backgroundColor));
            }
        }

        public void ShowGridPositionRange(MapPosition centerPosition, int range, TileColors borderColor, TileColors backgroundColor)
        {
            List<MapPosition> positions = new List<MapPosition>();
            
            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    MapPosition testPosition = centerPosition + new MapPosition(x, y);
                    
                    if (MapManager.Instance.IsValidGridPosition(testPosition) == false) continue;
                    int testDistance = Mathf.Abs(x) + Mathf.Abs(y);
                    if (testDistance > range)
                    {
                        continue;
                    }

                    if (MapManager.Instance.Linecast(centerPosition, testPosition)) continue;
                    
                    positions.Add(testPosition);
                }
            }

            ShowGridPositionList(positions, borderColor, backgroundColor);
        }

        public void ShowGridPositionRangeSquare(MapPosition centerPosition, int range, TileColors borderColor, TileColors backgroundColor)
        {
            List<MapPosition> positions = new List<MapPosition>();
            
            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    MapPosition testPosition = centerPosition + new MapPosition(x, y);
                    
                    if (MapManager.Instance.IsValidGridPosition(testPosition) == false) continue;
                    if (MapManager.Instance.Linecast(centerPosition, testPosition)) continue;
                    
                    positions.Add(testPosition);
                }
            }

            ShowGridPositionList(positions, borderColor, backgroundColor);
        }
        
        private void UpdateGridVisuals()
        {
            HideAllGridPositions();
            Unit selectedUnit = UnitManager.Instance.SelectedHero;
            BaseAction selectedAction = ActionManager.Instance.SelectedAction;
            TileColors borderColor = TileColors.Black;
            TileColors backgroundColor = TileColors.Black;
            
            switch (selectedAction)
            {
                case MoveAction moveAction:
                    borderColor = TileColors.Blue;
                    backgroundColor = TileColors.Blue;
                    break;
                case SpinAction spinAction:
                    borderColor = TileColors.Yellow;
                    backgroundColor = TileColors.Yellow;
                    break;
                case ThrowAction throwAction:
                    borderColor = TileColors.Black;
                    backgroundColor = TileColors.Black;
                    break;
                case ShootAction shootAction:
                    borderColor = TileColors.Red;
                    backgroundColor = TileColors.Red;
                    ShowGridPositionRange(selectedUnit.CurrentMapPosition, shootAction.MaxShootDistance, TileColors.Black, TileColors.Black);
                    break;
                case MeleeAction meleeAction:
                    borderColor = TileColors.Green;
                    backgroundColor = TileColors.Green;
                    ShowGridPositionRangeSquare(selectedUnit.CurrentMapPosition, meleeAction.MeleeRange, TileColors.Black, TileColors.Black);
                    break;
                case InteractAction interactAction:
                    borderColor = TileColors.Yellow;
                    backgroundColor = TileColors.Yellow;
                    ShowGridPositionRangeSquare(selectedUnit.CurrentMapPosition, interactAction.InteractRange, TileColors.Black, TileColors.Black);
                    break;
            }
            
            ShowGridPositionList(selectedAction.GetValidActionGridPositions(), borderColor, backgroundColor);
        }

        private Material GetBorderMaterial(TileColors tileColor)
        {
            return _tileDataList[(int) tileColor].BorderMaterial;
        }

        private Material GetBackgroundMaterial(TileColors tileColor)
        {
            return _tileDataList[(int) tileColor].BackgroundMaterial;
        }
    }
}
