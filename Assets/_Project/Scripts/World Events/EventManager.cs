using System.Collections;
using System.Collections.Generic;
using Descending.Game_Events;
using UniStorm;
using UnityEngine;

namespace Descending.World_Events
{
    public class EventManager : MonoBehaviour
    {
        [SerializeField] private int _eventChance = 10;
        [SerializeField] private Transform _eventsParent = null;
        [SerializeField] private List<WorldEventSpawner> _spawners = null;
        [SerializeField] private List<GameObject> _eventPrefabs = null;
        
        [SerializeField] private List<WorldEvent> _events = null;

        public static EventManager instance { get; private set; }

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("Multiple Event Managers " + transform + " - " + instance);
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        public void Setup()
        {
            for (int i = 0; i < _spawners.Count; i++)
            {
                _spawners[i].Hide();
            }

            UniStormSystem.Instance.OnHourChangeEvent.AddListener(OnHourChanged);
        }

        private void OnHourChanged()
        {
            CheckAllEvents();
            TrySpawnNewEvent();
        }

        private void TrySpawnNewEvent()
        {
            //Debug.Log("Rolling for event");

            if (Random.Range(0, 100) < _eventChance)
            {
                //Debug.Log("Spawning New Event");
                int spawnerIndex = GetUnoccupiedSpawnerIndex();
                int typeIndex = Random.Range(0, _eventPrefabs.Count);

                GameObject clone = Instantiate(_eventPrefabs[typeIndex], _eventsParent);
                clone.transform.position = _spawners[spawnerIndex].transform.position;
                _spawners[spawnerIndex].IsOccupied = true;
                
                WorldEvent worldEvent = clone.GetComponent<WorldEvent>();
                worldEvent.Setup(UniStormSystem.Instance.Hour, spawnerIndex);
                _events.Add(worldEvent);
            }
        }

        private void CheckAllEvents()
        {
            for (int i = 0; i < _events.Count; i++)
            {
                _events[i].UpdateTime();
            }
            
            for (int i = 0; i < _events.Count; i++)
            {
                if (_events[i].HasTime() == false)
                {
                    //Debug.Log("Destroying Event");
                    _spawners[_events[i].SpawnerIndex].IsOccupied = false;
                    Destroy(_events[i].gameObject);
                    _events.RemoveAt(i);
                }
            }
        }

        private int GetUnoccupiedSpawnerIndex()
        {
            int index = -1;

            do
            {
                index = Random.Range(0, _spawners.Count);
            } while (_spawners[index].IsOccupied);

            return index;
        }
    }
}