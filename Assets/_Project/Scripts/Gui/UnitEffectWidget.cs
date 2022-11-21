using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Descending.Gui
{
    public class UnitEffectWidget : MonoBehaviour
    {
        [SerializeField] private Image _iconImage = null;
        [SerializeField] private TMP_Text _durationLabel = null;

        public void Setup(Sprite icon, int duration)
        {
            _iconImage.sprite = icon;
            _durationLabel.SetText(duration.ToString());
        }
    }
}