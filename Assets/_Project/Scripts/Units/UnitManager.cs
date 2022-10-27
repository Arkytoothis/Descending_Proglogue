using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private List<Unit> _playerUnits = null;
        [SerializeField] private List<Unit> _enemyUnits = null;
        [SerializeField] private List<EnemySpawner> _enemySpawners = null;
        
        [SerializeField] private BoolEvent onSyncParty = null;
        [SerializeField] private GameObjectEvent onSelectHero = null;

        private PlayerSpawner _playerSpawner = null;
        private HeroUnit _selectedHero = null;
        
        public List<Unit> Units => _units;
        public List<Unit> PlayerUnits => _playerUnits;
        public List<Unit> EnemyUnits => _enemyUnits;
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
            _playerUnits = new List<Unit>();
            _enemyUnits = new List<Unit>();
        }

        public void Setup()
        {
            
        }

        public void UnitSpawned(Unit unit)
        {
            if (unit.IsEnemy)
            {
                _enemyUnits.Add(unit);
            }
            else
            {
                _playerUnits.Add(unit);
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
                _enemyUnits.Remove(unit);
            }
            else
            {
                _playerUnits.Remove(unit);
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
            
            SpawnHero(new MapPosition(spawnerPosition.X - 3, spawnerPosition.Y), 0);
            SpawnHero(new MapPosition(spawnerPosition.X, spawnerPosition.Y), 1);
            SpawnHero(new MapPosition(spawnerPosition.X + 1, spawnerPosition.Y), 2);
            
            PortraitRoom.Instance.Setup();
        }
        
        private void SpawnHero(MapPosition mapPosition, int listIndex)
        {
            //Debug.Log("Spawning Hero at " + mapPosition.ToString());
            GameObject clone = Instantiate(_heroPrefab, _heroesParent);
            clone.transform.position = MapManager.Instance.GetWorldPosition(mapPosition);
            
            HeroUnit heroUnit = clone.GetComponent<HeroUnit>();
            heroUnit.SetupHero(Genders.Male, Database.instance.Races.GetRandomRace(), Database.instance.Profession.GetRandomProfession(), listIndex);
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
            return _playerUnits[index] as HeroUnit;
        }

        public void SelectHero(HeroUnit hero)
        {
            _selectedHero = hero;
            
            for (int i = 0; i < _playerUnits.Count; i++)
            {
                if (hero.HeroData.ListIndex == i)
                {
                    _playerUnits[i].Select();
                }
                else
                {
                    _playerUnits[i].Deselect();
                }
            }
            
            ActionManager.Instance.SetSelectedAction(UnitManager.Instance.SelectedHero.GetAction<MoveAction>());
            onSelectHero.Invoke(_selectedHero.gameObject);
        }
    }
}