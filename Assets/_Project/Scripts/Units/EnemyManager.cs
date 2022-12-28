using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using UnityEngine;

namespace Descending.Units
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance { get; private set; }
        
        [SerializeField] private List<EnemyUnit> _enemyUnits = null;
        [SerializeField] private Transform _enemiesParent = null;
        [SerializeField] private List<EnemySpawner> _enemySpawners = null;
        
        public List<EnemyUnit> EnemyUnits => _enemyUnits;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple EnemyManagers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
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
        }
        
        public void UnitDead(Unit unit)
        {
            if (unit.IsEnemy)
            {
                EnemyUnit enemyUnit = unit as EnemyUnit;
                enemyUnit.DropTreasure();
                _enemyUnits.Remove(enemyUnit);
            }
        }

        public void RegisterEnemySpawner(GameObject spawnerObject)
        {
            EnemySpawner spawner = spawnerObject.GetComponent<EnemySpawner>();
            _enemySpawners.Add(spawner);
            spawner.Spawn(_enemiesParent);
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
    }
}