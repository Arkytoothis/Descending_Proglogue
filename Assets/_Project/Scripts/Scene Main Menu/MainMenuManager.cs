using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Attributes;
using Descending.Core;
using Descending.Equipment;
using Descending.Units;
using UnityEngine;

namespace Descending.Scene_Main_Menu
{
    public class MainMenuManager : MonoBehaviour
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
            ItemGenerator.Setup();
            
            SpawnGui();
            PartyBuilder.Instance.SpawnHero(0, Database.instance.Races.GetRace("Godkin"), Database.instance.Profession.GetProfession("Soldier"));
        }

        private void SpawnGui()
        {
            GameObject clone = Instantiate(_guiPrefab, _guiParent);
            _guiManager = clone.GetComponent<GuiManager>();
            _guiManager.Setup();
        }
    }
}