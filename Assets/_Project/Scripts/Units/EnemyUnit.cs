using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Equipment;
using Descending.Tiles;
using Descending.Treasure;
using UnityEngine;

namespace Descending.Units
{
    public class EnemyUnit : Unit
    {
        [SerializeField] protected UnitData _unitData = null;
        [SerializeField] private EnemyDefinition _definition = null;
        [SerializeField] private Transform _rightHandMount = null;
        [SerializeField] private Transform _leftHandMount = null;

        private bool _treasureDropped = false;
        private Item _meleeWeapon = null;
        private Item _rangedWeapon = null;
        
        public UnitData UnitData => _unitData;

        public void SetupEnemy(EnemyDefinition definition)
        {
            _isEnemy = true;
            _definition = definition;
            _treasureDropped = false;
            
            _attributes.Setup(_definition);
            
            if(_meleeWeapon != null)
            {
                _meleeWeapon = ItemGenerator.GenerateItem(_definition.MeleeWeapon);
                EquipWeapon(_meleeWeapon);
            }

            if (_rangedWeapon != null)
            {
                _rangedWeapon = ItemGenerator.GenerateItem(_definition.RangedWeapon);
                EquipWeapon(_rangedWeapon);
            }

            _healthSystem.Setup(_attributes);
            _worldPanel.Setup(this);

            UnitManager.Instance.UnitSpawned(this);
            //Activate();
            Deactivate();
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
            _modelParent.gameObject.SetActive(true);
            _worldPanel.gameObject.SetActive(true);
            _isActive = true;
        }
        
        public void Deactivate()
        {
            _modelParent.gameObject.SetActive(false);
            _worldPanel.gameObject.SetActive(false);
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

        public override Item GetMeleeWeapon()
        {
            return _meleeWeapon;
        }

        public override Item GetRangedWeapon()
        {
            return _rangedWeapon;
        }
        
        private void EquipWeapon(Item item)
        {
            if (item == null || item.Key == "" || item.GetWeaponData() == null) return;

            if (item.ItemDefinition.Hands == Hands.Right)
            {
                _rightHandMount.ClearTransform();
                GameObject clone = item.SpawnItemModel(_rightHandMount, 0);
                //_animationController.SetOverride(item.GetWeaponData().AnimatorOverride);
                
            }
            else if (item.ItemDefinition.Hands == Hands.Left)
            {
                _leftHandMount.ClearTransform();
                GameObject clone = item.SpawnItemModel(_leftHandMount, 0);
                //_animationController.SetOverride(item.GetWeaponData().AnimatorOverride);
            }
        }
        
        public override void Damage(GameObject attacker, int damage)
        {
            if (_isAlive == false) return;
            
            _healthSystem.TakeDamage(attacker, damage);

            if (GetHealth() <= 0)
            {
                Dead();
            }
        }

        protected override void Dead()
        {
            _isAlive = false;
            MapManager.Instance.RemoveUnitAtGridPosition(currentMapPosition, this);
            UnitManager.Instance.UnitDead(this);
            _ragdollSpawner.Activate(_healthSystem);
            Destroy(gameObject);
        }
    }
}