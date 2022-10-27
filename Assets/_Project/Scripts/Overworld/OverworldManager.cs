using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Party;
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
        }

        private void Start()
        {
            _database.Setup();
            SpawnGui();
            
            PartyManager.Instance.Setup();
            ResourcesManager.Instance.Setup(100, 10, 0, 0);
        }

        public void OnRegisterPlayerSpawner(GameObject spawnerObject)
        {
            PlayerSpawner spawner = spawnerObject.GetComponent<PlayerSpawner>();
            UnitManager.Instance.RegisterPlayerSpawner(spawner);
        }

        private void SpawnGui()
        {
            GameObject clone = Instantiate(_guiPrefab, _guiParent);
            _guiManager = clone.GetComponent<GuiManager>();
            _guiManager.Setup();
        }
    }
}