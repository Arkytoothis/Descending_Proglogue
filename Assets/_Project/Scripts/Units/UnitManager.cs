using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Descending.Attributes;
using Descending.Core;
using Descending.Party;
using Descending.Tiles;
using Descending.Units;
using ScriptableObjectArchitecture;
using Sirenix.Serialization;
using UnityEngine;

namespace Descending.Units
{
    public class UnitManager : MonoBehaviour
    {
        public static UnitManager Instance { get; private set; }

        [SerializeField] private GameObject _heroPrefab = null;
        [SerializeField] private Transform _heroesParent = null;
        [SerializeField] private Transform _enemiesParent = null;
        [SerializeField] private float _unitScaleFactor = 5f;
        [SerializeField] private PartyController _partyController = null;
        
        [SerializeField] private List<Unit> _units = null;
        [SerializeField] private List<HeroUnit> _heroUnits = null;
        [SerializeField] private List<EnemyUnit> _enemyUnits = null;
        [SerializeField] private List<EnemySpawner> _enemySpawners = null;
        
        [SerializeField] private bool _loadData = false;
        
        [SerializeField] private BoolEvent onSyncParty = null;
        [SerializeField] private GameObjectEvent onSelectHero = null;
        
        private PlayerSpawner _playerSpawner = null;
        private HeroUnit _selectedHero = null;
        
        public List<Unit> Units => _units;
        public List<HeroUnit> HeroUnits => _heroUnits;
        public List<EnemyUnit> EnemyUnits => _enemyUnits;
        public HeroUnit SelectedHero => _selectedHero;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple UnitManagers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            _units = new List<Unit>();
            _heroUnits = new List<HeroUnit>();
            _enemyUnits = new List<EnemyUnit>();
        }

        public void Setup()
        {
            
        }

        public void SyncHeroes()
        {
            //Debug.Log("Syncing Heroes");
            onSyncParty.Invoke(true);
            PortraitRoom.Instance.RefreshCameras();
            
            foreach (HeroUnit heroUnit in _heroUnits)
            {
                heroUnit.SyncWorldPanel();
            }
        }
        
        public void UnitSpawned(Unit unit)
        {
            if (unit.IsEnemy)
            {
                _enemyUnits.Add(unit as EnemyUnit);
            }
            else
            {
                _heroUnits.Add(unit as HeroUnit);
                onSyncParty.Invoke(true);
            }
            
            _units.Add(unit);
        }

        public void UnitDead(Unit unit)
        {
            if (unit.IsEnemy)
            {
                EnemyUnit enemyUnit = unit as EnemyUnit;
                enemyUnit.DropTreasure();
                _enemyUnits.Remove(enemyUnit);
            }
            else
            {
                _heroUnits.Remove(unit as HeroUnit);
            }
            
            _units.Remove(unit);
            
            onSyncParty.Invoke(true);
        }

        public void RegisterPlayerSpawner(PlayerSpawner spawner)
        {
            _playerSpawner = spawner;
        }

        public void SpawnHeroes_Overworld()
        {
            if (_loadData == true)
            {
                LoadState_Overworld();
            }
            else
            {
                GenerateHeroes_Overworld();
            }
        }

        public void SpawnHeroes_Combat()
        {
            if (_loadData == true)
            {
                LoadState_Combat();
            }
            else
            {
                GenerateHeroes_Combat();
            }
        }

        public void RegisterEnemySpawner(GameObject spawnerObject)
        {
            EnemySpawner spawner = spawnerObject.GetComponent<EnemySpawner>();
            _enemySpawners.Add(spawner);
            spawner.Spawn(_enemiesParent);
        }

        public void SpawnEnemies()
        {
            // foreach (EnemySpawner enemySpawner in _enemySpawners)
            // {
            //     enemySpawner.Spawn(_enemiesParent);
            // }
        }

        public HeroUnit GetHero(int index)
        {
            return _heroUnits[index] as HeroUnit;
        }

        public void SelectHeroDefault()
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

        public void RefreshSelectedHero()
        {
            SelectHero(_selectedHero);
        }
        
        public void SelectHero(HeroUnit hero)
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
        
        public void SelectHeroOverworld(HeroUnit hero)
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
        
