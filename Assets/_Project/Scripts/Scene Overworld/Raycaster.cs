using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Party;
using Descending.Scene_Overworld;
using UnityEngine;

namespace Descending.Overworld
{
    public class Raycaster : MonoBehaviour
    {
        [SerializeField] private PartyMover _partyMover = null;
        [SerializeField] private LayerMask _groundMask;

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMousePosition());
            
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000f, _groundMask))
            {
                if (Input.GetMouseButtonDown(1))
                {
                    WorldTile tile = hit.collider.gameObject.GetComponentInParent<WorldTile>();

                    if (tile != null && tile.IsMovable)
                    {
                        _partyMover.MoveTo(tile.transform.position);
                        return;
                    }
                }
            }
        }
    }
}
