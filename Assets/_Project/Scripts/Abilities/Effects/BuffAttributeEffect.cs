using System.Collections.Generic;
using Descending.Core;
using System.Text;
using Descending.Attributes;
using Descending.Units;
using UnityEngine;

namespace Descending.Abilities
{
    [System.Serializable]
    public class BuffAttributeEffect : AbilityEffect
    {
        [SerializeField] private AttributeDefinition _attribute = null;
        [SerializeField] private int _minimumValue = 0;
        [SerializeField] private int _maximumValue = 0;
        
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

        public override void Process(Unit user, List<Unit> targets)
        {
            if (_affects == AbilityEffectAffects.User)
            {
            }
            else if (_affects == AbilityEffectAffects.Target)
            {
                foreach (Unit target in targets)
                {
                    Debug.Log("Buffing " + _attribute.Name + " " + target.name);
                }
            }
        }
    }
}