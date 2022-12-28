using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Units;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Descending.Scene_Main_Menu
{
    public class SaveManager_MainMenu : SaveManager
    {
        [FormerlySerializedAs("_heroManager")] [FormerlySerializedAs("_unitManager")] [SerializeField] private HeroManager_Combat _heroManagerCombat = null;
        [SerializeField] private StockpileManager _stockpileManager = null;
        [SerializeField] private ResourcesManager _resourcesManager = null;
        
        [SerializeField] private SaveManagerLoadStates _loadState = SaveManagerLoadStates.None;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveState();
            }
            else if (Input.GetKeyDown(KeyCode.F9))
            {
                LoadState();
            }
        }

        public override void SaveState()
        {
            _resourcesManager.SaveState();
            _stockpileManager.SaveState();
            _heroManagerCombat.SaveState();
        }

        public override void LoadState()
        {
            _resourcesManager.LoadState();
            _stockpileManager.LoadState();
            _heroManagerCombat.LoadState();
            
            SaveState();
        }

        [Button("Generate Data")]
        public void SetGenerateData()
        {
            _loadState = SaveManagerLoadStates.Generating;
            _heroManagerCombat.SetLoadData(false);
            _resourcesManager.SetLoadData(false);
            _stockpileManager.SetLoadData(false);
        }
        
        [Button("Load Data")]
        public void SetLoadData()
        {
            _loadState = SaveManagerLoadStates.Loading;
            _heroManagerCombat.SetLoadData(true);
            _resourcesManager.SetLoadData(true);
            _stockpileManager.SetLoadData(true);
        }
    }
}