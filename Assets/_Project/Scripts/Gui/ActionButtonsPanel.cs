using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using Descending.Core;
using Descending.Equipment;
using Descending.Scene_Overworld;
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

        public void CreateActionButtons()
        {
            _actionButtonsParent.ClearTransform();
            _abilitiesParent.ClearTransform();
            _actionButtons.Clear();
            _abilityButtons.Clear();
            
            Unit selectedUnit = UnitManager.Instance.SelectedHero;
            if (selectedUnit == null || selectedUnit.IsEnemy == true) return;
            
            int abilityHotkey = 1;
            //int itemHotkey = 1;

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
                    else
                    {
                        actionButton.SetIcon(Database.instance.DefaultMeleeActionIcon);
                    }
                }
                else if (action.GetType() == typeof(RangedAttackAction))
                {
                    if (selectedUnit.GetRangedWeapon() != null)
                    {
                        actionButton.SetIcon(selectedUnit.GetRangedWeapon().Icon);
                    }
                    else
                    {
                        actionButton.SetIcon(Database.instance.BlankSprite);
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
                else if (action.GetType() == typeof(ThrowAction))
                {
                    Item item = ((ThrowAction)action).Item;
                    UsableData usableData = item.GetUsableData();
                    actionButton.SetItem(item);
                    actionButton.SetHotkey(item.UsesLeft + "/" + usableData.MaxUses);
                    _actionButtons.Add(actionButton);
                }
                else if (action.GetType() == typeof(ItemAction))
                {
                    Item item = ((ItemAction)action).Item;
                    UsableData usableData = item.GetUsableData();
                    actionButton.SetItem(item);
                    actionButton.SetHotkey(item.UsesLeft + "/" + usableData.MaxUses);
                    _actionButtons.Add(actionButton);
                }
                else if (action.GetType() == typeof(MoveAction))
                {
                    actionButton.SetIcon(Database.instance.MoveActionIcon);
                    _actionButtons.Add(actionButton);
                }
                else if (action.GetType() == typeof(JumpAction))
                {
                    actionButton.SetIcon(Database.instance.JumpActionIcon);
                    _actionButtons.Add(actionButton);
                }
                else if (action.GetType() == typeof(InteractAction))
                {
                    actionButton.SetIcon(Database.instance.InteractActionIcon);
                    _actionButtons.Add(actionButton);
                }
                else if (action.GetType() == typeof(SearchAction))
                {
                    actionButton.SetIcon(Database.instance.SearchActionIcon);
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
