using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Features;
using Descending.Party;
using UnityEngine;

namespace Descending.Overworld
{
    public class Raycaster : MonoBehaviour
    {
        public static Raycaster Instance { get; private set; }

        [SerializeField] private PartyMover _partyMover = null;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private LayerMask _featureMask;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple Raycasters " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMousePosition());
            
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _featureMask))
            {
                WorldFeature feature = hit.collider.GetComponent<WorldFeature>();
                
                if (InputManager.Instance.GetLeftMouseDown())
                {
                    //Debug.Log("Feature Left Clicked");
                    return;
                }
                
                if (InputManager.Instance.GetRightMouseDown())
                {
                    //Debug.Log("Feature Right Clicked");
                    _partyMover.SetDestination(feature, feature.InteractionTransform.position);
                    return;
                }
            }
            
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, _groundMask))
            {
                if (Input.GetMouseButtonDown(1))
                {
                    _partyMover.SetDestination(null, hit.point);
                    return;
                }
            }
        }
    }
}
