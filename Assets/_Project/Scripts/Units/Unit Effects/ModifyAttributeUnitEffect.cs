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
        private Sprite _icon = null;
        private AttributeDefinition _attributeDefinition = null;
        private int _minimumDuration = 0;
        private int _maximumDuration = 0;
        private int _minimumModifier = 0;
        private int _maximumModifier = 0;

        public Sprite Icon => _icon;
        public AttributeDefinition AttributeDefinition => _attributeDefinition;
        public int MinimumDuration => _minimumDuration;
        public int MaximumDuration => _maximumDuration;
        public int MinimumModifier => _minimumModifier;
        public int MaximumModifier => _maximumModifier;

        public ModifyAttributeUnitEffect(AbilityEffect abilityEffect)
        {
            if (abilityEffect is ModifyAttributeAbilityEffect unitEffect)
            {
                _icon = unitEffect.Icon;
                _attributeDefinition = unitEffect.Attribute;
                _minimumDuration = unitEffect.MinimumDuration;
                _maximumDuration = unitEffect.MaximumDuration;
                _minimumModifier = unitEffect.MinimumModifier;
                _maximumModifier = unitEffect.MaximumModifier;
            }
        }
    }
}