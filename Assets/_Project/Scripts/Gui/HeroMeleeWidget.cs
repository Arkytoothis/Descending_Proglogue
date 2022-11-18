using System.Collections;
using System.Collections.Generic;
using Descending.Equipment;
using Descending.Units;
using TMPro;
using UnityEngine;

namespace Descending.Gui
{
    public class HeroMeleeWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _meleeWeaponLabel = null;
        [SerializeField] private TMP_Text _offhandItemLabel = null;
        [SerializeField] private TMP_Text _meleeWeaponDetailsLabel = null;
        [SerializeField] private TMP_Text _offhandItemDetailsLabel = null;
        [SerializeField] private HeroItemWidget _meleeWidget = null;
        [SerializeField] private HeroItemWidget _offhandWidget = null;
        
        public void DisplayHero(HeroUnit hero)
        {
            Item meleeWeapon = hero.Inventory.GetMeleeWeapon();
            Item offhandItem = hero.Inventory.GetOffhandItem();
            
            if (meleeWeapon != null)
            {
                _meleeWeaponLabel.SetText(meleeWeapon.DisplayName());
                _meleeWeaponDetailsLabel.SetText(meleeWeapon.GetItemWidgetText());
                _meleeWidget.SetItem(meleeWeapon);
            }
            else
            {
                _meleeWeaponLabel.SetText("No Melee Weapon");
                _meleeWeaponDetailsLabel.SetText("");
                _meleeWidget.SetItem(null);
            }
            
            if (offhandItem != null)
            {
                _offhandItemLabel.SetText(offhandItem.DisplayName());
                _offhandItemDetailsLabel.SetText(offhandItem.GetItemWidgetText());
                _offhandWidget.SetItem(offhandItem);
            }
            else
            {
                _offhandItemLabel.SetText("No Offhand Item");
                _offhandItemDetailsLabel.SetText("");
                _offhandWidget.SetItem(null);
            }
        }
    }
}