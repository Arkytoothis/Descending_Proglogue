using System.Collections;
using System.Collections.Generic;
using Descending.Treasure;
using UnityEngine;

namespace Descending.Units
{
    public class EnemyUnit : Unit
    {
        [SerializeField] protected UnitData _unitData = null;
        [SerializeField] private EnemyDefinition _definition = null;

        private bool _treasureDropped = false;

        public UnitData UnitData => _unitData;

        public void SetupEnemy(EnemyDefinition definition)
        {
            _isEnemy = true;
            _definition = definition;
            _treasureDropped = false;
            _healthSystem.Setup(100);
            _worldPanel.Setup(this);

            UnitManager.Instance.UnitSpawned(this);
            //Deactivate();
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
                TreasureManager.Instance.SpawnCoins(transform.position, Random.Range(dropData.Minimum, dropData.Maximum), coinType, 0.3f);
            }
        }

        private void TryDropGems(GemTypes gemType)
        {
            DropData dropData = _definition.GemData[(int) gemType];
            
            if (Random.Range(0, 100) < dropData.Chance)
            {
                TreasureManager.Instance.SpawnGems(transform.position, Random.Range(dropData.Minimum, dropData.Maximum), gemType, 0.3f);
            }
        }

        public void Activate()
        {
            _modelParent.gameObject.SetActive(false);
            _isActive = true;
        }
        
        public void Deactivate()
        {
            _modelParent.gameObject.SetActive(false);
            _isActive = false;
        }

        public override string GetFullName()
        {
            return _definition.Name;
        }

        public override string GetShortName()
        {
            return _definition.Name;
        }
    }
}