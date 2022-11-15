using System.Collections.Generic;
using System.Text;
using Descending.Attributes;
using Descending.Core;
using Descending.Units;
using UnityEngine;

namespace Descending.Abilities
{
    [System.Serializable]
    public class DamageEffect : AbilityEffect
    {
        [SerializeField] private DamageTypeDefinition _damageType = null;
        [SerializeField] private AttributeDefinition _attribute = null;
        [SerializeField] private int _minimumValue = 0;
        [SerializeField] private int _maximumValue = 0;

        public DamageTypeDefinition DamageType => _damageType;
        public AttributeDefinition Attribute => _attribute;
        public int MinimumValue => _minimumValue;
        public int MaximumValue => _maximumValue;

        public override string GetTooltipText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Causes ").Append(_minimumValue).Append(" - ").Append(_maximumValue).Append(" ").Append(_damageType.Name).Append(" damage\n");

            return sb.ToString();
        }

        public override void Process(Unit user, List<Unit> targets)
        {
            if (_affects == AbilityEffectAffects.User)
            {
                // if (user.GetType() == typeof(HeroUnit))
                // {
                //     HeroUnit hero = user as HeroUnit;
                //
                //     if (hero != null)
                //     {
                //         int amount = Random.Range(_minimumValue, _maximumValue + 1);
                //         hero.Damage(_attribute, _damageType, amount, false);
                //     }
                // }
                // else if (user.GetType() == typeof(Enemy))
                // {
                //     Enemy enemy = (Enemy)user;
                //
                //     if (enemy != null)
                //     {
                //         int amount = Random.Range(_minimumValue, _maximumValue + 1);
                //         enemy.TakeDamage(_attribute, _damageType, amount, false);
                //     }
                // }
            }
            else if (_affects == AbilityEffectAffects.Target)
            {
                foreach (Unit unit in targets)
                {
                    int amount = Random.Range(_minimumValue, _maximumValue + 1);
                    unit.Damage(user.gameObject, amount);
                }
            }
        }
    }
}