using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using UnityEngine;

namespace Descending.Party
{
    public class PartyController : MonoBehaviour
    {
        [SerializeField] private List<Transform> _formation = null;
        
        private HeroUnit _leaderHero = null;
        
        public void SetPartyLeader(int index)
        {
            for (int i = 0; i < HeroManager_Overworld.Instance.HeroUnits.Count; i++)
            {
                HeroUnit hero = HeroManager_Overworld.Instance.HeroUnits[i];
                hero.transform.position = _formation[i].position;

                HeroPathfinder heroPathfinder = hero.GetComponent<HeroPathfinder>();
                heroPathfinder.SetTarget(_formation[i]);
            }
        }
    }
}