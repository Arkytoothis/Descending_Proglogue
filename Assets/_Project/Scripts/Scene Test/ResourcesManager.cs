using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Core
{
    public class ResourcesManager : MonoBehaviour
    {
        public static ResourcesManager Instance { get; private set; }

        [SerializeField] private int _coins = 0;
        [SerializeField] private int _supplies = 0;
        [SerializeField] private int _materials = 0;
        [SerializeField] private int _gems = 0;

        [SerializeField] private IntEvent onUpdateCoins = null;
        [SerializeField] private IntEvent onUpdateSupplies = null;
        [SerializeField] private IntEvent onUpdateMaterials = null;
        [SerializeField] private IntEvent onUpdateGems = null;
        
        public int Coins => _coins;
        public int Supplies => _supplies;
        public int Materials => _materials;
        public int Gems => _gems;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple Resource Managers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }
        
        public void Setup(int coins, int supplies, int materials, int gems)
        {
            AddCoins(coins);
            AddSupplies(supplies);
            AddMaterials(materials);
            AddGems(gems);
        }

        public void AddCoins(int coins)
        {
            _coins += coins;
            onUpdateCoins.Invoke(_coins);
            //Debug.Log("Coins Added: " + coins + "/" + _coins);
        }

        public void AddSupplies(int supplies)
        {
            _supplies += supplies;
            onUpdateSupplies.Invoke(_supplies);
        }

        public void AddMaterials(int materials)
        {
            _materials += materials;
            onUpdateMaterials.Invoke(_materials);
        }

        public void AddGems(int gems)
        {
            _gems += gems;
            onUpdateGems.Invoke(_gems);
        }
    }
}
