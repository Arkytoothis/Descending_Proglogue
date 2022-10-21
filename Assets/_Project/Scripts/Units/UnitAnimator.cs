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
                moveAction.OnStartMoving += MoveAction_OnStartMoving;
                moveAction.OnStopMoving += MoveAction_OnStopMoving;
            }
            
            if (TryGetComponent<ShootAction>(out ShootAction shootAction))
            {
                shootAction.OnShoot += ShootAction_OnShoot;
            }
            
            if (TryGetComponent<MeleeAction>(out MeleeAction meleeAction))
            {
                meleeAction.OnMeleeStarted += meleeAction_OnMeleeStarted;
                meleeAction.OnMeleeCompleted += meleeAction_OnMeleeCompleted;
            }
        }
        
        private void meleeAction_OnMeleeStarted(object sender, EventArgs e)
        {
            _animator.SetTrigger("Melee");
        }

        private void meleeAction_OnMeleeCompleted(object sender, EventArgs e)
        {
            
        }

        private void MoveAction_OnStartMoving(object sender, EventArgs e)
        {
            _animator.SetBool("isWalking", true);
        }

        private void MoveAction_OnStopMoving(object sender, EventArgs e)
        {
            _animator.SetBool("isWalking", false);
        }

        private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
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