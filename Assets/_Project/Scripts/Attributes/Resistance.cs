using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using UnityEngine;

namespace Descending.Attributes
{
    [System.Serializable]
    public class Resistance
    {
        [SerializeField] private string _damageType;
        [SerializeField] private float _currentValue = 0;
        [SerializeField] private float _maximumValue = 0;
        public float CurrentValue => _currentValue;
        public float MaximumValue => _maximumValue;

        public string DamageType => _damageType;

        public Resistance(Resistance resistance)
        {
            _damageType = resistance._damageType;
            _currentValue = resistance._maximumValue;
            _maximumValue = resistance._maximumValue;
        }
    }
}