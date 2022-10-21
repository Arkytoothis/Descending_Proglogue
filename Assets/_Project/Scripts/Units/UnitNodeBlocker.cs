using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace Descending.Units
{
    public class UnitNodeBlocker : MonoBehaviour
    {
        public void Start () {
            var blocker = GetComponent<SingleNodeBlocker>();

            blocker.BlockAtCurrentPosition();
        }
    }
}