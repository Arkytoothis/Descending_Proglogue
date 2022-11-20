using System.Collections;
using System.Collections.Generic;
using Descending.Abilities;
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

        public void AddEffect(AbilityEffect abilityEffect)
        {
            ModifyAttributeUnitEffect unitEffect = new ModifyAttributeUnitEffect(abilityEffect);
            Debug.Log(unitEffect.GetType() + " added to UnitEffects");
            _effects.Add(unitEffect);
        }
    }
}