using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using Descending.Tiles;
using UnityEngine;

namespace Descending.Units
{
    public class MoveAction : BaseAction
    {
        [SerializeField] private UnitAnimator _unitAnimator = null;
        [SerializeField] private float _moveSpeed = 4f;
        [SerializeField] private float _stoppingDistance = 0.1f;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private int _maxMoveDistance = 3;
        
        private List<Vector3> _pathList;
        private int _currentPositionIndex; 
        
        protected override void Awake()
        {
            _unit = GetComponentInParent<Unit>();
            _unitAnimator = _unit.UnitAnimator;
        }
        
        public override string GetName()
        {
            return "M";
        }

        private void Update()
        {
            if (_isActive == false) return;

            Vector3 targetPosition = _pathList[_currentPositionIndex];
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            
            _unit.transform.forward = Vector3.Lerp(_unit.transform.forward, moveDirection, _rotationSpeed * Time.deltaTime);
            
            if (Vector3.Distance(_unit.transform.position, targetPosition) > _stoppingDistance)
            {
                _unit.transform.position += moveDirection * (_moveSpeed * Time.deltaTime);
            }
            else
            {
                _currentPositionIndex++;
            
                if (_currentPositionIndex >= _pathList.Count)
                {
                    _unitAnimator.StopMoving();
                    ActionComplete();
                }
            }
        }

        public override void PerformAction(MapPosition targetMapPosition, Action onMoveComplete)
        {
            List<MapPosition> mapPositions = PathfindingManager.Instance.FindPath(_unit.CurrentMapPosition, targetMapPosition, out int pathLength);
            
            _currentPositionIndex = 0;

            _pathList = new List<Vector3>();
            foreach (MapPosition mapPosition in mapPositions)
            {
                _pathList.Add(MapManager.Instance.GetWorldPosition(mapPosition));
            }
            
            _unitAnimator.StartMoving();
            ActionStart(onMoveComplete);
        }

        public override List<MapPosition> GetValidActionGridPositions()
        {
            _maxMoveDistance = _unit.Attributes.GetStatistic("Movement").Current;
            
            List<MapPosition> validGridPositions = new List<MapPosition>();
            for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
            {
                for (int y = -_maxMoveDistance; y <= _maxMoveDistance; y++)
                {
                    MapPosition offsetMapPosition = new MapPosition(x, y);
                    MapPosition testMapPosition = _unit.CurrentMapPosition + offsetMapPosition;

                    if (MapManager.Instance.IsValidGridPosition(testMapPosition) == false) continue;
                    if (MapManager.Instance.HasAnyUnit(testMapPosition) == true) continue;
                    if (_unit.CurrentMapPosition == testMapPosition) continue;
                    if (!PathfindingManager.Instance.IsGridPositionWalkable(testMapPosition)) continue;
                    if (!CanPathToTarget(testMapPosition)) continue;
                    
                    validGridPositions.Add(testMapPosition);
                }
            }

            return validGridPositions;
        }

        public override int GetActionPointCost()
        {
            return 1;
        }

        private bool CanPathToTarget(MapPosition targetPosition)
        {
            int pathLength =  PathfindingManager.Instance.GetPathLength(_unit.CurrentMapPosition, targetPosition);
            if (pathLength <= 0 || pathLength > _maxMoveDistance * 10)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override EnemyAction GetEnemyAction(MapPosition mapPosition)
        {
            int targetCount = _unit.GetAction<RangedAttackAction>().GetTargetCountAtPosition(mapPosition);
            
            return new EnemyAction
            {
                _mapPosition = mapPosition,
                ActionValue = targetCount * 10,
            };
        }
    }
}
