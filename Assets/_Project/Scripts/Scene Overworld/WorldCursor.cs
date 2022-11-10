using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Scene_Overworld
{
    public class WorldCursor : MonoBehaviour
    {
        [SerializeField] private GameObjectEvent onDisplayWorldTile = null;
        
        private WorldTile _currentTile = null;

        private void Start()
        {
            
        }

        public void SetTile(WorldTile tile)
        {
            _currentTile = tile;
            
            if (tile != null)
            {
                onDisplayWorldTile.Invoke(_currentTile.gameObject);
                transform.position = tile.transform.position;
            }
            else
            {
                transform.position = new Vector3(0f, -1000f, 0f);
            }
        }
    }
}