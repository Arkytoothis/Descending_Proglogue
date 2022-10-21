using Descending.Core;
using System.Text;
using UnityEngine;

namespace Descending.Abilities
{
    [System.Serializable]
    public class WeaponAttackEffect : AbilityEffect
    {
        [SerializeField] private int _bonusAttacks = 0;
        [SerializeField] private int _accuracyModifier = 0;
        [SerializeField] private int _bonusDamage = 0;
        [SerializeField] private float _damageModifier = 1f;

        public int BonusAttacks => _bonusAttacks;
        public int AccuracyModifier => _accuracyModifier;
        public int BonusDamage => _bonusDamage;
        public float DamageModifier => _damageModifier;

        public override string GetTooltipText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Weapon Attack").AppendLine();

            if (_bonusAttacks > 0)
            {
                sb.Append("Bonus Attacks +").Append(_bonusAttacks).AppendLine();
            }
            
            if (_accuracyModifier > 0)
            {
                sb.Append("Accuracy +").Append(_accuracyModifier).AppendLine();
            }
            else if (_accuracyModifier < 0)
            {
                sb.Append("Accuracy ").Append(_accuracyModifier).AppendLine();
            }

            if (_damageModifier > 1f || _bonusDamage > 0)
            {
                sb.Append("Damage ");
            }
            
            if (_damageModifier > 1f)
            {
                sb.Append(" x").Append(_damageModifier);
            }

            if (_bonusDamage > 0)
            {
                sb.Append(" +").Append(_bonusDamage);
            }
            else if (_bonusDamage < 0)
            {
                sb.Append(" ").Append(_bonusDamage);
            }

            sb.AppendLine();
            
            return sb.ToString();
        }
    }
}