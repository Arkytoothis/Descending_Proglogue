using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Units
{
    public class AIManager : MonoBehaviour
    {
        public static AIManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple AIManagers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }
    }
}
