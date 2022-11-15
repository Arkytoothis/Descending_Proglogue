using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using UnityEngine;

namespace Descending.Units
{
    public abstract class EnemyBehavior : MonoBehaviour
    {
        public abstract BaseAction ProcessAction(EnemyUnit enemyUnit, ref MapPosition targetPosition);
    }
}