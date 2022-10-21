using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Descending.Gui
{
    public class UnitWorldPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _actionPointsLabel = null;
        [SerializeField] private Image _healthBarImage = null;
        [SerializeField] private Unit _unit = null;
        [SerializeField] private HealthSystem _healthSystem = null;

        private void Start()
        {
            Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
            _healthSystem.OnDamaged += HealthSystem_OnDamaged;
            UpdateActionPoints();
            UpdateHealth();
        }

        private void UpdateActionPoints()
        {
            if (_unit == null) return;
            
            _actionPointsLabel.SetText(_unit.ActionPoints.ToString());
        }

        private void UpdateHealth()
        {
            if (_healthSystem == null) return;
            
            _healthBarImage.fillAmount = _healthSystem.GetHealthNormalized();
        }

        private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }

        private void HealthSystem_OnDamaged(object sender, EventArgs e)
        {
            UpdateHealth();
        }
    }
}