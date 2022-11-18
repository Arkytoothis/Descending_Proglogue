using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Units
{
    public class UnitEffects : MonoBehaviour
    {
        [SerializeField] private List<UnitEffect> _effects = null;

        public void Setup()
        {
            _effects = new List<UnitEffect>();
        }
        
        
    }
}