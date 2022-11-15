using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Tiles;
using Descending.Units;
using UnityEngine;

namespace Descending.Combat_Events
{
    [System.Serializable]
    public class SpawnEnemyEvent : CombatEvent
    {
        [SerializeField] private EnemyDefinition _enemyDefinition = null;

        public SpawnEnemyEvent()
        {
            
        }
        
        public override void TriggerEvent(MapPosition mapPosition)
        {
            UnitManager.Instance.SpawnEnemy(mapPosition, _enemyDefinition);
        }
    }
}