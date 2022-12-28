using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Features;
using Descending.Units;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

namespace Descending.Gui
{
    public class PartyPanel_Overworld : MonoBehaviour
    {
        [SerializeField] private List<PartyMemberWidget_Overworld> _partyMemberWidgets = null;

        [SerializeField] private BoolEvent onOpenDungeonWindow = null;
        
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
            for (int i = 0; i < HeroManager_Overworld.Instance.HeroUnits.Count; i++)
            {
                _partyMemberWidgets[i].Setup(HeroManager_Overworld.Instance.GetHero(i));
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

        public void DungeonButtonClick()
        {
            onOpenDungeonWindow.Invoke(true);
        }
    }
}