using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Attribute = Descending.Attributes.Attribute;

namespace Descending.Gui
{
    public class StatisticWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentLabel = null;
        [SerializeField] private TMP_Text _maximumLabel = null;

        public void SetAttribute(int value)
        {
            if (value < 0)
            {
                _currentLabel.SetText("-" + value);
            }
            else
            {
                _currentLabel.SetText("+" + value);
            }
            
            _maximumLabel.SetText("");
        }
        
        public void SetAttribute(string value)
        {
            _currentLabel.SetText(value);
            _maximumLabel.SetText("");
        }
    }
}