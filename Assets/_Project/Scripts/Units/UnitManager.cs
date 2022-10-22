using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Tiles;
using Pathfinding;
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

        private PlayerSpawner _playerSpawner = null;
        
        public List<Unit> Units => _units;
        public List<Unit> PlayerUnits => _playerUnits;
        public List<Unit> EnemyUnits => _enemyUnits;

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
        }
        
        private void SpawnHero(MapPosition mapPosition, int listIndex)
        {
            //Debug.Log("Spawning Hero at " + mapPosition.ToString());
            GameObject clone = Instantiate(_heroPrefab, _heroesParent);
            clone.transform.position = MapManager.Instance.GetWorldPosition(mapPosition);
            Unit unit = clone.GetComponent<Unit>();
            unit.SetupHero(Genders.Male, Database.instance.Races.GetRandomRace(), Database.instance.Profession.GetRandomProfession(), listIndex);
            clone.name = "Hero: " + unit.UnitData.GetName();
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
    }
}