using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Descending.Abilities;
using UnityEngine;

namespace Descending.Units
{
    public class ActionController : MonoBehaviour
    {
        [SerializeField] private Transform _projectileSpawnPoint = null;
        [SerializeField] private List<BaseAction> _actions = null;
        
        private Unit _unit = null;
        
        public List<BaseAction> Actions => _actions;
        
        public void Setup(Unit unit)
        {
            _unit = unit;
            _actions = GetComponents<BaseAction>().ToList();

            foreach (Ability power in _unit.Abilities.MemorizedPowers)
            {
                AbilityAction abilityAction = gameObject.AddComponent<AbilityAction>();
                abilityAction.SetAbility(power, _projectileSpawnPoint);
                _actions.Add(abilityAction);
            }
            
            foreach (Ability spell in _unit.Abilities.MemorizedSpells)
            {
                AbilityAction abilityAction = gameObject.AddComponent<AbilityAction>();
                abilityAction.SetAbility(spell, _projectileSpawnPoint);
                _actions.Add(abilityAction);
            }
        }

        public T GetAction<T>() where T : BaseAction
        {
            foreach (BaseAction action in _actions)
            {
                if (action is T)
                {
                    return (T)action;
                }
            }

            return null;
        }
    }
}