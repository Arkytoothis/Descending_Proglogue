using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

        public void Setup(Unit unit)
        {
            if (unit == null) return;
            
            UpdateActionPoints(unit);
            UpdateHealth(unit.HealthSystem);
        }

        public void UpdateActionPoints(Unit unit)
        {
            _actionPointsLabel.SetText(unit.GetActionsCurrent().ToString());
        }

        public void UpdateHealth(HealthSystem healthSystem)
        {
            if (healthSystem == null) return;
            
            _healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
        }
    }
}