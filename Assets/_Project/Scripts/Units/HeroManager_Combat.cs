using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Descending.Attributes;
using Descending.Core;
using Descending.Party;
using Descending.Tiles;
using Sirenix.Serialization;
using UnityEngine;

namespace Descending.Units
{
    public class HeroManager_Combat : HeroManager
    {
        public static HeroManager_Combat Instance { get; private set; }

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

        public override void SelectHeroDefault()
        {
            _selectedHero = _heroUnits[0];
            
            for (int i = 0; i < _heroUnits.Count; i++)
            {
                _heroUnits[i].Deselect();
            }
            
            _selectedHero.Select();
            ActionManager.Instance.SetSelectedAction(_selectedHero.GetAction<MoveAction>());
            onSelectHero.Invoke(_selectedHero.gameObject);
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

            if (ActionManager.Instance != null)
            {
                ActionManager.Instance.SetSelectedAction(_selectedHero.GetAction<MoveAction>());
            }
            
            onSelectHero.Invoke(_selectedHero.gameObject);
            
            //Debug.Log(_selectedHero.GetShortName() + " selected");
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
            byte[] bytes = SerializationUtility.SerializeValue(saveData, DataFormat.JSON);
            File.WriteAllBytes(Database.instance.PartyDataFilePath, bytes);
        }
        
        public override void GenerateHeroes()
        {
            MapPosition spawnerPosition = MapManager.Instance.GetGridPosition(_playerSpawner.transform.position);
            
            SpawnHero(new MapPosition(spawnerPosition.X, spawnerPosition.Y), 0, Database.instance.Races.GetRace("Godkin"), Database.instance.Profession.GetProfession("Soldier"));
            SpawnHero(new MapPosition(spawnerPosition.X + 1, spawnerPosition.Y), 1, Database.instance.Races.GetRace("Halfling"), Database.instance.Profession.GetProfession("Scout"));
            SpawnHero(new MapPosition(spawnerPosition.X, spawnerPosition.Y - 1), 2, Database.instance.Races.GetRace("Sun Elf"), Database.instance.Profession.GetProfession("Acolyte"));
            SpawnHero(new MapPosition(spawnerPosition.X + 1, spawnerPosition.Y - 1), 3, Database.instance.Races.GetRace("Valarian"), Database.instance.Profession.GetProfession("Apprentice"));
            
            PortraitRoom.Instance.Setup();
            PortraitRoom.Instance.SyncParty();
        }
        
        protected override void SpawnHero(MapPosition mapPosition, int listIndex, RaceDefinition race, ProfessionDefinition profession)
        {
            //Debug.Log("Spawning Hero at " + mapPosition.ToString());
            GameObject clone = Instantiate(_heroPrefab, _heroesParent);
            clone.transform.position = MapManager.Instance.GetWorldPosition(mapPosition);
            
            HeroUnit heroUnit = clone.GetComponent<HeroUnit>();
            heroUnit.SetupHero(Utilities.GetRandomGender(), race, profession, listIndex, Database.instance.CombatHeroAnimator);
            clone.name = "Hero: " + heroUnit.GetFullName();
            
            UnitSpawned(heroUnit);
        }
        
        protected override void LoadHero(MapPosition mapPosition, HeroSaveData saveData)
        {
            //Debug.Log("Spawning Hero at " + mapPosition.ToString());
            GameObject clone = Instantiate(_heroPrefab, _heroesParent);
            clone.transform.position = MapManager.Instance.GetWorldPosition(mapPosition);
            
            HeroUnit heroUnit = clone.GetComponent<HeroUnit>();
            heroUnit.LoadHero(saveData, Database.instance.CombatHeroAnimator);
            clone.name = "Hero: " + heroUnit.GetFullName();
            
            _heroUnits.Add(heroUnit);
        }
        
        public override void LoadState()
        {
            //Debug.Log("LoadState_Combat");
            if (!File.Exists(Database.instance.PartyDataFilePath)) return; // No state to load
	
            byte[] bytes = File.ReadAllBytes(Database.instance.PartyDataFilePath);
            PartySaveData saveData = SerializationUtility.DeserializeValue<PartySaveData>(bytes, DataFormat.JSON);

            _heroesParent.ClearTransform();
            _heroUnits.Clear();

            MapPosition spawnerPosition = MapManager.Instance.GetGridPosition(_playerSpawner.transform.position);
            LoadHero(new MapPosition(spawnerPosition.X, spawnerPosition.Y), saveData.Heroes[0]);
            LoadHero(new MapPosition(spawnerPosition.X + 1, spawnerPosition.Y), saveData.Heroes[1]);
            LoadHero(new MapPosition(spawnerPosition.X, spawnerPosition.Y - 1), saveData.Heroes[2]);
            LoadHero(new MapPosition(spawnerPosition.X + 1, spawnerPosition.Y - 1), saveData.Heroes[3]);
            
            PortraitRoom.Instance.Setup();
            PortraitRoom.Instance.SyncParty();
        }

        public override void SetLoadData(bool loadData)
        {
            _loadData = loadData;
        }
    }
}