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
        [SerializeField] private WorldCursor _worldCursor = null;
        [SerializeField] private LayerMask _groundMask;

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMousePosition());
            
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000f, _groundMask))
            {
                WorldTile tile = hit.collider.gameObject.GetComponentInParent<WorldTile>();

                if (tile == null) return;
                
                _worldCursor.SetTile(tile);
                
                if (Input.GetMouseButtonDown(1))
                {
                    if (tile.IsMovable)
                    {
                        _partyMover.MoveTo(tile.transform.position);
                        return;
                    }
                }
            }
            else
            {
                _worldCursor.SetTile(null);
            }
        }
    }
}
