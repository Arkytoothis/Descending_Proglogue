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
        [SerializeField] private Image _armorImage = null;
        [SerializeField] private Image _lifeImage = null;

        public void Setup(Unit unit)
        {
            if (unit == null) return;
            
            UpdateActionPoints(unit);
            UpdateHealth(unit.HealthSystem);
        }

        public void UpdateActionPoints(Unit unit)
        {
            _actionPointsLabel.SetText(unit.GetActions().Current + "/" + unit.GetActions().Maximum);
        }

        public void UpdateHealth(HealthSystem healthSystem)
        {
            if (healthSystem == null) return;
            
            _armorImage.fillAmount = healthSystem.GetVitalNormalized("Armor");
            _lifeImage.fillAmount = healthSystem.GetVitalNormalized("Life");
        }
    }
}