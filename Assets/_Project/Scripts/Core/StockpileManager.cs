using System.Collections;
using System.Collections.Generic;
using System.IO;
using Descending.Equipment;
using ScriptableObjectArchitecture;
using Sirenix.Serialization;
using UnityEngine;

namespace Descending.Core
{
    public class StockpileManager : MonoBehaviour
    {
        public static StockpileManager Instance { get; private set; }
        public const int MAX_STOCKPILE_SLOTS = 96;

        [SerializeField] private BoolEvent onSyncStockpile = null;
        
        [SerializeField] private List<Item> _items = null;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple Stockpile Managers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        public void Setup()
        {
            _items = new List<Item>();
            
            for (int i = 0; i < MAX_STOCKPILE_SLOTS; i++)
            {
                _items.Add(null);    
            }

            for (int i = 0; i < 20; i++)
            {
                AddItem(ItemGenerator.GenerateRandomItem(Database.instance.Rarities.GetRarity("Legendary"), 10, 10, 10));
            }
        }

        public Item GetItem(int index)
        {
            return _items[index];
        }

        public void AddItem(Item item)
        {
            int index = -1;

            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i] == null)
                {
                    index = i;
                    break;
                }
            }

            if (index > -1)
            {
                _items[index] = new Item(item);
            }
        }

        public void OnItemPickedUp(Item item)
        {
            AddItem(item);
            onSyncStockpile.Invoke(true);
        }

        public void ClearItem(int index)
        {
            _items[index] = null;
        }

        public void SetItem(Item item, int index)
        {
            _items[index] = new Item(item);
        }

        public void SyncStockpile()
        {
            onSyncStockpile.Invoke(true);
        }
        
        public void SaveState(string filePath)
        {
            byte[] bytes = SerializationUtility.SerializeValue(_items, DataFormat.JSON);
            File.WriteAllBytes(filePath, bytes);
        }
        
        public void LoadState(string filePath)
        {
            if (!File.Exists(filePath)) return; // No state to load
	
            byte[] bytes = File.ReadAllBytes(filePath);
            _items = SerializationUtility.DeserializeValue<List<Item>>(bytes, DataFormat.JSON);
            
            SyncStockpile();
        }
    }
}