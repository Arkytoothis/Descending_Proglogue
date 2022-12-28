using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using UnityEngine;

namespace Descending.Gui
{
    public class PartyPanel : MonoBehaviour
    {
        [SerializeField] private List<PartyMemberWidget> _partyMemberWidgets = null;

        public void Setup()
        {
            
        }
        
        public void SyncParty(bool b)
        {
            for (int i = 0; i < _partyMemberWidgets.Count; i++)
            {
                _partyMemberWidgets[i].Clear();
            }

            //Debug.Log("Syncing Party Data");
            for (int i = 0; i < HeroManager_Combat.Instance.HeroUnits.Count; i++)
            {
                _partyMemberWidgets[i].Setup(HeroManager_Combat.Instance.GetHero(i));
            }
        }

        public void OnSelectHero(GameObject heroObject)
        {
            HeroUnit hero = heroObject.GetComponent<HeroUnit>();
            
            if (hero == null) return;
            
            for (int i = 0; i < _partyMemberWidgets.Count; i++)
            {
                if (hero.HeroData.ListIndex == i)
                {
                    _partyMemberWidgets[i].Select();
                }
                else
                {
                    _partyMemberWidgets[i].Deselect();
                }
            }
        }
    }
}