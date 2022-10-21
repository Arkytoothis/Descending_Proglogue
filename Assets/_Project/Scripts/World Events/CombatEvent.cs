using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.World_Events
{
    public class CombatEvent : WorldEvent
    {
        public override void Setup(int startHour, int spawnerIndex)
        {
            _startHour = startHour;
            _currentHour = _startHour;
            _spawnerIndex = spawnerIndex;
        }

        public override void UpdateTime()
        {
            _currentHour++;
        }
    }
}