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
        [SerializeField] private LayerMask _groundMask;

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMousePosition());
            
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _groundMask))
            {
                if (Input.GetMouseButtonDown(1))
                {
                    _worldCursor.MoveTo(hit.point);
                    _partyMover.MoveTo(hit.point);
                }
            }
        }
    }
}
