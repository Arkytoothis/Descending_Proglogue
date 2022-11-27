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
        [SerializeField] private TMP_Text _armorLabel = null;
        [SerializeField] private TMP_Text _lifeLabel = null;
        [SerializeField] private Image _armorImage = null;
        [SerializeField] private Image _lifeImage = null;

        private Unit _unit = null;
        
        public void Setup(Unit unit)
        {
            if (unit == null) return;
            _unit = unit;
            
            UpdateActionPoints();
            Sync();
        }

        public void UpdateActionPoints()
        {
            _actionPointsLabel.SetText(_unit.GetActions().Current + "/" + _unit.GetActions().Maximum);
        }

        public void Sync()
        {
            if (_unit == null) return;
            
            _armorImage.fillAmount = _unit.DamageSystem.GetVitalNormalized("Armor");
            _lifeImage.fillAmount = _unit.DamageSystem.GetVitalNormalized("Life");
            _armorLabel.SetText(_unit.GetArmor().TotalCurrent() + "/" + _unit.GetArmor().TotalMaximum());
            _lifeLabel.SetText(_unit.GetLife().TotalCurrent() + "/" + _unit.GetLife().TotalMaximum());
        }

        public void OnSyncData(bool b)
        {
            Sync();
        }
    }
}