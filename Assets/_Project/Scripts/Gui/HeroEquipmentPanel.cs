using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Equipment;
using Descending.Units;
using UnityEngine;
using UnityEngine.UI;

namespace Descending.Gui
{
    public class HeroEquipmentPanel : MonoBehaviour
    {
        [SerializeField] private RawImage _portraitImage = null;

        [SerializeField] private EquippedItemWidget[] _equippedItemWidgets = null;
        [SerializeField] private AccessoryWidget[] _accessoryWidgets = null;

        public void Setup()
        {
            
        }
        
        public void DisplayHero(HeroUnit hero)
        {
            _portraitImage.texture = hero.Portrait.RtFar;

            for (int i = 0; i < (int)EquipmentSlots.Number; i++)
            {
                _equippedItemWidgets[i].SetItem(hero.Inventory.Equipment[i]);
            }
            
            for (int i = 0; i < InventoryController.MAX_ACCESSORY_SLOTS; i++)
            {
                _accessoryWidgets[i].SetItem(hero.Inventory.Accessories[i]);
            }
        }
    }
}