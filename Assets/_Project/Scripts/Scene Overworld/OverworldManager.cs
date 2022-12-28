using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Equipment;
using Descending.Features;
using Descending.Scene_Overworld;
using Descending.Units;
using UnityEngine;

namespace Descending.Overworld
{
    public class OverworldManager : MonoBehaviour
    {
        [SerializeField] private Database _database = null;
        [SerializeField] private GameObject _guiPrefab = null;
        [SerializeField] private Transform _guiParent = null;

        private GuiManager _guiManager = null;
        
        private void Awake()
        {
            _database.Setup();
            ItemGenerator.Setup();
        }

        private void Start()
        {
            //_worldGenerator.Setup();
            HeroManager_Overworld.Instance.SpawnHeroes();
            
            SpawnGui();
            
            ResourcesManager.Instance.Setup();
            StockpileManager.Instance.Setup();
            FeatureManager.Instance.Setup();
            HeroManager_Overworld.Instance.SelectHeroDefault();
            HeroManager_Overworld.Instance.SyncHeroes();
        }

        private void SpawnGui()
        {
            GameObject clone = Instantiate(_guiPrefab, _guiParent);
            _guiManager = clone.GetComponent<GuiManager>();
            _guiManager.Setup();
        }
    }
}