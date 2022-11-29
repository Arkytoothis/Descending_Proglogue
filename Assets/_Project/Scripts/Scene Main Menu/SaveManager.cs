using System.Collections;
using System.Collections.Generic;
using System.IO;
using Descending.Core;
using Descending.Units;
using Sirenix.Serialization;
using UnityEngine;

namespace Descending.Scene_Main_Menu
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple UnitManagers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        public void SaveState()
        {
            UnitManager.Instance.SaveState(Database.instance.PartyDataFilePath);
        }

        public void LoadState()
        {
            UnitManager.Instance.LoadState(Database.instance.PartyDataFilePath);
        }
    }
}