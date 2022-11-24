using System.Collections;
using System.Collections.Generic;
using Descending.Equipment;
using Descending.Gui;
using Descending.Units;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Descending.Combat
{
    public static class CombatCalculator
    {
        public static void ProcessAttack(Unit attacker, Unit defender)
        {
            if (TryDefense(defender))
            {
                Miss(attacker, defender);
            }
            else
            {
                Hit(attacker, defender);
            }
        }

        private static bool TryDefense(Unit defender)
        {
            bool defended = false;
            int block = defender.Attributes.GetStatistic("Block").TotalCurrent();
            int dodge = defender.Attributes.GetStatistic("Dodge").TotalCurrent();
            int roll = Random.Range(0, 100);

            if (block >= dodge)
            {
                if (roll <= block)
                {
                    defended = true;
                    CombatTextHandler.Instance.DisplayCombatText(new CombatText(defender.CombatTextTransform.position, "Block!", "default"));
                }
            }
            else
            {
                if (roll <= dodge)
                {
                    defended = true;
                    CombatTextHandler.Instance.DisplayCombatText(new CombatText(defender.CombatTextTransform.position, "Dodge!", "default"));
                }
            }

            return defended;
        }

        private static void Hit(Unit attacker, Unit defender)
        {
            Item meleeWeapon = attacker.GetMeleeWeapon();
            Item rangedWeapon = attacker.GetRangedWeapon();
            WeaponData weaponData = null;
            
            if (meleeWeapon != null)
            {
                RollDamage(meleeWeapon, attacker, defender);
            }
            else if (rangedWeapon != null)
            {
                RollDamage(rangedWeapon, attacker, defender);
            }
        }

        private static void Miss(Unit attacker, Unit defender)
        {
        }

        private static void RollDamage(Item weapon, Unit attacker, Unit defender)
        {
            WeaponData weaponData = weapon.GetWeaponData();
            
            if (weaponData != null)
            {
                if (weaponData.Projectile != null)
                {
                    for (int i = 0; i < weaponData.Projectile.DamageEffects.Count; i++)
                    {
                        int minDamage = weaponData.Projectile.DamageEffects[i].MinimumValue + attacker.Attributes.GetStatistic("Might Damage").TotalCurrent();
                        int maxDamage = weaponData.Projectile.DamageEffects[i].MaximumValue + attacker.Attributes.GetStatistic("Might Damage").TotalCurrent();
                        int damage = Random.Range(minDamage, maxDamage + 1);
                        defender.Damage(attacker.gameObject, weaponData.Projectile.DamageEffects[i].DamageType, damage, weaponData.Projectile.DamageEffects[i].Attribute.Key);
                    }
                }
                else
                {
                    for (int i = 0; i < weaponData.DamageEffects.Count; i++)
                    {
                        int minDamage = weaponData.DamageEffects[i].MinimumValue + attacker.Attributes.GetStatistic("Might Damage").TotalCurrent();
                        int maxDamage = weaponData.DamageEffects[i].MaximumValue + attacker.Attributes.GetStatistic("Might Damage").TotalCurrent();
                        int damage = Random.Range(minDamage, maxDamage + 1);

                        defender.Damage(attacker.gameObject, weaponData.DamageEffects[i].DamageType, damage, weaponData.DamageEffects[i].Attribute.Key);
                    }
                }
            }
        }
    }
}