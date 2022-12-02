using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Party;
using Descending.Scene_Overworld;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Descending.Overworld
{
    public class Raycaster : MonoBehaviour
    {
        [SerializeField] private PartyMover _partyMover = null;
        [SerializeField] private WorldCursor _worldCursor = null;
        [SerializeField] private LayerMask _tileMask;

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMousePosition());
            
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000f, _tileMask))
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
