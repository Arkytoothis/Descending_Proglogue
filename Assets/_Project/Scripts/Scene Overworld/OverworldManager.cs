using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Equipment;
using Descending.Scene_Overworld;
using Descending.Units;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Descending.Overworld
{
    public class OverworldManager : MonoBehaviour
    {
        [SerializeField] private Database _database = null;
        [SerializeField] private WorldGenerator _worldGenerator = null;
        [SerializeField] private GameObject _guiPrefab = null;
        [SerializeField] private Transform _guiParent = null;

        private GuiManager _guiManager = null;
        
        private void Awake()
        {
        }

        private void Start()
        {
            _database.Setup();
            ItemGenerator.Setup();
            _worldGenerator.Setup();
            UnitManager.Instance.SpawnHeroes_Overworld();
            
            SpawnGui();
            
            ResourcesManager.Instance.Setup();
            StockpileManager.Instance.Setup();
            UnitManager.Instance.SelectDefaultHeroOverworld();
            UnitManager.Instance.SyncHeroes();
        }

        private void SpawnGui()
        {
            GameObject clone = Instantiate(_guiPrefab, _guiParent);
            _guiManager = clone.GetComponent<GuiManager>();
            _guiManager.Setup();
        }
    }
}