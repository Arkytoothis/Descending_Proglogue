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

        public MarketData MarketData => _marketData;

        public override void RegisterFeature(WorldFeature feature)
        {
            
        }

        public override void Interact()
        {
            Debug.Log("Interacting with Village");
            UnitManager.Instance.SavePartyPosition();
            onSetVillageButtonInteractable.Invoke(true);
            onSetCurrentVillage.Invoke(this);
        }

        public override void Leave()
        {
            Debug.Log("Leaving Village");
            onSetVillageButtonInteractable.Invoke(false);
            onSetCurrentVillage.Invoke(null);
        }
    }
}