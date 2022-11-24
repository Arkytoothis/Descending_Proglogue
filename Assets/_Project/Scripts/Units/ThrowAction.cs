using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using Descending.Equipment;
using UnityEngine;

namespace Descending.Units
{
    public class ThrowAction : BaseAction
    {
        [SerializeField] private int _range = 0;

        private UnitAnimator _unitAnimator = null;
        private Item _item = null;
        private int _itemIndex;
        
        public int Range => _range;
        public Item Item => _item;

        protected override void Awake()
        {
            _unit = GetComponentInParent<Unit>();
            _unitAnimator = _unit.UnitAnimator;
        }
        
        private void Update()
        {
            if (!_isActive) return;
            
        }

        public void SetupData()
        {
            if (_item != null)
            {
                _range = _item.GetUsableData().Range;
            }
        }
        
        public void SetItem(Item item, int index)
        {
            _item = item;
            _itemIndex = index;
            _icon = _item.ItemDefinition.Icon;
            _range = 1;
        }
        
        public override string GetName()
        {
            return "Throw";
        }

        public override void PerformAction(MapPosition mapPosition, Action onThrowComplete)
        {
            //Debug.Log("Throwing");
            ActionStart(onThrowComplete);
            GameObject clone = Instantiate(_item.GetUsableData().Projectile.Prefab, _unit.transform.position, Quaternion.identity);
            ThrowableProjectile projectile = clone.GetComponent<ThrowableProjectile>();
            projectile.Setup(_unit, mapPosition, OnThrowComplete, projectile.DamageType, projectile.Attribute);
            
            _item.Use(1);
            
            if (_item.UsesLeft <= 0)
            {
                _unit.Inventory.ClearAccessory(_itemIndex);
            }
            
            UnitManager.Instance.SelectedHero.ActionController.SetupActions();
            UnitManager.Instance.SyncHeroes();
            UnitManager.Instance.RefreshSelectedHero();
        }

        public override List<MapPosition> GetValidActionGridPositions()
        {
            List<MapPosition> validGridPositions = new List<MapPosition>();
            
            for (int x = -_range; x <= _range; x++)
            {
                for (int y = -_range; y <= _range; y++)
                {
                    MapPosition offsetMapPosition = new MapPosition(x, y);
                    MapPosition testMapPosition = _unit.CurrentMapPosition + offsetMapPosition;
                    
                    if (MapManager.Instance.IsValidGridPosition(testMapPosition) == false) continue;
                    
                    int testDistance = Mathf.Abs(x) + Mathf.Abs(y);
                    if (testDistance > _range) continue;
                    
                    if (MapManager.Instance.Linecast(_unit.CurrentMapPosition, testMapPosition)) continue;
                    
                    validGridPositions.Add(testMapPosition);
                }
            }

            return validGridPositions;
        }

        public override int GetActionPointCost()
        {
            return 1;
        }

        public override EnemyAction GetEnemyAction(MapPosition mapPosition)
        {
            return new EnemyAction
            {
                _mapPosition = mapPosition,
                ActionValue = 0
            };
        }

        private void OnThrowComplete()
        {
            ActionComplete();
        }
    }
}
