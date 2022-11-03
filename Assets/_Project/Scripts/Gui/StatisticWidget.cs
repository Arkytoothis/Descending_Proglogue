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

        public void SetAttribute(Attribute attribute)
        {
            _currentLabel.SetText(attribute.Current.ToString());
            _maximumLabel.SetText(attribute.Maximum.ToString());
        }
    }
}