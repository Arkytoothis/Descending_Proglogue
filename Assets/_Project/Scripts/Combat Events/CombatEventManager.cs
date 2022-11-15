using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Combat_Events
{
    public class CombatEventManager : MonoBehaviour
    {
        public static CombatEventManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple Combat Event Managers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        public void Setup()
        {
            
        }

        public void TriggerEvent(CombatEventParameters parameters)
        {
            Debug.Log(parameters.CombatEvent.GetType() + " triggered");
        }
    }
}