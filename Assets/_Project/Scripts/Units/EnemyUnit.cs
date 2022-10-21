using System.Collections;
using System.Collections.Generic;
using Descending.Treasure;
using UnityEngine;

namespace Descending.Units
{
    public class EnemyUnit : Unit
    {
        [SerializeField] private EnemyDefinition _definition = null;

        private bool _treasureDropped = false;
        
        public void SetupEnemy(EnemyDefinition definition)
        {
            _isEnemy = true;
            _definition = definition;
            _treasureDropped = false;
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
            DropData dropData = _definition.CoinData[(int) coinType];
            
            if (Random.Range(0, 100) < dropData.Chance)
            {
                TreasureManager.Instance.SpawnCoins(transform.position, Random.Range(dropData.Minimum, dropData.Maximum), coinType);
            }
        }

        private void TryDropGems(GemTypes gemType)
        {
            DropData dropData = _definition.GemData[(int) gemType];
            
            if (Random.Range(0, 100) < dropData.Chance)
            {
                TreasureManager.Instance.SpawnGems(transform.position, Random.Range(dropData.Minimum, dropData.Maximum), gemType);
            }
            
        }
    }
}