using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Descending.Units
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trailRenderer = null;
        [SerializeField] private GameObject _hitEffectPrefab = null;
        [SerializeField] private float _moveSpeed = 100f;
        [SerializeField] private float _force = 300f;
        [SerializeField] private int _minimumDamage = 0;
        [SerializeField] private int _maximumDamage = 0;

        private Unit _sourceUnit;
        private Unit _targetUnit;

        public Unit SourceUnit => _sourceUnit;
        public Unit TargetUnit => _targetUnit;
        public float Force => _force;

        public void Setup(Unit sourceUnit, Unit targetUnit)
        {
            _sourceUnit = sourceUnit;
            _targetUnit = targetUnit;
        }

        private void Update()
        {
            Vector3 targetPosition = _targetUnit.HitTransform.position;
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;
            float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

            if (distanceBeforeMoving < distanceAfterMoving)
            {
                _targetUnit.Damage(gameObject,Random.Range(_minimumDamage, _maximumDamage + 1));
                
                transform.position = targetPosition;
                _trailRenderer.transform.SetParent(null);
                Destroy(gameObject);

                Instantiate(_hitEffectPrefab, targetPosition, Quaternion.identity);
            }
        }
    }
}