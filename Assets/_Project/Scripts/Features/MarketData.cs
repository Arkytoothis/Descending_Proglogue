using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Equipment;
using UnityEngine;

namespace Descending.Features
{
    [System.Serializable]
    public class MarketData : MonoBehaviour
    {
        [SerializeField] private List<Item> _shopItems = null;

        public List<Item> ShopItems => _shopItems;   
         
        

        private void Start()
        {
            _shopItems = new List<Item>();
            GenerateShopItems();
        }
        
        private void GenerateShopItems()
        {
            int numWeaponsToGenerate = 5;

            for (int i = 0; i < numWeaponsToGenerate; i++)
            {
                Item item = ItemGenerator.GenerateRandomItem(Database.instance.Rarities.GetRarity("Uncommon"), GenerateItemType.Any_Weapon, 0, 0, 0);
                _shopItems.Add(item);
            }
            
            int numArmorToGenerate = 5;

            for (int i = 0; i < numArmorToGenerate; i++)
            {
                Item item = ItemGenerator.GenerateRandomItem(Database.instance.Rarities.GetRarity("Uncommon"), GenerateItemType.Any_Armor, 0, 0, 0);
                _shopItems.Add(item);
            }
            
            int numShieldsToGenerate = 2;

            for (int i = 0; i < numShieldsToGenerate; i++)
            {
                Item item = ItemGenerator.GenerateRandomItem(Database.instance.Rarities.GetRarity("Uncommon"), GenerateItemType.Any_Shield, 0, 0, 0);
                _shopItems.Add(item);
            }
            
            int numAccessoriesToGenerate = 5;

            for (int i = 0; i < numAccessoriesToGenerate; i++)
            {
                Item item = ItemGenerator.GenerateRandomItem(Database.instance.Rarities.GetRarity("Uncommon"), GenerateItemType.Any_Accessory, 0, 0, 0);
                _shopItems.Add(item);
            }
        }
    }
}