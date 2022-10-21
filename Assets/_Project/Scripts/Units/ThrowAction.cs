using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using Descending.Core;
using UnityEngine;

namespace Descending.Units
{
    public class ThrowAction : BaseAction
    {
        [SerializeField] private GameObject _projectilePrefab = null;
        [SerializeField] private int _maxThrowDistance = 7;
        
        private void Update()
        {
            if (!_isActive) return;
            
        }

        public override string GetName()
        {
            return "Throw";
        }

        public override void PerformAction(MapPosition mapPosition, Action onThrowComplete)
        {
            //Debug.Log("Throwing");
            ActionStart(onThrowComplete);
            GameObject clone = Instantiate(_projectilePrefab, _unit.transform.position, Quaternion.identity);
            ThrowableProjectile projectile = clone.GetComponent<ThrowableProjectile>();
            projectile.Setup(_unit, mapPosition, OnThrowComplete);
        }

        public override List<MapPosition> GetValidActionGridPositions()
        {
            List<MapPosition> validGridPositions = new List<MapPosition>();
            
            for (int x = -_maxThrowDistance; x <= _maxThrowDistance; x++)
            {
                for (int y = -_maxThrowDistance; y <= _maxThrowDistance; y++)
                {
                    MapPosition offsetMapPosition = new MapPosition(x, y);
                    MapPosition testMapPosition = _unit.CurrentMapPosition + offsetMapPosition;
                    
                    if (MapManager.Instance.IsValidGridPosition(testMapPosition) == false) continue;
                    
                    int testDistance = Mathf.Abs(x) + Mathf.Abs(y);
                    if (testDistance > _maxThrowDistance) continue;
                    
                    if (MapManager.Instance.Linecast(_unit.CurrentMapPosition, testMapPosition)) continue;
                    
                    validGridPositions.Add(testMapPosition);
                }
            }

            return validGridPositions;
        }

        public override int GetActionPointCost()
        {
            return 1;
        }

        public override EnemyAction GetEnemyAction(MapPosition mapPosition)
        {
            return new EnemyAction
            {
                _mapPosition = mapPosition,
                ActionValue = 0
            };
        }

        private void OnThrowComplete()
        {
            ActionComplete();
        }
    }
}
