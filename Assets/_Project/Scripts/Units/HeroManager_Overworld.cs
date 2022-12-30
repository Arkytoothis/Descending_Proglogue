using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Descending.Attributes;
using Descending.Core;
using Descending.Party;
using Descending.Tiles;
using ScriptableObjectArchitecture;
using Sirenix.Serialization;
using UnityEngine;

namespace Descending.Units
{
    public class HeroManager_Overworld : HeroManager
    {
        public static HeroManager_Overworld Instance { get; private set; }

        [SerializeField] private float _unitScaleFactor = 5f;
        [SerializeField] private PartyController _partyController = null;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple HeroManagers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            _heroUnits = new List<HeroUnit>();
        }

        public override void Setup()
        {
            
        }

        public override void SyncHeroes()
        {
            //Debug.Log("Syncing Heroes");
            onSyncParty.Invoke(true);
            PortraitRoom.Instance.RefreshCameras();
            
            foreach (HeroUnit heroUnit in _heroUnits)
            {
                heroUnit.SyncWorldPanel();
            }
        }
        
        public override void UnitSpawned(Unit unit)
        {
            if (unit.IsEnemy == false)
            {
                _heroUnits.Add(unit as HeroUnit);
                onSyncParty.Invoke(true);
            }
        }

        public override void UnitDead(Unit unit)
        {
            if (unit.IsEnemy == false)
            {
                _heroUnits.Remove(unit as HeroUnit);
            }
            
            onSyncParty.Invoke(true);
        }

        public void RegisterPlayerSpawner(PlayerSpawner spawner)
        {
            _playerSpawner = spawner;
        }

        public override void SpawnHeroes()
        {
            if (_loadData == true)
            {
                LoadState();
            }
            else
            {
                GenerateHeroes();
            }
        }

        public override HeroUnit GetHero(int index)
        {
            return _heroUnits[index] as HeroUnit;
        }

        public override void RefreshSelectedHero()
        {
            SelectHero(_selectedHero);
        }
        
        public override void SelectHero(HeroUnit hero)
        {
            _selectedHero = hero;
            
            for (int i = 0; i < _heroUnits.Count; i++)
            {
                if (hero.HeroData.ListIndex == i)
                {
                    _heroUnits[i].Select();
                }
                else
                {
                    _heroUnits[i].Deselect();
                }
            }
            
            SetPartyLeader(_selectedHero);
            //Debug.Log(_selectedHero.GetShortName() + " selected");
        }
        
        public override void SelectHeroDefault()
        {
            _selectedHero = _heroUnits[0];
            SelectHero(_selectedHero);
            SetPartyLeader(_selectedHero);
        }
        

        public void HideHeroes(Transform parent)
        {
            for (int i = 0; i < _heroUnits.Count; i++)
            {
                _heroUnits[i].WorldModel.transform.SetParent(parent, false);
            }
        }

        public void SetPartyLeader(HeroUnit hero)
        {
            if (_partyController != null)
            {
                _partyController.SetPartyLeader(hero.HeroData.ListIndex);
            }
            
            onSelectHero.Invoke(hero.gameObject);
        }

        public override void AwardExperience(int experience)
        {
            foreach (HeroUnit heroUnit in _heroUnits)
            {
                heroUnit.AddExperience(experience);
            }
        }

        public override void RecalculateHeroAttributes()
        {
            foreach (HeroUnit heroUnit in _heroUnits)
            {
                heroUnit.RecalculateAttributes();
            }
            
            SyncHeroes();
        }
        
        public override void SaveState()
        {
            PartySaveData saveData = new PartySaveData(_heroUnits);
            byte[] saveDataBytes = SerializationUtility.SerializeValue(saveData, DataFormat.JSON);
            File.WriteAllBytes(Database.instance.PartyDataFilePath, saveDataBytes);
            SavePartyPosition();
        }

