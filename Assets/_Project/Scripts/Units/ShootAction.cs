using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Equipment;
using Descending.Tiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Descending.Units
{
    public class ShootAction : BaseAction
    {
        public enum State
        {
            Aiming,
            Shooting,
            Cooldown
        };

        [SerializeField] private UnitAnimator _unitAnimator = null;
        [SerializeField] private Transform _projectileSpawnPoint = null;
        [SerializeField] private float _shootingStateTime = 0.1f;
        [SerializeField] private float _cooldownStateTime = 0.5f;
        [SerializeField] private float _aimingStateTime = 1f;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _spawnProjectileDelay = 1f;

        private int _maxShootDistance = 0;
        private State _state;
        private float _stateTimer;
        private Unit _targetUnit;
        private bool _canShoot;
        
        public Unit TargetUnit => _targetUnit;
        public int MaxShootDistance => _maxShootDistance;


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
                case State.Shooting:
                    if (_canShoot)
                    {
                        _canShoot = false;
                        Shoot();
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
                    _state = State.Shooting;
                    _stateTimer = _shootingStateTime;
                    break;
                case State.Shooting:
                    _state = State.Cooldown;
                    _stateTimer = _cooldownStateTime;
                    break;
                case State.Cooldown:
                    ActionComplete();
                    break;
            }
        }
        
        public override void PerformAction(MapPosition mapPosition, Action onShootComplete)
        {
            _targetUnit = MapManager.Instance.GetUnitAtGridPosition(mapPosition);
            _state = State.Aiming;
            _stateTimer = _aimingStateTime;
            _canShoot = true;
            
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
            for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
            {
                for (int y = -_maxShootDistance; y <= _maxShootDistance; y++)
                {
                    MapPosition offsetMapPosition = new MapPosition(x, y);
                    MapPosition testMapPosition = unitPosition + offsetMapPosition;

                    if (MapManager.Instance.IsValidGridPosition(testMapPosition) == false) continue;
                    int testDistance = Mathf.Abs(x) + Mathf.Abs(y);
                    if (testDistance > _maxShootDistance) continue;
                    if (!MapManager.Instance.HasAnyUnit(testMapPosition)) continue;
                    
                    Unit targetUnit = MapManager.Instance.GetUnitAtGridPosition(testMapPosition);
                    if (targetUnit.IsEnemy == _unit.IsEnemy) continue;

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
            Item item = _unit.GetRangedWeapon();
            WeaponData weaponData = item.GetWeaponData();
            _unitAnimator.SetAnimatorOverride(weaponData.AnimatorOverride);
            _spawnProjectileDelay = weaponData.ProjectileDelay;
            _maxShootDistance = weaponData.Range;
        }

        public override int GetActionPointCost()
        {
            return 1;
        }

        private void Shoot()
        {
            _unitAnimator.Shoot();
            StartCoroutine(DelayedSpawnProjectile());
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
        
        private IEnumerator DelayedSpawnProjectile()
        {
            yield return new WaitForSeconds(_spawnProjectileDelay);

            Item rangedWeapon = _unit.GetRangedWeapon();

            if (rangedWeapon != null)
            {
                GameObject clone = Instantiate(rangedWeapon.GetWeaponData().Projectile.Prefab, _projectileSpawnPoint.position,
                    _projectileSpawnPoint.rotation);

                if (_targetUnit != null)
                {
                    Vector3 projectileTargetPosition = _targetUnit.transform.position;
                    projectileTargetPosition.y = _projectileSpawnPoint.position.y;

                    Projectile projectile = clone.GetComponent<Projectile>();
                    projectile.Setup(_unit, _targetUnit, rangedWeapon);
                }
            }
        }
    }
}
