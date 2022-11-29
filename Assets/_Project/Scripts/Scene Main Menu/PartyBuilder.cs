using System.Collections;
using System.Collections.Generic;
using System.IO;
using Descending.Attributes;
using Descending.Core;
using Descending.Units;
using Sirenix.Serialization;
using UnityEngine;

namespace Descending.Scene_Main_Menu
{
    public class PartyBuilder : MonoBehaviour
    {
        public static PartyBuilder Instance { get; private set; }
        
        [SerializeField] private GameObject _heroPrefab = null;
        [SerializeField] private Transform _heroParent = null;

        private HeroUnit _hero = null;

        public HeroUnit Hero => _hero;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple PartyBuilders " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        public void SpawnHero(int listIndex, RaceDefinition race, ProfessionDefinition profession)
        {
            //Debug.Log("Spawning Hero at " + mapPosition.ToString());
            _heroParent.ClearTransform();
            GameObject clone = Instantiate(_heroPrefab, _heroParent);
            
            _hero = clone.GetComponent<HeroUnit>();
            _hero.SetupHero(Genders.Male, race, profession, listIndex);
            _hero.PortraitModel.SetActive(false);
            clone.name = "Hero: " + _hero.GetFullName();
            
            Destroy(_hero.PortraitModel);
        }

        public void LoadHero(HeroSaveData saveData)
        {
            //Debug.Log("Spawning Hero at " + mapPosition.ToString());
            _heroParent.ClearTransform();
            GameObject clone = Instantiate(_heroPrefab, _heroParent);
            
            _hero = clone.GetComponent<HeroUnit>();
            _hero.LoadHero(saveData);
            clone.name = "Hero: " + _hero.GetFullName();
            
            Destroy(_hero.PortraitModel);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveState(Database.instance.PartyDataFilePath);
            }
            else if (Input.GetKeyDown(KeyCode.F9))
            {
                LoadState(Database.instance.PartyDataFilePath);
            }
        }
        
        public void SaveState(string filePath)
        {
            byte[] bytes = SerializationUtility.SerializeValue(new HeroSaveData(_hero), DataFormat.JSON);
            File.WriteAllBytes(filePath, bytes);
        }
        
        public void LoadState(string filePath)
        {
            if (!File.Exists(filePath)) return; // No state to load
	
            byte[] bytes = File.ReadAllBytes(filePath);
            HeroSaveData saveData = SerializationUtility.DeserializeValue<HeroSaveData>(bytes, DataFormat.JSON);
            LoadHero(saveData);
        }
    }
}