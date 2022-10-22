using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using Descending.Treasure;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Descending.Interactables
{
    public class DestructibleCrate : MonoBehaviour, IDamagable
    {
        public static event EventHandler OnAnyInteractableDestroyed;

        [SerializeField] private GameObject _debrisEffectPrefab = null;
        [SerializeField] private GameObject _smokeEffectPrefab = null;
        [SerializeField] private List<DropData> _coinData;
        [SerializeField] private List<DropData> _gemData;
        
        private MapPosition mapPosition;
        private int _health = 10;
        private int _maxHealth = 10;
        private bool _treasureDropped = false;

        public MapPosition MapPosition => mapPosition;
        public int Health => _health;

        private void Start()
        {
            mapPosition = MapManager.Instance.GetGridPosition(transform.position);
            _treasureDropped = false;
        }

        public void Damage(int damage)
        {
            _health -= damage;

            //Debug.Log(name + " takes " + damage + " damage, " + _health + " health remaining");
            if (_health <= 0)
            {
                Destroy();
            }
        }

        private void Destroy()
        {
            Instantiate(_debrisEffectPrefab, transform.position, Quaternion.identity);
            Instantiate(_smokeEffectPrefab, transform.position, Quaternion.identity);
            DropTreasure();
            OnAnyInteractableDestroyed?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
        
        public void DropTreasure()
        {
            if (_treasureDropped == true) return;
            
            _treasureDropped = true;
            
            TryDropCoins(CoinTypes.Copper);
            TryDropCoins(CoinTypes.Silver);
            TryDropCoins(CoinTypes.Gold);
            TryDropCoins(CoinTypes.Mithril);

            TryDropGems(GemTypes.Sapphire);
            TryDropGems(GemTypes.Ruby);
            TryDropGems(GemTypes.Emerald);
            TryDropGems(GemTypes.Diamond);
        }

        private void TryDropCoins(CoinTypes coinType)
        {
            DropData dropData = _coinData[(int) coinType];
            
            if (Random.Range(0, 100) < dropData.Chance)
            {
                TreasureManager.Instance.SpawnCoins(transform.position, Random.Range(dropData.Minimum, dropData.Maximum), coinType, 0.1f);
            }
        }

        private void TryDropGems(GemTypes gemType)
        {
            DropData dropData = _gemData[(int) gemType];
            
            if (Random.Range(0, 100) < dropData.Chance)
            {
                TreasureManager.Instance.SpawnGems(transform.position, Random.Range(dropData.Minimum, dropData.Maximum), gemType, 0.1f);
            }
        }
    }
}
