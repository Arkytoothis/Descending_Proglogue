using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Descending.Units
{
    public class MeleeAction : BaseAction
    {
        private enum States { Pre_Hit, Post_Hit }

        public event EventHandler OnMeleeStarted;
        public event EventHandler OnMeleeCompleted;
        
        [SerializeField] private int _meleeRange = 1;
        [SerializeField] private float _preHitTime = 0.7f;
        [SerializeField] private float _postHitTime = 0.5f;
        [SerializeField] private float _rotationSpeed = 10f;

        private States _state;
        private float _stateTimer;
        private Unit _targetUnit;
        
        public int MeleeRange => _meleeRange;

        private void Update()
        {
            if (!_isActive) return;
            
            _stateTimer -= Time.deltaTime;
            
            switch (_state)
            {
                case States.Pre_Hit:
                    Vector3 aimDirection = (_targetUnit.transform.position - _unit.transform.position).normalized;
                    transform.forward = Vector3.Lerp(transform.forward, aimDirection, _rotationSpeed * Time.deltaTime);
                    break;
                case States.Post_Hit:
                    break;
            }

            if (_stateTimer <= 0f)
            {
                NextState();
            }
        }

        private void NextState()
        {
            switch (_state)
            {
                case States.Pre_Hit:
                    _state = States.Post_Hit;
                    _stateTimer = _postHitTime;
                    int damage = Random.Range(60, 121);
                    _targetUnit.Damage(_unit.gameObject, damage);
                    break;
                case States.Post_Hit:
                    
                    OnMeleeCompleted?.Invoke(this, EventArgs.Empty);
                    ActionComplete();
                    break;
            }
        }
        
        public override string GetName()
        {
            return "Melee";
        }

        public override void PerformAction(MapPosition mapPosition, Action onMeleeComplete)
        {
            //Debug.Log("Melee Attack");
            _targetUnit = MapManager.Instance.GetUnitAtGridPosition(mapPosition);
            _state = States.Pre_Hit;
            _stateTimer = _preHitTime;
            
            OnMeleeStarted?.Invoke(this, EventArgs.Empty);
            ActionStart(onMeleeComplete);
        }

        public override List<MapPosition> GetValidActionGridPositions()
        {
            List<MapPosition> validGridPositions = new List<MapPosition>();
            
            for (int x = -_meleeRange; x <= _meleeRange; x++)
            {
                for (int y = -_meleeRange; y <= _meleeRange; y++)
                {
                    MapPosition offsetMapPosition = new MapPosition(x, y);
                    MapPosition testMapPosition = _unit.CurrentMapPosition + offsetMapPosition;
                    
                    if (MapManager.Instance.IsValidGridPosition(testMapPosition) == false) continue;
                    
                    if (!MapManager.Instance.HasAnyUnit(testMapPosition)) continue;
                    Unit targetUnit = MapManager.Instance.GetUnitAtGridPosition(testMapPosition);
                    if (targetUnit.IsEnemy == _unit.IsEnemy) continue;
                    
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
                ActionValue = 3000
            };
        }
    }
}
