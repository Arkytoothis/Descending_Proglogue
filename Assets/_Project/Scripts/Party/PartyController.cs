using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Units;
using UnityEngine;

namespace Descending.Party
{
    public class PartyController : MonoBehaviour
    {
        [SerializeField] private Transform _leaderTransform = null;
        [SerializeField] private Transform _offScreenTransform = null;
        
        private HeroUnit _leaderHero = null;
        
        public void SetPartyLeader(int index)
        {
            HeroManager_Overworld.Instance.HideHeroes(_offScreenTransform);
            
            HeroUnit hero = HeroManager_Overworld.Instance.HeroUnits[index];
            _leaderHero = hero;
            _leaderTransform.ClearTransform();
            _leaderHero.WorldModel.transform.SetParent(_leaderTransform, false);
            _leaderHero.WorldModel.transform.position = _leaderTransform.position;
        }
    }
}