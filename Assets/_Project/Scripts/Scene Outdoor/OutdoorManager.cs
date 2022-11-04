using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Combat;
using Descending.Core;
using Descending.Enemies;
using Descending.Gui;
using Descending.Tiles;
using Descending.Units;
using DunGen;
using UnityEngine;

namespace Descending.Scene_Outdoor
{
    public class OutdoorManager : MonoBehaviour
    {
        [SerializeField] private Database _database = null;
        [SerializeField] private EnemyAI _enemyAI = null;
        [SerializeField] private RuntimeDungeon _dungeon = null;
        [SerializeField] private GameObject _guiPrefab = null;
        [SerializeField] private Transform _guiParent = null;

        private GuiManager _guiManager = null;
        
        private void Awake()
        {
        }

        private void Start()
        {
            _database.Setup();
            InputManager.Instance.Setup();
            TurnManager.Instance.Setup();
            MapManager.Instance.Setup();
            _dungeon.Generate();
            UnitManager.Instance.Setup();
            _enemyAI.Setup();
            SpawnGui();
            ResourcesManager.Instance.Setup(100, 10, 0, 0);
            
            UnitManager.Instance.SyncHeroes();
        }

        public void OnRegisterPlayerSpawner(GameObject spawnerObject)
        {
            PlayerSpawner spawner = spawnerObject.GetComponent<PlayerSpawner>();
            UnitManager.Instance.RegisterPlayerSpawner(spawner);
            ActionManager.Instance.Setup();
            TileMapRenderer.Instance.Setup();
        }

        private void SpawnGui()
        {
            GameObject clone = Instantiate(_guiPrefab, _guiParent);
            _guiManager = clone.GetComponent<GuiManager>();
            _guiManager.Setup();
        }
    }
}