        public void SelectDefaultHeroOverworld()
        {
            _selectedHero = _heroUnits[0];
            SelectHeroOverworld(_selectedHero);
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

        public void AwardExperience(int experience)
        {
            foreach (HeroUnit heroUnit in _heroUnits)
            {
                heroUnit.AddExperience(experience);
            }
        }

        public void SpawnEnemy(MapPosition mapPosition, EnemyDefinition enemyDefinition)
        {
            Debug.Log("Spawning: " + enemyDefinition.Name + " at Map Position " + mapPosition.ToString());
            GameObject clone = Instantiate(enemyDefinition.Prefab, _enemiesParent);
            clone.name = enemyDefinition.Name;
            clone.transform.position = MapManager.Instance.GetWorldPosition(mapPosition);

            EnemyUnit unit = clone.GetComponent<EnemyUnit>();
            unit.SetupEnemy(enemyDefinition);
        }

        public void RecalculateHeroAttributes()
        {
            foreach (HeroUnit heroUnit in _heroUnits)
            {
                heroUnit.RecalculateAttributes();
            }
            
            SyncHeroes();
        }
        
        
        public void SaveState_Combat()
        {
            PartySaveData saveData = new PartySaveData(_heroUnits);
            byte[] bytes = SerializationUtility.SerializeValue(saveData, DataFormat.JSON);
            File.WriteAllBytes(Database.instance.PartyDataFilePath, bytes);
        }
        
        public void SaveState_Overworld()
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
        
            
        public void GenerateHeroes_Overworld()
        {
            _heroUnits = new List<HeroUnit>();
            SpawnHeroOverworld(0, Database.instance.Races.GetRace("Half Orc"), Database.instance.Profession.GetProfession("Soldier"));
            SpawnHeroOverworld(1, Database.instance.Races.GetRace("Wild Elf"), Database.instance.Profession.GetProfession("Scout"));
            SpawnHeroOverworld(2, Database.instance.Races.GetRace("Imperial"), Database.instance.Profession.GetProfession("Acolyte"));
            SpawnHeroOverworld(3, Database.instance.Races.GetRace("Valarian"), Database.instance.Profession.GetProfession("Apprentice"));
            
            PortraitRoom.Instance.Setup();
            PortraitRoom.Instance.SyncParty();
        }
        
        public void GenerateHeroes_Combat()
        {
            MapPosition spawnerPosition = MapManager.Instance.GetGridPosition(_playerSpawner.transform.position);
            
            SpawnHero(new MapPosition(spawnerPosition.X, spawnerPosition.Y), 0, Database.instance.Races.GetRace("Godkin"), Database.instance.Profession.GetProfession("Soldier"));
            SpawnHero(new MapPosition(spawnerPosition.X + 1, spawnerPosition.Y), 1, Database.instance.Races.GetRace("Halfling"), Database.instance.Profession.GetProfession("Scout"));
            SpawnHero(new MapPosition(spawnerPosition.X, spawnerPosition.Y - 1), 2, Database.instance.Races.GetRace("Sun Elf"), Database.instance.Profession.GetProfession("Acolyte"));
            SpawnHero(new MapPosition(spawnerPosition.X + 1, spawnerPosition.Y - 1), 3, Database.instance.Races.GetRace("Valarian"), Database.instance.Profession.GetProfession("Apprentice"));
            
            PortraitRoom.Instance.Setup();
            PortraitRoom.Instance.SyncParty();
        }
        
        private void SpawnHeroOverworld(int listIndex, RaceDefinition race, ProfessionDefinition profession)
        {
            //Debug.Log("Spawning Hero at " + mapPosition.ToString());
            GameObject clone = Instantiate(_heroPrefab, _heroesParent);
            
            HeroUnit heroUnit = clone.GetComponent<HeroUnit>();
            heroUnit.SetupHero(Genders.Male, race, profession, listIndex);
            heroUnit.WorldModel.transform.localScale = new Vector3(_unitScaleFactor, _unitScaleFactor, _unitScaleFactor);
            clone.name = "Hero: " + heroUnit.GetFullName();
            
            _heroUnits.Add(heroUnit);
        }
        
        private void LoadHeroOverworld(HeroSaveData saveData)
        {
            //Debug.Log("Spawning Hero at " + mapPosition.ToString());
            GameObject clone = Instantiate(_heroPrefab, _heroesParent);
            
            HeroUnit heroUnit = clone.GetComponent<HeroUnit>();
            heroUnit.LoadHero(saveData);
            heroUnit.WorldModel.transform.localScale = new Vector3(_unitScaleFactor, _unitScaleFactor, _unitScaleFactor);
            clone.name = "Hero: " + heroUnit.GetFullName();
            
            _heroUnits.Add(heroUnit);
        }
        
        public void LoadState_Overworld()
        {
            if (!File.Exists(Database.instance.PartyDataFilePath)) return; // No state to load
	
            byte[] bytes = File.ReadAllBytes(Database.instance.PartyDataFilePath);
            PartySaveData saveData = SerializationUtility.DeserializeValue<PartySaveData>(bytes, DataFormat.JSON);

            //_partyController.transform.position = saveData.WorldPosition;
            _heroesParent.ClearTransform();
            _heroUnits.Clear();

            for (int i = 0; i < saveData.Heroes.Length; i++)
            {
                LoadHeroOverworld(saveData.Heroes[i]);
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
        
        private void SpawnHero(MapPosition mapPosition, int listIndex, RaceDefinition race, ProfessionDefinition profession)
        {
            //Debug.Log("Spawning Hero at " + mapPosition.ToString());
            GameObject clone = Instantiate(_heroPrefab, _heroesParent);
            clone.transform.position = MapManager.Instance.GetWorldPosition(mapPosition);
            
            HeroUnit heroUnit = clone.GetComponent<HeroUnit>();
            heroUnit.SetupHero(Genders.Male, race, profession, listIndex);
            clone.name = "Hero: " + heroUnit.GetFullName();
            
            UnitSpawned(heroUnit);
        }
        
        private void LoadHero(MapPosition mapPosition, HeroSaveData saveData)
        {
            //Debug.Log("Spawning Hero at " + mapPosition.ToString());
            GameObject clone = Instantiate(_heroPrefab, _heroesParent);
            clone.transform.position = MapManager.Instance.GetWorldPosition(mapPosition);
            
            HeroUnit heroUnit = clone.GetComponent<HeroUnit>();
            heroUnit.LoadHero(saveData);
            clone.name = "Hero: " + heroUnit.GetFullName();
            
            _heroUnits.Add(heroUnit);
        }
        
        public void LoadState_Combat()
        {
            //Debug.Log("LoadState_Combat");
            if (!File.Exists(Database.instance.PartyDataFilePath)) return; // No state to load
	
            byte[] bytes = File.ReadAllBytes(Database.instance.PartyDataFilePath);
            PartySaveData saveData = SerializationUtility.DeserializeValue<PartySaveData>(bytes, DataFormat.JSON);

            //_partyController.transform.position = saveData.WorldPosition;
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

        public void SetLoadData(bool loadData)
        {
            _loadData = loadData;
        }
    }
}