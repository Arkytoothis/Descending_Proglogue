using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Equipment;
using Descending.Equipment.Enchantments;
using Descending.Units;
using UnityEngine;

namespace Descending.Attributes
{
    public class AttributesController : MonoBehaviour
    {
        [SerializeField] private UnitEffects _unitEffects = null;
        [SerializeField] private InventoryController _inventory = null;
        [SerializeField] private AttributeDictionary _characteristics = null;
        [SerializeField] private AttributeDictionary _vitals = null;
        [SerializeField] private AttributeDictionary _statistics = null;
        [SerializeField] private ResistanceDictionary _resistances = null;

        private RaceDefinition _raceDefinition = null;
        private ProfessionDefinition _professionDefinition = null;
        
        public AttributeDictionary Characteristics => _characteristics;
        public AttributeDictionary Vitals => _vitals;
        public AttributeDictionary Statistics => _statistics;
        public ResistanceDictionary Resistances => _resistances;

        public void Setup(RaceDefinition race, ProfessionDefinition profession)
        {
            _raceDefinition = race;
            _professionDefinition = profession;
            
            _characteristics.Clear();
            _vitals.Clear();
            _statistics.Clear();
            _resistances.Clear();
            
            foreach (var startingAttribute in race.StartingCharacteristics)
            {
                int value = Random.Range(startingAttribute.Value.MinimumValue, startingAttribute.Value.MaximumValue + 1);
                _characteristics.Add(startingAttribute.Key, new Attribute(startingAttribute.Key));
                _characteristics[startingAttribute.Key].Setup(value);
            }
            
            foreach (var startingAttribute in profession.StartingCharacteristics)
            {
                int value = Random.Range(startingAttribute.Value.MinimumValue, startingAttribute.Value.MaximumValue + 1);
                _characteristics[startingAttribute.Key].AddValue(value);
            }

            foreach (var kvp in Database.instance.Attributes.Vitals)
            {
                _vitals.Add(kvp.Key, new Attribute(kvp.Key));
            }

            foreach (var kvp in Database.instance.Attributes.Statistics)
            {
                _statistics.Add(kvp.Key, new Attribute(kvp.Key));
            }
            
            foreach (var damageTypeKVP in Database.instance.DamageTypes.Data)
            {
                _resistances.Add(damageTypeKVP.Key, new Resistance(damageTypeKVP.Value, 0));
            }

            foreach (Resistance raceResistance in race.Resistances)
            {
                _resistances[raceResistance.DamageType].SetResistance(raceResistance);
            }
        }
        
        public void Setup(EnemyDefinition definition)
        {
            _characteristics.Clear();
            _vitals.Clear();
            _statistics.Clear();
            
            foreach (var startingVital in definition.StartingVitals)
            {
                int value = Random.Range(startingVital.Value.MinimumValue, startingVital.Value.MaximumValue + 1);
                _vitals.Add(startingVital.Key, new Attribute(startingVital.Key));
                _vitals[startingVital.Key].Setup(value);
            }

            foreach (var startingStatistic in definition.StartingStatistic)
            {
                int value = Random.Range(startingStatistic.Value.MinimumValue, startingStatistic.Value.MaximumValue + 1);
                _statistics.Add(startingStatistic.Key, new Attribute(startingStatistic.Key));
                _statistics[startingStatistic.Key].Setup(value);
            }
        }

        // public void LoadData(AttributesSaveData saveData)
        // {
        //     _characteristics = Attribute.ConvertToDictionary(saveData.Characteristics);
        //     _vitals = Attribute.ConvertToDictionary(saveData.Vitals);
        //     _statistics = Attribute.ConvertToDictionary(saveData.Statistics);
        // }

        private void ResetModifiers()
        {
            foreach (KeyValuePair<string,Attribute> kvp in _characteristics)
            {
                kvp.Value.ResetModifier();
            }
            
            foreach (KeyValuePair<string,Attribute> kvp in _vitals)
            {
                kvp.Value.ResetModifier();
            }
            
            foreach (KeyValuePair<string,Attribute> kvp in _statistics)
            {
                kvp.Value.ResetModifier();
            }
        }
        
