using System.Collections;
using System.Collections.Generic;
using Descending.Dropables;
using Descending.Interactables;
using Descending.Units;
using UnityEngine;

namespace Descending.Tiles
{
    [System.Serializable]
    public class Tile
    {
        [SerializeField] private List<CoinDrop> _coins = null;
        [SerializeField] private List<GemDrop> _gems = null;
        
        private TileMap<Tile> tileMap = null;
        private MapPosition mapPosition;
        private List<Unit> _units = null;
        private IInteractable _interactable = null;

        public IInteractable Interactable => _interactable;
        public MapPosition MapPosition => mapPosition;

        public Tile(TileMap<Tile> tileMap, MapPosition mapPosition)
        {
            this.tileMap = tileMap;
            this.mapPosition = mapPosition;
            _units = new List<Unit>();
            _coins = new List<CoinDrop>();
            _gems = new List<GemDrop>();
        }

        public override string ToString()
        {
            string unitsString = "";
            foreach (Unit unit in _units)
            {
                if (unit != null)
                {
                    unitsString += unit.name + "\n";
                }
            }

            return mapPosition.ToString() + "\n" + unitsString;
        }

        public void AddUnit(Unit unit)
        {
            _units.Add(unit);
        }

        public void RemoveUnit(Unit unit)
        {
            _units.Remove(unit);
        }

        public void AddCoinDrop(CoinDrop coinDrop)
        {
            _coins.Add(coinDrop);
        }


        public void AddGemDrop(GemDrop gemDrop)
        {
            _gems.Add(gemDrop);
        }
        
        public List<Unit> GetUnitList()
        {
            return _units;
        }

        public bool HasAnyUnit()
        {
            return _units.Count > 0;
        }

        public Unit GetUnit()
        {
            if (HasAnyUnit())
            {
                return _units[0];
            }
            else
            {
                return null;
            }
        }

        public void SetInteractable(IInteractable interactable)
        {
            _interactable = interactable;
        }

        public int GetCoinValue()
        {
            int coinValue = 0;

            for (int i = 0; i < _coins.Count; i++)
            {
                coinValue += _coins[i].Coins;
            }

            return coinValue;
        }

        public int GetGemValue()
        {
            int gemValue = 0;

            for (int i = 0; i < _gems.Count; i++)
            {
                gemValue += _gems[i].Gems;
            }

            return gemValue;
        }

        public void ClearCoins()
        {
            for (int i = 0; i < _coins.Count; i++)
            {
                Object.Destroy(_coins[i].gameObject);
            }
            
            _coins.Clear();
        }

        public void ClearGems()
        {
            for (int i = 0; i < _gems.Count; i++)
            {
                Object.Destroy(_gems[i].gameObject);
            }
            
            _gems.Clear();
        }
    }
}
