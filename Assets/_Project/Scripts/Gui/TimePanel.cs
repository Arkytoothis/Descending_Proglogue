using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniStorm;
using UnityEngine;

namespace Descending.Scene_Overworld.Gui
{
    public class TimePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timeLabel = null;

        public void Setup()
        {
            UniStormSystem.Instance.OnHourChangeEvent.AddListener(OnUpdateTime);
            UniStormSystem.Instance.OnDayChangeEvent.AddListener(OnUpdateTime);
            UniStormSystem.Instance.OnMonthChangeEvent.AddListener(OnUpdateTime);
            UniStormSystem.Instance.OnYearChangeEvent.AddListener(OnUpdateTime);
            
            UpdateTime();
        }

        private void OnUpdateTime()
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            _timeLabel.SetText("Year: " + UniStormSystem.Instance.Year + " Month: " + UniStormSystem.Instance.Month + " Day: " + UniStormSystem.Instance.Day + " Hour: " + UniStormSystem.Instance.Hour); 
        }
    }
}
