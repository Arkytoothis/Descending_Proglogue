using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.World_Events
{
    public abstract class WorldEvent : MonoBehaviour
    {
        [SerializeField] protected int _duration = 2;
        [SerializeField] protected int _startHour = 0;
        [SerializeField] protected int _currentHour = 0;
        [SerializeField] protected int _spawnerIndex = 0;

        public int Duration => _duration;
        public int StartHour => _startHour;
        public int CurrentHour => _currentHour;
        public int SpawnerIndex => _spawnerIndex;

        public abstract void Setup(int startHour, int spawnerIndex);
        public abstract void UpdateTime();

        public bool HasTime()
        {
            if (_currentHour > _startHour + _duration)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}