        public void CalculateAttributes()
        {
            ResetModifiers();
            CalculateCharacteristicModifiers();
            
            _vitals["Life"].Setup(Random.Range(_raceDefinition.StartingVitals["Life"].MinimumValue, _raceDefinition.StartingVitals["Life"].MinimumValue + 1) + 
                                  (_characteristics["Endurance"].Maximum + _characteristics["Might"].Maximum) / 2);
            _vitals["Stamina"].Setup(Random.Range(_raceDefinition.StartingVitals["Stamina"].MinimumValue, _raceDefinition.StartingVitals["Stamina"].MinimumValue + 1) + 
                                  (_characteristics["Endurance"].Maximum + _characteristics["Spirit"].Maximum) / 2);
            _vitals["Magic"].Setup(Random.Range(_raceDefinition.StartingVitals["Magic"].MinimumValue, _raceDefinition.StartingVitals["Magic"].MinimumValue + 1) + 
                                  (_characteristics["Intellect"].Maximum + _characteristics["Spirit"].Maximum) / 2);
            _vitals["Actions"].Setup(Random.Range(_raceDefinition.StartingVitals["Actions"].MinimumValue, _raceDefinition.StartingVitals["Actions"].MinimumValue + 1));

            CalculateVitalModifiers();
            
            int mightDamage = (_characteristics["Might"].Maximum - 10) / 10;
            int finesseDamage = (_characteristics["Finesse"].Maximum - 10) / 10;
            int magicDamage = ((_characteristics["Intellect"].Maximum - 10) + (_characteristics["Spirit"].Maximum - 10)) / 10;
            
            _statistics["Might Damage"].Setup(mightDamage + Random.Range(_raceDefinition.StartingStatistics["Might Damage"].MinimumValue, _raceDefinition.StartingStatistics["Might Damage"].MinimumValue + 1));
            _statistics["Finesse Damage"].Setup(finesseDamage + Random.Range(_raceDefinition.StartingStatistics["Finesse Damage"].MinimumValue, _raceDefinition.StartingStatistics["Finesse Damage"].MinimumValue + 1));
            _statistics["Magic Damage"].Setup(magicDamage + Random.Range(_raceDefinition.StartingStatistics["Magic Damage"].MinimumValue, _raceDefinition.StartingStatistics["Magic Damage"].MinimumValue + 1));
            
            _statistics["Block"].Setup(Random.Range(_raceDefinition.StartingStatistics["Block"].MinimumValue, _raceDefinition.StartingStatistics["Block"].MinimumValue + 1) + 
                                     (_characteristics["Might"].Maximum + _characteristics["Endurance"].Maximum) / 2);
            _statistics["Dodge"].Setup(Random.Range(_raceDefinition.StartingStatistics["Dodge"].MinimumValue, _raceDefinition.StartingStatistics["Dodge"].MinimumValue + 1) + 
                                      (_characteristics["Finesse"].Maximum + _characteristics["Senses"].Maximum) / 2);
            _statistics["Willpower"].Setup(Random.Range(_raceDefinition.StartingStatistics["Willpower"].MinimumValue, _raceDefinition.StartingStatistics["Willpower"].MinimumValue + 1) + 
                                       (_characteristics["Endurance"].Maximum + _characteristics["Spirit"].Maximum) / 2);
            
            _statistics["Perception"].Setup(Random.Range(_raceDefinition.StartingStatistics["Perception"].MinimumValue, _raceDefinition.StartingStatistics["Perception"].MinimumValue + 1) + 
                                            _characteristics["Finesse"].Maximum + _characteristics["Senses"].Maximum);
            _statistics["Movement"].Setup(_raceDefinition.StartingStatistics["Movement"].MinimumValue);
            _statistics["Critical Damage"].Setup(_raceDefinition.StartingStatistics["Critical Damage"].MinimumValue);
            
            CalculateStatisticModifiers();
        }

        public void CalculateCharacteristicModifiers()
        {
            foreach (Item equippedItem in _inventory.Equipment)
            {
                if (equippedItem == null) continue;

                if (equippedItem.PrefixEnchantKey != "")
                {
                    CalculateEnchantModifiers(equippedItem.PrefixDefinition, _characteristics, AttributeTypes.Characteristic);
                }
                
                if (equippedItem.SuffixEnchantKey != "")
                {
                    CalculateEnchantModifiers(equippedItem.SuffixDefinition, _characteristics, AttributeTypes.Characteristic);
                }
            }
            
            CalculateUnitEffectModifiers(_characteristics, AttributeTypes.Characteristic);
        }

