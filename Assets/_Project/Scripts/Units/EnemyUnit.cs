using System.Collections;
using System.Collections.Generic;
using Descending.Treasure;
using UnityEngine;

namespace Descending.Units
{
    public class EnemyUnit : Unit
    {
        [SerializeField] private EnemyDefinition _definition = null;

        public void SetupEnemy(EnemyDefinition definition)
        {
            _isEnemy = true;
            _definition = definition;
        }
        
        public void DropTreasure()
        {
            DropData copperData = _definition.CoinData[(int) CoinTypes.Copper];
            DropData silverData = _definition.CoinData[(int) CoinTypes.Silver];
            DropData goldData = _definition.CoinData[(int) CoinTypes.Gold];
            DropData mithrilData = _definition.CoinData[(int) CoinTypes.Mithril];
            
            DropData quartzData = _definition.GemData[(int) GemTypes.Sapphire];
            DropData rubyData = _definition.GemData[(int) GemTypes.Ruby];
            DropData emeraldData = _definition.GemData[(int) GemTypes.Emerald];
            DropData diamondData = _definition.GemData[(int) GemTypes.Diamond];
            
            if (Random.Range(0, 100) < copperData.Chance)
            {
                TreasureManager.Instance.SpawnCoins(transform.position, Random.Range(copperData.Minimum, copperData.Maximum), CoinTypes.Copper);
            }

            if (Random.Range(0, 100) < silverData.Chance)
            {
                TreasureManager.Instance.SpawnCoins(transform.position, Random.Range(silverData.Minimum, silverData.Maximum), CoinTypes.Silver);
            }

            if (Random.Range(0, 100) < goldData.Chance)
            {
                TreasureManager.Instance.SpawnCoins(transform.position, Random.Range(goldData.Minimum, goldData.Maximum), CoinTypes.Gold);
            }

            if (Random.Range(0, 100) < mithrilData.Chance)
            {
                TreasureManager.Instance.SpawnCoins(transform.position, Random.Range(mithrilData.Minimum, mithrilData.Maximum), CoinTypes.Mithril);
            }
            
            if (Random.Range(0, 100) < quartzData.Chance)
            {
                TreasureManager.Instance.SpawnGems(transform.position, Random.Range(quartzData.Minimum, quartzData.Maximum), GemTypes.Sapphire);
            }
            
            if (Random.Range(0, 100) < rubyData.Chance)
            {
                TreasureManager.Instance.SpawnGems(transform.position, Random.Range(rubyData.Minimum, rubyData.Maximum), GemTypes.Ruby);
            }
            
            if (Random.Range(0, 100) < emeraldData.Chance)
            {
                TreasureManager.Instance.SpawnGems(transform.position, Random.Range(emeraldData.Minimum, emeraldData.Maximum), GemTypes.Emerald);
            }
            
            if (Random.Range(0, 100) < diamondData.Chance)
            {
                TreasureManager.Instance.SpawnGems(transform.position, Random.Range(diamondData.Minimum, diamondData.Maximum), GemTypes.Diamond);
            }
        }
    }
}