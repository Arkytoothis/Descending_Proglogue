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
            ActionManager.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
            ActionManager.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
            _actionButtons = new List<ActionButton>();
        }

        private void CreateActionButtons()
        {
            _actionButtonsParent.ClearTransform();
            _actionButtons.Clear();
            
            Unit selectedUnit = UnitManager.Instance.SelectedHero;
            foreach (BaseAction action in selectedUnit.Actions)
            {
                GameObject clone = Instantiate(_actionButtonPrefab, _actionButtonsParent);
                ActionButton actionButton = clone.GetComponent<ActionButton>();
                actionButton.SetAction(action);
                _actionButtons.Add(actionButton);
            }
        }

        public void OnSelectHero(GameObject heroObject)
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
            Unit unit = UnitManager.Instance.SelectedHero;
            if (unit == null) return;
            
            _actionPointsLabel.SetText("AP: " + unit.GetActions().Current + "/" + unit.GetActions().Maximum);
        }

        public void OnTurnChanged(bool b)
        {
            UpdateActionPoints();
        }

        private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }
    }
}
