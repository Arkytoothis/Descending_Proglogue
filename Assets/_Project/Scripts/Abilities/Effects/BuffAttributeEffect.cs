using Descending.Core;
using System.Text;
using Descending.Attributes;
using UnityEngine;

namespace Descending.Abilities
{
    [System.Serializable]
    public class BuffAttributeEffect : AbilityEffect
    {
        [SerializeField] private AttributeDefinition _attribute = null;
        [SerializeField] private int _minimumValue = 0;
        [SerializeField] private int _maximumValue = 0;

        public AttributeDefinition Attribute { get => _attribute; }
        public int MinimumValue { get => _minimumValue; }
        public int MaximumValue { get => _maximumValue; }

        public override string GetTooltipText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Increases ").Append(_attribute.Name).Append(" by ").Append(_minimumValue);

            if (_maximumValue > _minimumValue)
                sb.Append(" - ").Append(_maximumValue).Append("\n");
            else
                sb.Append("\n");

            return sb.ToString();
        }
    }
}