using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace Descending.Combat_Events
{
    [System.Serializable]
    public class CombatEventParameters
    {
        [SerializeReference] public CombatEvent CombatEvent;
        public MapPosition OffsetPosition;

        public CombatEventParameters(MapPosition offsetPosition, CombatEvent combatEvent)
        {
            OffsetPosition = offsetPosition;
            CombatEvent = combatEvent;
        }
    }
}