using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Units
{
    public class UnitAnimator : MonoBehaviour
    {
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
            
            if (TryGetComponent<RangedAttackAction>(out RangedAttackAction shootAction))
            {
            }
            
            if (TryGetComponent<MeleeAttackAction>(out MeleeAttackAction meleeAction))
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

        public void Shoot()
        {
            _animator.SetTrigger("Shoot");
        }

        public void Cast()
        {
            _animator.SetTrigger("Cast");
        }

        public void SetAnimatorOverride(AnimatorOverrideController overrideController)
        {
            _animator.runtimeAnimatorController = overrideController;
        }
    }
}