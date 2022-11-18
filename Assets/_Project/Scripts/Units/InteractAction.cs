using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Interactables;
using Descending.Tiles;
using UnityEngine;

namespace Descending.Units
{
    public class InteractAction : BaseAction
    {
        [SerializeField] private int _interactRange = 1;

        public int InteractRange => _interactRange;

        private void Update()
        {
            if (!_isActive) return;
        }

        public override string GetName()
        {
            return "I";
        }

        public override void PerformAction(MapPosition mapPosition, Action onInteractComplete)
        {
            IInteractable interactable = MapManager.Instance.GetInteractableAtGridPosition(mapPosition);
            interactable.Interact(onInteractComplete);
            ActionStart(onInteractComplete);
        }

        private void OnInteractComplete()
        {
            ActionComplete();
        }
        
        public override List<MapPosition> GetValidActionGridPositions()
        {
            List<MapPosition> validGridPositions = new List<MapPosition>();
            
            for (int x = -_interactRange; x <= _interactRange; x++)
            {
                for (int y = -_interactRange; y <= _interactRange; y++)
                {
                    MapPosition offsetMapPosition = new MapPosition(x, y);
                    MapPosition testMapPosition = _unit.CurrentMapPosition + offsetMapPosition;
                    
                    if (MapManager.Instance.IsValidGridPosition(testMapPosition) == false) continue;

                    IInteractable interactable = MapManager.Instance.GetInteractableAtGridPosition(testMapPosition);
                    if (interactable == null) continue;
                    
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
    }
}
