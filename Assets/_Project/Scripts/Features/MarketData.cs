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

        private void Start()
        {
            _shopItems = new List<Item>();
            GenerateShopItems();
        }
        
        private void GenerateShopItems()
        {
            int numItemToGenerate = 20;

            for (int i = 0; i < numItemToGenerate; i++)
            {
                Item item = ItemGenerator.GenerateRandomItem(Database.instance.Rarities.GetRarity("Uncommon"), 5, 5, 5);
                _shopItems.Add(item);
            }
        }
    }
}