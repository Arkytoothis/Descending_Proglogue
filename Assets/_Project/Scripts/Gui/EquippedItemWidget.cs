using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Equipment;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Descending.Gui
{
    public class EquippedItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _iconImage = null;
        [SerializeField] private TMP_Text _stackSizeLabel = null;

        private Item _item = null;
        
        public void SetItem(Item item)
        {
            _item = item;

            if (_item != null)
            {
                _iconImage.sprite = item.Icon;
                _stackSizeLabel.SetText("1");
            }
            else
            {
                Clear();
            }
        }

        private void Clear()
        {
            _iconImage.sprite = Database.instance.BlankSprite;
            _stackSizeLabel.SetText("");
        }
    }
}