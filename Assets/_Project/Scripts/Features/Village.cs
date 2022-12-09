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
        [SerializeField] private WorldFeatureEvent onRegisterFeature = null;

        public MarketData MarketData => _marketData;

        private void Awake()
        {
            onRegisterFeature.Invoke(this);
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

        public override void Setup()
        {
            _marketData.GenerateShopItems();
        }
    }
}