using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Units
{
    public class UnitAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _projectileSpawnPoint = null;
        [SerializeField] private GameObject _projectilePrefab = null;
        [SerializeField] private float _spawnProjectileDelay = 1f;
        
        [SerializeField] private Animator _animator = null;
        private Unit _unit;
        
        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }

        public void Setup(Animator animator)
        {
            _animator = animator;
            
            if (TryGetComponent<MoveAction>(out MoveAction moveAction))
            {
                
            }
            
            if (TryGetComponent<ShootAction>(out ShootAction shootAction))
            {
            }
            
            if (TryGetComponent<MeleeAction>(out MeleeAction meleeAction))
            {
            }
        }
        
        public void MeleeStarted()
        {
            _animator.SetTrigger("Melee");
        }

        public void MeleeCompleted()
        {
            
        }

        public void StartMoving()
        {
            _animator.SetBool("isWalking", true);
        }

        public void StopMoving()
        {
            _animator.SetBool("isWalking", false);
        }

        public void Shoot(ShootAction.OnShootEventArgs e)
        {
            _animator.SetTrigger("Shoot");

            StartCoroutine(DelayedSpawnProjectile(e));
        }

        private IEnumerator DelayedSpawnProjectile(ShootAction.OnShootEventArgs e)
        {
            yield return new WaitForSeconds(_spawnProjectileDelay);
            
            GameObject clone = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, _projectileSpawnPoint.rotation);

            if (e.TargetUnit != null)
            {
                Vector3 projectileTargetPosition = e.TargetUnit.transform.position;
                projectileTargetPosition.y = _projectileSpawnPoint.position.y;
                
                Projectile projectile = clone.GetComponent<Projectile>();
                projectile.Setup(_unit, e.TargetUnit);
            }
        }
    }
}