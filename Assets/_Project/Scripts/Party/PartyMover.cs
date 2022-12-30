using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Party
{
    public class PartyMover : MonoBehaviour
    {
        [SerializeField] private Seeker _seeker;

        public void MoveTo(Vector3 position)
        {
            _seeker.StartPath(transform.position, position);
        }
    }
}