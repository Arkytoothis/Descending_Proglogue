using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using UnityEngine;

namespace Descending.Party
{
    [System.Serializable]
    public class PartySaveData
    {
        private HeroSaveData[] _heroes = null;

        public HeroSaveData[] Heroes => _heroes;

        public PartySaveData()
        {
            _heroes = null;
        }

        public PartySaveData(List<HeroUnit> heroList)
        {
            _heroes = new HeroSaveData[heroList.Count];
            
            for (int i = 0; i < heroList.Count; i++)
            {
                _heroes[i] = new HeroSaveData(heroList[i]);
            }
        }
    }
}