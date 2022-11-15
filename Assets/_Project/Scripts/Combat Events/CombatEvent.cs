using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using UnityEngine;

namespace Descending.Combat_Events
{
    [System.Serializable]
    public abstract class CombatEvent
    {
        public abstract void TriggerEvent(MapPosition mapPosition);
    }
}