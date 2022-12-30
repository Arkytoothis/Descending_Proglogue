using System.Collections;
using System.Collections.Generic;
using System.IO;
using Descending.Attributes;
using Descending.Core;
using Descending.Party;
using Descending.Units;
using Sirenix.Serialization;
using UnityEngine;

namespace Descending.Scene_Main_Menu
{
    public class PartyBuilder : MonoBehaviour
    {
        public static PartyBuilder Instance { get; private set; }
        
        [SerializeField] private GameObject _heroPrefab = null;
        [SerializeField] private List<Transform> _heroMounts = null;

        private List<HeroUnit> _heroes = null;

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

        public void SpawnHeroes()
        {
            _heroes = new List<HeroUnit>();
            
            SpawnHero(0, Database.instance.Races.GetRace("Half Orc"), Database.instance.Profession.GetProfession("Soldier"));
            SpawnHero(1, Database.instance.Races.GetRace("Wild Elf"), Database.instance.Profession.GetProfession("Scout"));
            SpawnHero(2, Database.instance.Races.GetRace("Mountain Dwarf"), Database.instance.Profession.GetProfession("Acolyte"));
            SpawnHero(3, Database.instance.Races.GetRace("Valarian"), Database.instance.Profession.GetProfession("Apprentice"));
        }
        
        private void SpawnHero(int listIndex, RaceDefinition race, ProfessionDefinition profession)
        {
            //Debug.Log("Spawning Hero at " + mapPosition.ToString());
            
            _heroMounts[listIndex].ClearTransform();
            GameObject clone = Instantiate(_heroPrefab, _heroMounts[listIndex]);
            
            HeroUnit hero = clone.GetComponent<HeroUnit>();
            hero.SetupHero(Genders.Male, race, profession, listIndex, Database.instance.OverworldHeroAnimator);
            hero.SetWorldPanelAActive(false);
            hero.PortraitModel.SetActive(false);
            clone.name = "Hero: " + hero.GetFullName();
            
            Destroy(hero.PortraitModel);
            _heroes.Add(hero);
        }

        private void LoadHeroes(PartySaveData saveData)
        {
            for (int i = 0; i < saveData.Heroes.Length; i++)
            {
                LoadHero(saveData.Heroes[i]);
            }
        }
        
        private void LoadHero(HeroSaveData saveData)
        {
            //Debug.Log("Spawning Hero at " + mapPosition.ToString());
            _heroMounts[saveData.ListIndex].ClearTransform();
            GameObject clone = Instantiate(_heroPrefab, _heroMounts[saveData.ListIndex]);
            
            HeroUnit hero = clone.GetComponent<HeroUnit>();
            hero.LoadHero(saveData, Database.instance.OverworldHeroAnimator);
            clone.name = "Hero: " + hero.GetFullName();
            
            Destroy(hero.PortraitModel);
            _heroes.Add(hero);
        }
        
        public void SaveState(string filePath)
        {
            PartySaveData saveData = new PartySaveData(_heroes);
            byte[] bytes = SerializationUtility.SerializeValue(saveData, DataFormat.JSON);
            File.WriteAllBytes(filePath, bytes);
        }
        
        public void LoadState(string filePath)
        {
            if (!File.Exists(filePath)) return; // No state to load
	
            byte[] bytes = File.ReadAllBytes(filePath);
            PartySaveData saveData = SerializationUtility.DeserializeValue<PartySaveData>(bytes, DataFormat.JSON);
            LoadHeroes(saveData);
        }
    }
}