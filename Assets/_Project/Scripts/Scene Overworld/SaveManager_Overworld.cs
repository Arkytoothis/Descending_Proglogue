using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Units;
using UnityEngine;

namespace Descending.Scene_Overworld
{
    public class SaveManager_Overworld : SaveManager
    {
        [SerializeField] private WorldGenerator _worldGenerator = null;
        
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
            ResourcesManager.Instance.SaveState(Database.instance.ResourceDataFilePath);
            StockpileManager.Instance.SaveState(Database.instance.StockpileFilePath);
            UnitManager.Instance.SaveState(Database.instance.PartyDataFilePath);
            _worldGenerator.SaveState(Database.instance.WorldDataFilePath);
        }

        public override void LoadState()
        {
            ResourcesManager.Instance.LoadState(Database.instance.ResourceDataFilePath);
            StockpileManager.Instance.LoadState(Database.instance.StockpileFilePath);
            UnitManager.Instance.LoadState_Overworld(Database.instance.PartyDataFilePath);
            _worldGenerator.LoadState(Database.instance.WorldDataFilePath);
        }
    }
}