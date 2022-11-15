using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Abilities;
using Descending.Equipment;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Descending.Units
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _arcCurve;
        [SerializeField] private TrailRenderer _trailRenderer = null;
        [SerializeField] private GameObject _hitEffectPrefab = null;
        [SerializeField] private float _force = 300f;
        [SerializeField] private float _minimumDistance = 0.2f;
        [SerializeField] private AbilityEffects _hitEffects = null;

        private Unit _sourceUnit;
        private Unit _targetUnit;
        private float _moveSpeed = 100f;
        private Vector3 _targetPosition;
        private Vector3 _positionXZ;
        private float _totalDistance;
        private Rigidbody _rigidbody;
        
        public Unit SourceUnit => _sourceUnit;
        public Unit TargetUnit => _targetUnit;
        public float Force => _force;

        public void Setup(Unit sourceUnit, Unit targetUnit, Item weapon)
        {
            _sourceUnit = sourceUnit;
            _targetUnit = targetUnit;

            if (weapon.GetWeaponData().Projectile != null)
            {
                _moveSpeed = weapon.GetWeaponData().Projectile.Speed;
            }
            
            _targetPosition = _targetUnit.HitTransform.position;
            _positionXZ = new Vector3(transform.position.x, 0, transform.position.z);
            _totalDistance = Vector3.Distance(_positionXZ, _targetPosition);
        }
        
        public void Setup(Unit sourceUnit, Unit targetUnit, ProjectileDefinition projectileDefinition)
        {
            _sourceUnit = sourceUnit;
            _targetUnit = targetUnit;
            _moveSpeed = projectileDefinition.Speed;
            _targetPosition = _targetUnit.HitTransform.position;
            _positionXZ = new Vector3(transform.position.x, 0, transform.position.z);
            _totalDistance = Vector3.Distance(_positionXZ, _targetPosition);

        }

        private void Update()
        {
            if (gameObject == null) return;
            if (_targetUnit == null)
            {
                Destroy(gameObject);
                return;
            }
            
            Vector3 moveDirection = (_targetPosition - _positionXZ).normalized;
            _positionXZ += moveDirection * _moveSpeed * Time.deltaTime;
            float distanceBeforeMoving = Vector3.Distance(_positionXZ, _targetPosition);
            float distanceNormalized = 1 - distanceBeforeMoving / _totalDistance;
            float positionY = _arcCurve.Evaluate(distanceNormalized);
            transform.position = new Vector3(_positionXZ.x, positionY, _positionXZ.z);
            transform.forward = _targetPosition - transform.position;
            float distanceAfterMoving = Vector3.Distance(_positionXZ, _targetPosition);
            
            if (distanceAfterMoving < _minimumDistance)
            {
                //int damage = Random.Range(_minimumDamage, _maximumDamage + 1);
                //_targetUnit.Damage(gameObject, damage);
                foreach (AbilityEffect hitEffect in _hitEffects.Data)
                {
                    hitEffect.Process(_sourceUnit, new List<Unit>{ _targetUnit });
                }
                
                _trailRenderer.transform.SetParent(null);
                transform.position = _targetPosition;
                
                Destroy(gameObject);
                Instantiate(_hitEffectPrefab, _targetPosition, Quaternion.identity);
            }
        }
        
        // private void Update()
        // {
        //     Vector3 moveDirection = (_targetPosition - _positionXZ).normalized;
        //     _positionXZ += moveDirection * _moveSpeed * Time.deltaTime;
        //     float currentDistance = Vector3.Distance(_positionXZ, _targetPosition);
        //     float distanceNormalized = 1 - currentDistance / _totalDistance;
        //     float positionY = _arcCurve.Evaluate(distanceNormalized);
        //     transform.position = new Vector3(_positionXZ.x, positionY, _positionXZ.z);
        //     float distanceAfterMoving = Vector3.Distance(_positionXZ, _targetPosition);
        //     
        //     if (currentDistance < distanceAfterMoving) 
        //     {
        //         int damage = Random.Range(_minimumDamage, _maximumDamage + 1);
        //         _targetUnit.Damage(gameObject, damage);
        //         
        //         transform.position = _positionXZ;
        //         _trailRenderer.transform.SetParent(null);
        //         
        //         Destroy(gameObject);
        //         Instantiate(_hitEffectPrefab, _targetPosition, Quaternion.identity);
        //     }
        // }
    }
}