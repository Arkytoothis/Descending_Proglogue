using System.Collections;
using System.Collections.Generic;
using Descending.Attributes;
using Descending.Core;
using Descending.Units;
using Descending.Abilities;
using UnityEngine;

namespace Descending.Abilities
{
    public class AbilityController : MonoBehaviour
    {
        [SerializeField] private List<Ability> _memorizedPowers = null;
        [SerializeField] private List<Ability> _memorizedSpells = null;
        //[SerializeField] private List<Ability> _traits = null;

        [SerializeField] private List<ActionConfig> _actionConfigs = null;
        
        public List<Ability> MemorizedPowers => _memorizedPowers;
        public List<Ability> MemorizedSpells => _memorizedSpells;
        public List<ActionConfig> ActionConfigs => _actionConfigs;

        public void Setup(RaceDefinition race, ProfessionDefinition profession, SkillsController skills)
        {
            _actionConfigs = new List<ActionConfig>();
            FindStartingAbilities(race, profession, skills);
            LoadActionConfigs();
        }

        private void FindStartingAbilities(RaceDefinition race, ProfessionDefinition profession, SkillsController skills)
        {
            foreach (var abilityKvp in Database.instance.Abilities.Abilities)
            {
                if (skills.ContainsSkills(abilityKvp.Value.Details.Skill) && skills.GetSkill(abilityKvp.Value.Details.Skill.Key).Current >= abilityKvp.Value.Details.MinimumSkill)
                {
                    if (abilityKvp.Value.Details.AbilityType == AbilityType.Power)
                    {
                        _memorizedPowers.Add(new Ability(abilityKvp.Value));
                    }
                    else if (abilityKvp.Value.Details.AbilityType == AbilityType.Spell)
                    {
                        _memorizedSpells.Add(new Ability(abilityKvp.Value));
                    }
                }
            }
        }

        private void LoadActionConfigs()
        {
            for (int i = 0; i < _memorizedPowers.Count; i++)
            {
                AddAbility(_memorizedPowers[i]);
            }
            
            for (int i = 0; i < _memorizedSpells.Count; i++)
            {
                AddAbility(_memorizedSpells[i]);
            }
        }

        private void AddAbility(Ability ability)
        {
            //Debug.Log("Adding " + ability.Definition.Details.Name);
            _actionConfigs.Add(new ActionConfig(ability));
        }
    }
}