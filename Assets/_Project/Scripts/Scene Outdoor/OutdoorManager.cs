using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Combat;
using Descending.Combat_Events;
using Descending.Core;
using Descending.Enemies;
using Descending.Equipment;
using Descending.Gui;
using Descending.Tiles;
using Descending.Units;
using DunGen;
using UnityEngine;
using UnityEngine.SceneManagement;
using MapManager = Descending.Tiles.MapManager;

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
            Setup();
        }

        private void Setup()
        {
            _database.Setup();
            ItemGenerator.Setup();
            InputManager.Instance.Setup();
            TurnManager.Instance.Setup();
            MapManager.Instance.Setup();
            CombatEventManager.Instance.Setup();
            _dungeon.Generate();
            HeroManager_Combat.Instance.Setup();
            EnemyManager.Instance.Setup();
            _enemyAI.Setup();
            SpawnGui();
            ResourcesManager.Instance.Setup();
            StockpileManager.Instance.Setup();
            HeroManager_Combat.Instance.SyncHeroes();
        }

        public void OnRegisterPlayerSpawner(GameObject spawnerObject)
        {
            PlayerSpawner spawner = spawnerObject.GetComponent<PlayerSpawner>();
            HeroManager_Combat.Instance.RegisterPlayerSpawner(spawner);
            ActionManager.Instance.Setup();
            TileMapRenderer.Instance.Setup();
        }

        private void SpawnGui()
        {
            GameObject clone = Instantiate(_guiPrefab, _guiParent);
            _guiManager = clone.GetComponent<GuiManager>();
            _guiManager.Setup();
        }

        public void OnEndCombat(bool b)
        {
            Debug.Log("Ending Combat");
            
            StartCoroutine(DelayedLoadEndCombat());
        }
        
        private IEnumerator DelayedLoadEndCombat()
        {
            yield return new WaitForSeconds(1f);

            SaveManager.Instance.SaveState();
            SceneManager.LoadScene((int)GameScenes.Overworld);
        }
    }
}