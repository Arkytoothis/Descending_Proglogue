using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Combat_Events
{
    [System.Serializable]
    public class EndCombatEvent : CombatEvent
    {
        [SerializeField] private BoolEvent onEndCombat = null;
        
        public EndCombatEvent()
        {
            
        }
        
        public override void TriggerEvent(MapPosition mapPosition)
        {
            onEndCombat.Invoke(true);
        }
    }
}