        public void CalculateVitalModifiers()
        {
            foreach (Item equippedItem in _inventory.Equipment)
            {
                if (equippedItem == null) continue;

                if (equippedItem.PrefixEnchantKey != "")
                {
                    CalculateEnchantModifiers(equippedItem.PrefixDefinition, _vitals, AttributeTypes.Vital);
                }
                
                if (equippedItem.SuffixEnchantKey != "")
                {
                    CalculateEnchantModifiers(equippedItem.SuffixDefinition, _vitals, AttributeTypes.Vital);
                }
            }
            
            CalculateUnitEffectModifiers(_vitals, AttributeTypes.Vital);
        }
        
        public void CalculateStatisticModifiers()
        {
            foreach (Item equippedItem in _inventory.Equipment)
            {
                if (equippedItem == null) continue;

                if (equippedItem.PrefixEnchantKey != "")
                {
                    CalculateEnchantModifiers(equippedItem.PrefixDefinition, _statistics, AttributeTypes.Statistic);
                }
                
                if (equippedItem.SuffixEnchantKey != "")
                {
                    CalculateEnchantModifiers(equippedItem.SuffixDefinition, _statistics, AttributeTypes.Statistic);
                }
            }
            
            CalculateUnitEffectModifiers(_statistics, AttributeTypes.Statistic);
        }
        
        private void CalculateEnchantModifiers(EnchantmentDefinition enchant, AttributeDictionary attributes, AttributeTypes type)
        {
            foreach (EnchantmentEffect enchantmentEffect in enchant.Effects)
            {
                if (enchantmentEffect.GetType() == typeof(ModifyAttributeEnchantmentEffect))
                {
                    ModifyAttributeEnchantmentEffect modifyAttributeEffect = (ModifyAttributeEnchantmentEffect)enchantmentEffect;
                            
                    if (modifyAttributeEffect.AttributeDefinition == null || modifyAttributeEffect.AttributeDefinition.AttributeType != type) continue;
                            
                    attributes[modifyAttributeEffect.AttributeDefinition.Key].Modify(modifyAttributeEffect.Modifier);
                }
            }
        }

        private void CalculateUnitEffectModifiers(AttributeDictionary attributes, AttributeTypes type)
        {
            foreach (UnitEffect unitEffect in _unitEffects.Effects)
            {
                if (unitEffect.GetType() == typeof(ModifyAttributeUnitEffect))
                {
                    ModifyAttributeUnitEffect modifyAttributeEffect = (ModifyAttributeUnitEffect)unitEffect;
                            
                    if (modifyAttributeEffect.AttributeDefinition == null || modifyAttributeEffect.AttributeDefinition.AttributeType != type) continue;
                            
                    attributes[modifyAttributeEffect.AttributeDefinition.Key].Modify(modifyAttributeEffect.Modifier);
                }
            }
        }

        public Attribute GetAttribute(string key)
        {
            return _characteristics[key];
        }

        public Attribute GetVital(string key)
        {
            return _vitals[key];
        }

        public Attribute GetStatistic(string key)
        {
            return _statistics[key];
        }

        public Resistance GetResistance(string key)
        {
            return _resistances[key];
        }

        public void RefreshActions()
        {
            GetVital("Actions").Refresh();
        }

        public void ModifyVital(string key, int amount)
        {
            _vitals[key].Damage(amount);
        }
    }

    // [System.Serializable]
    // public class AttributesSaveData
    // {
    //     [SerializeField] private List<Attribute> _characteristics = null;
    //     [SerializeField] private List<Attribute> _vitals = null;
    //     [SerializeField] private List<Attribute> _statistics = null;
    //     //[SerializeField] private List<Resistance> _resistances = null;
    //
    //     public List<Attribute> Characteristics => _characteristics;
    //     public List<Attribute> Vitals => _vitals;
    //     public List<Attribute> Statistics => _statistics;
    //     //public List<Resistance> Resistances => _resistances;
    //
    //     public AttributesSaveData(Hero hero)
    //     {
    //         // _characteristics = Attribute.ConvertToList(hero.Attributes.Characteristics);
    //         // _vitals = Attribute.ConvertToList(hero.Attributes.Vitals);
    //         // _statistics = Attribute.ConvertToList(hero.Attributes.Statistics);
    //         //_resistances = Attribute.SaveAttributes(attributes.Resistances;
    //     }
    // }
}