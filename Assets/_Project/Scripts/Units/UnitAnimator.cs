using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Equipment;
using UnityEngine;

namespace Descending.Units
{
    public class UnitAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator = null;
        [SerializeField] private Rigidbody _rigidbody = null;
        [SerializeField] private bool _applyMotion = false;
        
        private Unit _unit;
        
        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }

        private void Update()
        {
            if (_applyMotion == false) return;
            
            _animator.SetFloat("Blend", _rigidbody.velocity.magnitude);
        }

        public void Setup(Animator animator, RuntimeAnimatorController animatorController)
        {
            _animator = animator;
            _animator.runtimeAnimatorController = animatorController;
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

        public void Shoot()
        {
            _animator.SetTrigger("Shoot");
        }

        public void Cast()
        {
            _animator.SetTrigger("Cast");
        }

        public void SetAnimatorOverride(WeaponData weaponData)
        {
            _animator.runtimeAnimatorController = weaponData.AnimatorOverride;
        }
    }
}