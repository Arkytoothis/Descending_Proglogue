using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Units
{
    public class HealthSystem : MonoBehaviour
    {
        public event EventHandler OnDead;
        public event EventHandler OnDamaged;
        
        [SerializeField] private int _health = 100;
        [SerializeField] private int _healthMax = 100;

        private GameObject _attacker = null;

        public GameObject Attacker => _attacker;

        public void TakeDamage(GameObject attacker, int amount)
        {
            _attacker = attacker;
            _health -= amount;

            if (_health < 0)
            {
                _health = 0;
            }
            
            OnDamaged?.Invoke(this, EventArgs.Empty);
            //Debug.Log(name + " takes " + amount + " damage, " + _health + " health remaining");

            if (_health <= 0)
            {
                OnDead?.Invoke(this, EventArgs.Empty);
            }
        }

        public float GetHealthNormalized()
        {
            return (float)_health / _healthMax;
        }
    }
}