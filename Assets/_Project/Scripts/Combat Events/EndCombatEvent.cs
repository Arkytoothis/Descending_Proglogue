using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Scene_Overworld;
using Descending.Tiles;
using Descending.Units;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Descending.Combat_Events
{
    [System.Serializable]
    public class EndCombatEvent : CombatEvent
    {

        public EndCombatEvent()
        {
            
        }
        
        public override void TriggerEvent(MapPosition mapPosition)
        {
            SceneManager.LoadScene((int)GameScenes.Overworld);
        }
    }
}