using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Abilities;
using Descending.Core;
using Descending.Equipment;
using Descending.Tiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Descending.Units
{
    public class AbilityAction : BaseAction
    {
        public enum State
        {
            Aiming,
            Using_Ability,
            Cooldown
        };

        [SerializeField] private UnitAnimator _unitAnimator = null;
        [SerializeField] private float _useAbilityStateTime = 0.1f;
        [SerializeField] private float _cooldownStateTime = 0.5f;
        [SerializeField] private float _aimingStateTime = 1f;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _spawnProjectileDelay = 1f;

        [SerializeField] private int _range = 0;
        [SerializeField] private Ability _ability = null;
        
        private State _state;
        private float _stateTimer;
        private Unit _targetUnit;
        private bool _canUseAbility;
        private Transform _projectileSpawnPoint;
        
        public Unit TargetUnit => _targetUnit;
        public int Range => _range;
        public Ability Ability => _ability;

        protected override void Awake()
        {
            _unit = GetComponentInParent<Unit>();
            _unitAnimator = _unit.UnitAnimator;
            _projectileSpawnPoint = _unit.ProjectileSpawnPoint;
        }
        
        public void SetAbility(Ability ability)
        {
            _ability = ability;
            _icon = _ability.Definition.Icon;
            _range = _ability.Definition.Range;
        }
        
        private void Update()
        {
            if (_isActive == false) return;

            _stateTimer -= Time.deltaTime;
            
            switch (_state)
            {
                case State.Aiming:
                    Vector3 aimDirection = (_targetUnit.transform.position - _unit.transform.position).normalized;
                    transform.forward = Vector3.Lerp(transform.forward, aimDirection, _rotationSpeed * Time.deltaTime);
                    break;
                case State.Using_Ability:
                    if (_canUseAbility)
                    {
                        _canUseAbility = false;
                        UseAbility();
                    }
                    break;
                case State.Cooldown:
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
                case State.Aiming:
                    _state = State.Using_Ability;
                    _stateTimer = _useAbilityStateTime;
                    break;
                case State.Using_Ability:
                    _state = State.Cooldown;
                    _stateTimer = _cooldownStateTime;
                    break;
                case State.Cooldown:
                    ActionComplete();
                    UnitManager.Instance.RecalculateHeroAttributes();
                    break;
            }
        }
        
        public override void PerformAction(MapPosition mapPosition, Action onShootComplete)
        {
            _targetUnit = MapManager.Instance.GetUnitAtGridPosition(mapPosition);
            _state = State.Aiming;
            _stateTimer = _aimingStateTime;
            _canUseAbility = true;
            
            ActionStart(onShootComplete);
        }

        public override string GetName()
        {
            return "Shoot";
        }

        public List<MapPosition> GetValidActionGridPositions(MapPosition unitPosition)
        {
            if (_unit.IsEnemy == true)
            {
                SetupData();
            }
            
            List<MapPosition> validGridPositions = new List<MapPosition>();
            for (int x = -_range; x <= _range; x++)
            {
                for (int y = -_range; y <= _range; y++)
                {
                    MapPosition offsetMapPosition = new MapPosition(x, y);
                    MapPosition testMapPosition = unitPosition + offsetMapPosition;

                    if (MapManager.Instance.IsValidGridPosition(testMapPosition) == false) continue;
                    int testDistance = Mathf.Abs(x) + Mathf.Abs(y);
                    if (testDistance > _range) continue;
                    if (!MapManager.Instance.HasAnyUnit(testMapPosition)) continue;
                    
                    Unit targetUnit = MapManager.Instance.GetUnitAtGridPosition(testMapPosition);

                    if (_ability.Definition.TargetType == TargetTypes.Friend)
                    {
                        if (targetUnit.IsEnemy == true) continue;
                    }
                    else if (_ability.Definition.TargetType == TargetTypes.Enemy)
                    {
                        if (targetUnit.IsEnemy == false) continue;
                    }

                    if (MapManager.Instance.Linecast(_unit.CurrentMapPosition, testMapPosition)) continue;
                    
                    validGridPositions.Add(testMapPosition);
                }
            }

            return validGridPositions;
        }

        public override List<MapPosition> GetValidActionGridPositions()
        {
            MapPosition unitPosition = _unit.CurrentMapPosition;
            
            return GetValidActionGridPositions(unitPosition);
        }

        public void SetupData()
        {
            if (_ability != null)
            {
                _spawnProjectileDelay = 0.5f;
                _range = _ability.Definition.Range;
            }
        }

        public override int GetActionPointCost()
        {
            return 1;
        }

        private void UseAbility()
        {
            _unitAnimator.Cast();

            if (_ability.Definition.TargetType == TargetTypes.Friend || _ability.Definition.TargetType == TargetTypes.Enemy)
            {
                _ability.Use(_unit, new List<Unit> { _targetUnit });
            }
            else if (_ability.Definition.TargetType == TargetTypes.Party)
            {
                List<Unit> targets = new List<Unit>();
                for (int i = 0; i < UnitManager.Instance.HeroUnits.Count; i++)
                {
                    if (Vector3.Distance(_unit.transform.position, UnitManager.Instance.HeroUnits[i].transform.position) <= _ability.Definition.Area)
                    {
                        targets.Add(UnitManager.Instance.HeroUnits[i]);
                    }
                }
                
                _ability.Use(_unit, targets);
            }
        }
        

        public override EnemyAction GetEnemyAction(MapPosition mapPosition)
        {
            Unit target = MapManager.Instance.GetUnitAtGridPosition(mapPosition);
            return new EnemyAction
            {
                _mapPosition = mapPosition,
                ActionValue = 100 + Mathf.RoundToInt((1 - target.GetHealth()) * 100f),
            };
        }

        public int GetTargetCountAtPosition(MapPosition mapPosition)
        {
            return GetValidActionGridPositions(mapPosition).Count;
        }
    }
}
