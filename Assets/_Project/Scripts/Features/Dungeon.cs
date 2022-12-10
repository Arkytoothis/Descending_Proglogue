using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Features
{
    public class Dungeon : WorldFeature
    {
        [SerializeField] private DungeonData _dungeonData = null;
        
        [SerializeField] private BoolEvent onSetDungeonButtonInteractable = null;
        [SerializeField] private WorldFeatureEvent onSetCurrentFeature = null;

        public DungeonData DungeonData => _dungeonData;

        private void Start()
        {
            FeatureManager.Instance.RegisterFeature(this);
        }

        public override void Approach()
        {
            Debug.Log("Approaching Dungeon");
            UnitManager.Instance.SavePartyPosition();
            onSetDungeonButtonInteractable.Invoke(true);
            onSetCurrentFeature.Invoke(this);
        }

        public override void Leave()
        {
            Debug.Log("Leaving Dungeon");
            onSetDungeonButtonInteractable.Invoke(false);
            onSetCurrentFeature.Invoke(null);
        }

        public override void Setup()
        {
            _dungeonData.GenerateData(0);
        }
    }
}