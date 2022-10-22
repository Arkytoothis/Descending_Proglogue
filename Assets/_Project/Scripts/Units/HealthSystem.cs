using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Gui;
using UnityEngine;

namespace Descending.Units
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] private int _health = 100;
        [SerializeField] private int _healthMax = 100;
        [SerializeField] private UnitWorldPanel _worldPanel = null;

        private GameObject _attacker = null;

        public GameObject Attacker => _attacker;

        public void Setup(int maxHealth)
        {
            _healthMax = maxHealth;
            _health = _healthMax;
        }
        
        public void TakeDamage(GameObject attacker, int amount)
        {
            _attacker = attacker;
            _health -= amount;

            if (_health < 0)
            {
                _health = 0;
            }
            
            _worldPanel.UpdateHealth(this);
            //Debug.Log(name + " takes " + amount + " damage, " + _health + " health remaining");
        }

        public float GetHealthNormalized()
        {
            return (float)_health / _healthMax;
        }
    }
}