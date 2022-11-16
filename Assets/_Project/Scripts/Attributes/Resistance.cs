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
        [SerializeField] private int _currentValue = 0;
        [SerializeField] private int _maximumValue = 0;
        public int CurrentValue => _currentValue;
        public int MaximumValue => _maximumValue;

        public string DamageType => _damageType;
        
        public Resistance(DamageTypeDefinition damageType, int maximum)
        {
            _damageType = damageType.Name;
            _currentValue = maximum;
            _maximumValue = maximum;
        }

        public void SetResistance(Resistance resistance)
        {
            _damageType = resistance._damageType;
            _currentValue = resistance._maximumValue;
            _maximumValue = resistance._maximumValue;
        }
    }
}