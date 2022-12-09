using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Features;
using Descending.Scene_Overworld;
using Pathfinding;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Party
{
    public class PartyMover : MonoBehaviour
    {
        //[SerializeField] private float _targetDistance = 3f;
        [SerializeField] private Seeker _seeker;

        [SerializeField] private WorldTile _currentTile = null;
        [SerializeField] private WorldTile _lastTile = null;
        
        [SerializeField] private BoolEvent onSetVillageButtonInteractable = null;
        [SerializeField] private BoolEvent onSetDungeonButtonInteractable = null;

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
                    if (_currentTile.Feature.GetType() == typeof(Village))
                    {
                        onSetVillageButtonInteractable.Invoke(true);
                    }
                    else if (_currentTile.Feature.GetType() == typeof(Dungeon))
                    {
                        onSetDungeonButtonInteractable.Invoke(true);
                    }
                }

                if (_lastTile == null)
                {
                    _lastTile = _currentTile;
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("World Tile"))
            {
                _lastTile = other.GetComponentInParent<WorldTile>();
                
                if (_lastTile.Feature != null)
                {
                    if (_lastTile.Feature.GetType() == typeof(Village))
                    {
                        onSetVillageButtonInteractable.Invoke(false);
                    }
                    else if (_lastTile.Feature.GetType() == typeof(Dungeon))
                    {
                        onSetDungeonButtonInteractable.Invoke(false);
                    }
                }
            }
        }
    }
}