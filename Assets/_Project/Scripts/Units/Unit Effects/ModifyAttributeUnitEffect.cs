using System.Collections;
using System.Collections.Generic;
using Descending.Abilities;
using Descending.Attributes;
using UnityEngine;

namespace Descending.Units
{
    [System.Serializable]
    public class ModifyAttributeUnitEffect : UnitEffect
    {
        [SerializeField] private AttributeDefinition _attributeDefinition = null;
        [SerializeField] private int _modifier = 0;

        public AttributeDefinition AttributeDefinition => _attributeDefinition;
        public int Modifier => _modifier;

        public ModifyAttributeUnitEffect(AbilityEffect abilityEffect)
        {
            if (abilityEffect is ModifyAttributeAbilityEffect unitEffect)
            {
                _icon = unitEffect.Icon;
                _attributeDefinition = unitEffect.Attribute;
                _duration = Random.Range(unitEffect.MinimumDuration, unitEffect.MaximumDuration + 1);
                _modifier = Random.Range(unitEffect.MinimumModifier, unitEffect.MaximumModifier + 1);
            }
        }
    }
}