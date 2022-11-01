using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Interactables
{
    public class InteractableManager : MonoBehaviour
    {
        public static InteractableManager Instance { get; private set; }

        private List<IInteractable> _interactables = null;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple Interactable Managers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;

            _interactables = new List<IInteractable>();
        }

        public void Setup()
        {
            foreach (IInteractable interactable in _interactables)
            {
                interactable.Setup();
            }    
        }
        
        public void RegisterInteractable(IInteractable interactable)
        {
            _interactables.Add(interactable);
        }
    }
}