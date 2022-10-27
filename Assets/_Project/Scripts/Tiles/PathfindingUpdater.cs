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
            Crate.OnAnyInteractableDestroyed += DestructibleCrate_OnAnyInteractableDestroyed;
        }

        private void DestructibleCrate_OnAnyInteractableDestroyed(object sender, EventArgs e)
        {
            Crate crate = sender as Crate;
            PathfindingManager.Instance.SetIsGridPositionWalkable(crate.MapPosition, true);
        }
    }
}
