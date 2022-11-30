using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Combat;
using Descending.Equipment;
using Descending.Tiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Descending.Units
{
    public class MeleeAttackAction : BaseAction
    {
        private enum States { Pre_Hit, Post_Hit }

        [SerializeField] private UnitAnimator _unitAnimator = null;
        [SerializeField] private int _meleeRange = 1;
        [SerializeField] private float _preHitTime = 0.7f;
        [SerializeField] private float _postHitTime = 0.5f;
        [SerializeField] private float _rotationSpeed = 10f;

        private States _state;
        private float _stateTimer;
        private Unit _targetUnit;
        
        public int MeleeRange => _meleeRange;

        protected override void Awake()
        {
            _unit = GetComponentInParent<Unit>();
            _unitAnimator = _unit.UnitAnimator;
        }

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
        
        public void SetupData()
        {
            Item item = _unit.GetMeleeWeapon();
            if (item != null)
            {
                WeaponData weaponData = item.GetWeaponData();
                _unitAnimator.SetAnimatorOverride(weaponData.AnimatorOverride);
                _meleeRange = weaponData.Range;
            }
        }
        
        private void NextState()
        {
            switch (_state)
            {
                case States.Pre_Hit:
                    _state = States.Post_Hit;
                    _stateTimer = _postHitTime;
                    CombatCalculator.ProcessAttack(_unit, _targetUnit, null);
                    break;
                case States.Post_Hit:
                    
                    _unitAnimator.MeleeCompleted();
                    ActionComplete();
                    break;
            }
        }
        
        public override string GetName()
        {
            return "";
        }

        public override void PerformAction(MapPosition mapPosition, Action onMeleeComplete)
        {
            //Debug.Log("Melee Attack");
            _targetUnit = MapManager.Instance.GetUnitAtGridPosition(mapPosition);
            _state = States.Pre_Hit;
            _stateTimer = _preHitTime;
            
            _unitAnimator.MeleeStarted();
            ActionStart(onMeleeComplete);
        }

        public int GetTargetCountAtPosition(MapPosition mapPosition)
        {
            return GetValidActionGridPositions(mapPosition).Count;
        }

        public override List<MapPosition> GetValidActionGridPositions()
        {
            return GetValidActionGridPositions(_unit.CurrentMapPosition);
        }
        
        public List<MapPosition> GetValidActionGridPositions(MapPosition mapPosition)
        {
            if (_unit.IsEnemy == true)
            {
                SetupData();
            }
            
            List<MapPosition> validGridPositions = new List<MapPosition>();
            
            for (int x = -_meleeRange; x <= _meleeRange; x++)
            {
                for (int y = -_meleeRange; y <= _meleeRange; y++)
                {
                    MapPosition offsetMapPosition = new MapPosition(x, y);
                    MapPosition testMapPosition = mapPosition + offsetMapPosition;
                    
                    if (MapManager.Instance.IsValidGridPosition(testMapPosition) == false) continue;

                    if (_meleeRange > 1)
                    {
                        int testDistance = Mathf.Abs(x) + Mathf.Abs(y);
                        if (testDistance > _meleeRange) continue;
                    }

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
