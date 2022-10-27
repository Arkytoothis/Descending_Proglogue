using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Features;
using Pathfinding;
using UnityEngine;

namespace Descending.Party
{
    public class PartyMover : MonoBehaviour
    {
        [SerializeField] private float _targetDistance = 3f;
        [SerializeField] private Seeker _seeker;

        [SerializeField] private WorldFeature _targetFeature = null;
        
        public void SetDestination(WorldFeature targetFeature, Vector3 destination)
        {
            _targetFeature = targetFeature;
            _seeker.StartPath(transform.position, destination);
        }

        private void Update()
        {
            if (_targetFeature == null) return;

            float distance = Vector3.Distance(transform.position, _targetFeature.InteractionTransform.position);
            
            if (distance <= _targetDistance)
            {
                _targetFeature.Interact();
                _targetFeature = null;
            }
        }
    }
}