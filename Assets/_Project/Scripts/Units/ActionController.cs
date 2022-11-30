using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Descending.Abilities;
using Descending.Core;
using Descending.Equipment;
using UnityEngine;

namespace Descending.Units
{
    public class ActionController : MonoBehaviour
    {
        [SerializeField] private List<BaseAction> _actions = null;
        
        private Unit _unit = null;
        
        public List<BaseAction> Actions => _actions;

        private void Awake()
        {
            _unit = GetComponentInParent<Unit>();
        }

        public void SetupActions()
        {
            _actions = new List<BaseAction>();

            SetupDefaultActions();
            SetupAbilityActions();
            SetupItemActions();
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

        private void SetupDefaultActions()
        {
            MeleeAttackAction meleeAction = gameObject.AddComponent<MeleeAttackAction>();
            _actions.Add(meleeAction);
            
            RangedAttackAction rangedAction = gameObject.AddComponent<RangedAttackAction>();
            _actions.Add(rangedAction);
            
            MoveAction moveAction = gameObject.AddComponent<MoveAction>();
            _actions.Add(moveAction);
            
            JumpAction jumpAction = gameObject.AddComponent<JumpAction>();
            _actions.Add(jumpAction);
            
            InteractAction interactAction = gameObject.AddComponent<InteractAction>();
            _actions.Add(interactAction);
            
            SearchAction searchAction = gameObject.AddComponent<SearchAction>();
            _actions.Add(searchAction);
        }

        private void SetupAbilityActions()
        {
            foreach (Ability power in _unit.Abilities.MemorizedPowers)
            {
                AbilityAction abilityAction = gameObject.AddComponent<AbilityAction>();
                abilityAction.SetAbility(power);
                _actions.Add(abilityAction);
            }
            
            foreach (Ability spell in _unit.Abilities.MemorizedSpells)
            {
                AbilityAction abilityAction = gameObject.AddComponent<AbilityAction>();
                abilityAction.SetAbility(spell);
                _actions.Add(abilityAction);
            }
        }

        private void SetupItemActions()
        {
            int index = 0;
            foreach (Item accessory in _unit.Inventory.Accessories)
            {
                if (accessory != null && accessory.Key != "")
                {
                    if (accessory.GetUsableData().UsableType == UsableTypes.Bomb)
                    {
                        ThrowAction throwAction = gameObject.AddComponent<ThrowAction>();
                        throwAction.SetItem(accessory, index);
                        _actions.Add(throwAction);
                    }
                    else
                    {
                        ItemAction itemAction = gameObject.AddComponent<ItemAction>();
                        itemAction.SetItem(accessory, index);
                        _actions.Add(itemAction);
                    }
                }

                index++;
            }
        }
    }
}