using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Scene_Overworld;
using Descending.Units;
using UnityEngine;

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
            SpawnGui();
            _worldGenerator.BuildWorld();
            UnitManager.Instance.SpawnHeroesOverworld();
            ResourcesManager.Instance.Setup(100, 10, 0, 0);
        }

        private void SpawnGui()
        {
            GameObject clone = Instantiate(_guiPrefab, _guiParent);
            _guiManager = clone.GetComponent<GuiManager>();
            _guiManager.Setup();
        }
    }
}