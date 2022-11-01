using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Attributes;
using Descending.Core;
using Descending.Tiles;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Units
{
    public class UnitManager : MonoBehaviour
    {
        public static UnitManager Instance { get; private set; }

        [SerializeField] private GameObject _heroPrefab = null;
        [SerializeField] private Transform _heroesParent = null;
        [SerializeField] private Transform _enemiesParent = null;
        
        [SerializeField] private List<Unit> _units = null;
        [SerializeField] private List<HeroUnit> _heroUnits = null;
        [SerializeField] private List<EnemyUnit> _enemyUnits = null;
        [SerializeField] private List<EnemySpawner> _enemySpawners = null;
        
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

        public void UnitSpawned(Unit unit)
        {
            if (unit.IsEnemy)
            {
                _enemyUnits.Add(unit as EnemyUnit);
            }
            else
            {
                _heroUnits.Add(unit as HeroUnit);
            }
            
            _units.Add(unit);
            
            onSyncParty.Invoke(true);
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
            //Debug.Log("RegisterPlayerSpawner");
            _playerSpawner = spawner;
        }

        public void SpawnHeroes()
        {
            MapPosition spawnerPosition = MapManager.Instance.GetGridPosition(_playerSpawner.transform.position);
            
            SpawnHero(new MapPosition(spawnerPosition.X, spawnerPosition.Y), 0, Database.instance.Races.GetRandomRace(), Database.instance.Profession.GetProfession("Soldier"));
            SpawnHero(new MapPosition(spawnerPosition.X + 1, spawnerPosition.Y), 1, Database.instance.Races.GetRandomRace(), Database.instance.Profession.GetProfession("Scout"));
            SpawnHero(new MapPosition(spawnerPosition.X, spawnerPosition.Y - 1), 2, Database.instance.Races.GetRandomRace(), Database.instance.Profession.GetProfession("Acolyte"));
            SpawnHero(new MapPosition(spawnerPosition.X + 1, spawnerPosition.Y - 1), 3, Database.instance.Races.GetRandomRace(), Database.instance.Profession.GetProfession("Apprentice"));
            
            PortraitRoom.Instance.Setup();
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
            
            ActionManager.Instance.SetSelectedAction(UnitManager.Instance.SelectedHero.GetAction<MoveAction>());
            onSelectHero.Invoke(_selectedHero.gameObject);
        }
    }
}