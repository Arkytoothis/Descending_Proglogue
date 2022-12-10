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
        [SerializeField] private WorldFeatureEvent onSetCurrentFeature = null;

        public MarketData MarketData => _marketData;

        private void Start()
        {
            FeatureManager.Instance.RegisterFeature(this);
        }

        public override void Approach()
        {
            Debug.Log("Approaching Village");
            UnitManager.Instance.SavePartyPosition();
            onSetVillageButtonInteractable.Invoke(true);
            onSetCurrentFeature.Invoke(this);
        }

        public override void Leave()
        {
            Debug.Log("Leaving Village");
            onSetVillageButtonInteractable.Invoke(false);
            onSetCurrentFeature.Invoke(null);
        }

        public override void Setup()
        {
            _marketData.GenerateShopItems();
        }
    }
}