using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace Descending.Units
{
    public class UnitPathfinder : MonoBehaviour
    {
        [SerializeField] private Seeker _seeker = null;
        //[SerializeField] private AILerp _aiLerp = null;

        private void Start()
        {
            
        }

        public Path FindPath(Vector3 start, Vector3 end)
        {
            //_aiLerp.canMove = true;
            return _seeker.StartPath(start, end);
        }
    }
}