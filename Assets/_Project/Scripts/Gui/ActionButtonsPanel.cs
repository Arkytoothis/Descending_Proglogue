using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using Descending.Core;
using TMPro;
using UnityEngine;

namespace Descending
{
    public class ActionButtonsPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _actionButtonPrefab = null;
        [SerializeField] private Transform _actionButtonsParent = null;
        [SerializeField] private Transform _abilitiesParent = null;

        private List<ActionButton> _actionButtons = null;
        private List<ActionButton> _abilityButtons = null;
        
        public void Setup()
        {
            ActionManager.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
            ActionManager.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
            _actionButtons = new List<ActionButton>();
            _abilityButtons = new List<ActionButton>();
        }

        private void CreateActionButtons()
        {
            _actionButtonsParent.ClearTransform();
            _abilitiesParent.ClearTransform();
            _actionButtons.Clear();
            _abilityButtons.Clear();
            
            Unit selectedUnit = UnitManager.Instance.SelectedHero;
            int abilityHotkey = 1;
            int itemHotkey = 1;
            
            foreach (BaseAction action in selectedUnit.ActionController.Actions)
            {
                GameObject clone = Instantiate(_actionButtonPrefab, _actionButtonsParent);
                ActionButton actionButton = clone.GetComponent<ActionButton>();
                actionButton.SetAction(action);

                if (action.GetType() == typeof(MeleeAttackAction))
                {
                    if (selectedUnit.GetMeleeWeapon() != null)
                    {
                        actionButton.SetIcon(selectedUnit.GetMeleeWeapon().Icon);
                    }
                }
                else if (action.GetType() == typeof(RangedAttackAction))
                {
                    if (selectedUnit.GetRangedWeapon() != null)
                    {
                        actionButton.SetIcon(selectedUnit.GetRangedWeapon().Icon);
                    }
                }
                
                if (action.GetType() == typeof(AbilityAction))
                {
                    clone.transform.SetParent(_abilitiesParent);
                    actionButton.SetAbility(((AbilityAction)action).Ability);
                    actionButton.SetHotkey(abilityHotkey.ToString());
                    abilityHotkey++;
                    _abilityButtons.Add(actionButton);
                }
                else if (action.GetType() == typeof(ItemAction))
                {
                    actionButton.SetItem(((ItemAction)action).Item);
                    actionButton.SetHotkey("s" + itemHotkey);
                    itemHotkey++;
                    _actionButtons.Add(actionButton);
                }
                else
                {
                    _actionButtons.Add(actionButton);
                }
            }
        }

        public void OnSelectHero(GameObject heroObject)
        {
            CreateActionButtons();
            UpdateSelectedVisual();
        }

        private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
        {
            UpdateSelectedVisual();
        }

        private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
        {
        }

        private void UpdateSelectedVisual()
        {
            foreach (ActionButton actionButton in _actionButtons)
            {
                actionButton.UpdateSelectedVisual();
            }
            
            foreach (ActionButton actionButton in _abilityButtons)
            {
                actionButton.UpdateSelectedVisual();
            }
        }
    }
}
