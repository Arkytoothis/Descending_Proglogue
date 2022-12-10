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
        [SerializeField] private Button _villageButton = null;
        [SerializeField] private Button _dungeonButton = null;
        [SerializeField] private List<PartyMemberWidget_Overworld> _partyMemberWidgets = null;

        [SerializeField] private BoolEvent onOpenVillageWindow = null;
        [SerializeField] private BoolEvent onOpenDungeonWindow = null;
        
        public void Setup()
        {
            _villageButton.interactable = false;
            _dungeonButton.interactable = false;
        }
        
        public void SyncParty(bool b)
        {
            for (int i = 0; i < _partyMemberWidgets.Count; i++)
            {
                _partyMemberWidgets[i].Clear();
            }

            //Debug.Log("Syncing Party Data");
            for (int i = 0; i < UnitManager.Instance.HeroUnits.Count; i++)
            {
                _partyMemberWidgets[i].Setup(UnitManager.Instance.GetHero(i));
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

        public void OnSetVillageButtonInteractable(bool interactable)
        {
            _villageButton.interactable = interactable;
        }

        public void OnSetDungeonButtonInteractable(bool interactable)
        {
            _dungeonButton.interactable = interactable;
        }

        public void VillageButtonClick()
        {
            onOpenVillageWindow.Invoke(true);
        }

        public void DungeonButtonClick()
        {
            onOpenDungeonWindow.Invoke(true);
        }
    }
}