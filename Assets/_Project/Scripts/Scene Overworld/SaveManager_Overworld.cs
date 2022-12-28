using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Units;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Descending.Scene_Overworld
{
    public class SaveManager_Overworld : SaveManager
    {
        [FormerlySerializedAs("_heroManager")] [FormerlySerializedAs("_unitManager")] [SerializeField] private HeroManager_Combat _heroManagerCombat = null;
        [SerializeField] private StockpileManager _stockpileManager = null;
        [SerializeField] private ResourcesManager _resourcesManager = null;
        [SerializeField] private WorldGenerator _worldGenerator = null;
        
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
            _worldGenerator.SaveState();
        }

        public override void LoadState()
        {
            _resourcesManager.LoadState();
            _stockpileManager.LoadState();
            _heroManagerCombat.LoadState();
            _worldGenerator.LoadState();
            
            SaveState();
        }

        [Button("Generate Data")]
        public void SetGenerateData()
        {
            _loadState = SaveManagerLoadStates.Generating;
            _heroManagerCombat.SetLoadData(false);
            _resourcesManager.SetLoadData(false);
            _stockpileManager.SetLoadData(false);
            _worldGenerator.SetLoadData(false);
        }
        
        [Button("Load Data")]
        public void SetLoadData()
        {
            _loadState = SaveManagerLoadStates.Loading;
            _heroManagerCombat.SetLoadData(true);
            _resourcesManager.SetLoadData(true);
            _stockpileManager.SetLoadData(true);
            _worldGenerator.SetLoadData(true);
        }
    }
}