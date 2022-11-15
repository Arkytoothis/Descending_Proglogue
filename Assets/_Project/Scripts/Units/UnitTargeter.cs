using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using UnityEngine;

namespace Descending.Units
{
    public class UnitTargeter : MonoBehaviour
    {
        private Unit _unit = null;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }

        public List<MapPosition> GetValidActionGridPositions(MapPosition unitPosition, int range)
        {
            List<MapPosition> validGridPositions = new List<MapPosition>();
            
            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    MapPosition offsetMapPosition = new MapPosition(x, y);
                    MapPosition testMapPosition = unitPosition + offsetMapPosition;

                    if (MapManager.Instance.IsValidGridPosition(testMapPosition) == false) continue;
                    int testDistance = Mathf.Abs(x) + Mathf.Abs(y);
                    if (testDistance > range) continue;
                    if (!MapManager.Instance.HasAnyUnit(testMapPosition)) continue;
                    
                    Unit targetUnit = MapManager.Instance.GetUnitAtGridPosition(testMapPosition);
                    if (targetUnit.IsEnemy == _unit.IsEnemy) continue;

                    if (MapManager.Instance.Linecast(_unit.CurrentMapPosition, testMapPosition)) continue;
                    
                    validGridPositions.Add(testMapPosition);
                }
            }

            return validGridPositions;
        }
    }
}