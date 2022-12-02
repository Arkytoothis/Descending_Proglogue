using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Features;
using Descending.Scene_Overworld;
using Pathfinding;
using UnityEngine;

namespace Descending.Party
{
    public class PartyMover : MonoBehaviour
    {
        //[SerializeField] private float _targetDistance = 3f;
        [SerializeField] private Seeker _seeker;

        [SerializeField] private WorldTile _currentTile = null;
        [SerializeField] private WorldTile _lastTile = null;

        public WorldTile CurrentTile => _currentTile;
        public WorldTile LastTile => _lastTile;

        public void MoveTo(Vector3 position)
        {
            _seeker.StartPath(transform.position, position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("World Tile"))
            {
                _currentTile = other.GetComponentInParent<WorldTile>();
                
                if (_currentTile.Feature != null)
                {
                    //_currentTile.Feature.Interact();
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("World Tile"))
            {
                _lastTile = other.GetComponentInParent<WorldTile>();
            }
        }
    }
}