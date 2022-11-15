using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Attributes;
using Descending.Gui;
using UnityEngine;

namespace Descending.Units
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] private AttributesController _attributes = null;
        [SerializeField] private UnitWorldPanel _worldPanel = null;

        private GameObject _attacker = null;

        public GameObject Attacker => _attacker;

        public void Setup(AttributesController attributes)
        {
            _attributes = attributes;
        }
        
        public void TakeDamage(GameObject attacker, int amount)
        {
            _attacker = attacker;
            _attributes.GetVital("Life").Damage(amount);
            _worldPanel.UpdateHealth(this);
            //Debug.Log(name + " takes " + amount + " damage, " + _health + " health remaining");
        }

        public void UseResource(string vital, int amount)
        {
            _attributes.GetVital(vital).Damage(amount);
            _worldPanel.UpdateHealth(this);
        }

        public void RestoreVital(string vital, int amount)
        {
            _attributes.GetVital(vital).Restore(amount);
            _worldPanel.UpdateHealth(this);
        }

        public float GetVitalNormalized(string vitalKey)
        {
            return (float)_attributes.GetVital(vitalKey).Current / _attributes.GetVital(vitalKey).Maximum;
        }
    }
}