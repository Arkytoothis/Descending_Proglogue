using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Combat;
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
        [SerializeField] private TMP_Text _actionPointsLabel = null;

        private List<ActionButton> _actionButtons = null;
        
        public void Setup()
        {
            ActionManager.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
            ActionManager.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
            ActionManager.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
            TurnManager.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
            //Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
            _actionButtons = new List<ActionButton>();
            //UpdateActionPoints();
            //CreateActionButtons();
            //UpdateSelectedVisual();
        }

        private void CreateActionButtons()
        {
            _actionButtonsParent.ClearTransform();
            _actionButtons.Clear();
            
            Unit selectedUnit = ActionManager.Instance.SelectedUnit;
            foreach (BaseAction action in selectedUnit.Actions)
            {
                GameObject clone = Instantiate(_actionButtonPrefab, _actionButtonsParent);
                ActionButton actionButton = clone.GetComponent<ActionButton>();
                actionButton.SetAction(action);
                _actionButtons.Add(actionButton);
            }
        }

        private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
        {
            UpdateActionPoints();
            CreateActionButtons();
            UpdateSelectedVisual();
        }

        private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
        {
            UpdateSelectedVisual();
        }

        private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }

        private void UpdateSelectedVisual()
        {
            foreach (ActionButton actionButton in _actionButtons)
            {
                actionButton.UpdateSelectedVisual();
            }
        }

        private void UpdateActionPoints()
        {
            Unit unit = ActionManager.Instance.SelectedUnit;
            if (unit == null) return;
            
            _actionPointsLabel.SetText("Action Points: " + unit.GetActionsCurrent());
        }

        private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }

        private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }
    }
}
