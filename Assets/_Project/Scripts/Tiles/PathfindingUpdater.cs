using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Interactables;
using UnityEngine;

namespace Descending.Tiles
{
    public class PathfindingUpdater : MonoBehaviour
    {
        private void Start()
        {
            DestructibleCrate.OnAnyInteractableDestroyed += DestructibleCrate_OnAnyInteractableDestroyed;
        }

        private void DestructibleCrate_OnAnyInteractableDestroyed(object sender, EventArgs e)
        {
            DestructibleCrate crate = sender as DestructibleCrate;
            PathfindingManager.Instance.SetIsGridPositionWalkable(crate.MapPosition, true);
        }
    }
}
