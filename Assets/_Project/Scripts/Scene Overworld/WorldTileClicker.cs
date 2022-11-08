using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Scene_Overworld
{
    public class WorldTileClicker : MonoBehaviour
    {
        private WorldTile _tile = null;

        private void Awake()
        {
            _tile = GetComponentInParent<WorldTile>();
        }

        private void OnMouseDown()
        {
            _tile.Clicked();
        }
    }
}