        public void SavePartyPosition()
        {
            byte[] positionBytes = SerializationUtility.SerializeValue(new PartyPositionSaveData(_partyController.transform.position), DataFormat.JSON);
            File.WriteAllBytes(Database.instance.OverworldSpawnFilePath, positionBytes);
        }

        public override void GenerateHeroes()
        {
            _heroUnits = new List<HeroUnit>();
            SpawnHero(new MapPosition(),0, Database.instance.Races.GetRace("Half Orc"), Database.instance.Profession.GetProfession("Soldier"));
            SpawnHero(new MapPosition(),1, Database.instance.Races.GetRace("Wild Elf"), Database.instance.Profession.GetProfession("Scout"));
            SpawnHero(new MapPosition(),2, Database.instance.Races.GetRace("Imperial"), Database.instance.Profession.GetProfession("Acolyte"));
            SpawnHero(new MapPosition(),3, Database.instance.Races.GetRace("Valarian"), Database.instance.Profession.GetProfession("Apprentice"));
            
            PortraitRoom.Instance.Setup();
            PortraitRoom.Instance.SyncParty();
        }
        
        protected override void SpawnHero(MapPosition mapPosition, int listIndex, RaceDefinition race, ProfessionDefinition profession)
        {
            //Debug.Log("Spawning Hero at " + mapPosition.ToString());
            GameObject clone = Instantiate(_heroPrefab, _heroesParent);
            
            HeroUnit heroUnit = clone.GetComponent<HeroUnit>();
            heroUnit.SetupHero(Genders.Male, race, profession, listIndex, Database.instance.OverworldHeroAnimator);
            heroUnit.WorldModel.transform.localScale = new Vector3(_unitScaleFactor, _unitScaleFactor, _unitScaleFactor);
            clone.name = "Hero: " + heroUnit.GetFullName();
            
            _heroUnits.Add(heroUnit);
        }
        
        protected override void LoadHero(MapPosition mapPosition, HeroSaveData saveData)
        {
            //Debug.Log("Spawning Hero at " + mapPosition.ToString());
            GameObject clone = Instantiate(_heroPrefab, _heroesParent);
            
            HeroUnit heroUnit = clone.GetComponent<HeroUnit>();
            heroUnit.LoadHero(saveData, Database.instance.OverworldHeroAnimator);
            heroUnit.WorldModel.transform.localScale = new Vector3(_unitScaleFactor, _unitScaleFactor, _unitScaleFactor);
            clone.name = "Hero: " + heroUnit.GetFullName();
            
            _heroUnits.Add(heroUnit);
        }
        
        public override void LoadState()
        {
            if (!File.Exists(Database.instance.PartyDataFilePath)) return; // No state to load
	
            byte[] bytes = File.ReadAllBytes(Database.instance.PartyDataFilePath);
            PartySaveData saveData = SerializationUtility.DeserializeValue<PartySaveData>(bytes, DataFormat.JSON);

            //_partyController.transform.position = saveData.WorldPosition;
            _heroesParent.ClearTransform();
            _heroUnits.Clear();

            for (int i = 0; i < saveData.Heroes.Length; i++)
            {
                LoadHero(new MapPosition(), saveData.Heroes[i]);
            }
            
            PortraitRoom.Instance.Setup();
            PortraitRoom.Instance.SyncParty();
            LoadOverworldSpawn();
        }

        private void LoadOverworldSpawn()
        {
            if (!File.Exists(Database.instance.OverworldSpawnFilePath)) return; // No state to load
	
            byte[] bytes = File.ReadAllBytes(Database.instance.OverworldSpawnFilePath);
            PartyPositionSaveData saveData = SerializationUtility.DeserializeValue<PartyPositionSaveData>(bytes, DataFormat.JSON);
            _partyController.transform.position = saveData.WorldPosition;
        }

        public override void SetLoadData(bool loadData)
        {
            _loadData = loadData;
        }
    }
}