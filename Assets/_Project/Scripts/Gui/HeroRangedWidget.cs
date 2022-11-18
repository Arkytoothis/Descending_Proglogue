using System.Collections;
using System.Collections.Generic;
using Descending.Equipment;
using Descending.Units;
using TMPro;
using UnityEngine;

namespace Descending.Gui
{
    public class HeroRangedWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _rangedWeaponLabel = null;
        [SerializeField] private TMP_Text _ammoLabel = null;
        [SerializeField] private TMP_Text _rangedWeaponDetailsLabel = null;
        [SerializeField] private TMP_Text _ammoDetailsLabel = null;
        [SerializeField] private HeroItemWidget _rangedWidget = null;
        [SerializeField] private HeroItemWidget _ammoWidget = null;
        
        public void DisplayHero(HeroUnit hero)
        {
            Item rangedWeapon = hero.Inventory.GetRangedWeapon();
            Item ammo = hero.Inventory.GetAmmo();
            
            if (rangedWeapon != null)
            {
                _rangedWeaponLabel.SetText(rangedWeapon.DisplayName());
                _rangedWeaponDetailsLabel.SetText(rangedWeapon.GetItemWidgetText());
                _rangedWidget.SetItem(rangedWeapon);
            }
            else
            {
                _rangedWeaponLabel.SetText("No Ranged Weapon");
                _rangedWeaponDetailsLabel.SetText("");
                _rangedWidget.SetItem(null);
            }
            
            if (ammo != null)
            {
                _ammoLabel.SetText(ammo.DisplayName());
                _ammoDetailsLabel.SetText(ammo.GetItemWidgetText());
                _ammoWidget.SetItem(ammo);
            }
            else
            {
                _ammoLabel.SetText("No Ammo");
                _ammoDetailsLabel.SetText("");
                _ammoWidget.SetItem(null);
            }
        }
    }
}