using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Features
{
    public class Village : WorldFeature
    {
        [SerializeField] private MarketData _marketData = null;
        
        [SerializeField] private BoolEvent onSetVillageButtonInteractable = null;
        [SerializeField] private VillageEvent onSetCurrentVillage = null;

        public override void Interact()
        {
            Debug.Log("Interacting with Village");
            UnitManager.Instance.SavePartyPosition();
            onSetVillageButtonInteractable.Invoke(true);
            onSetCurrentVillage.Invoke(this);
        }
    }
}