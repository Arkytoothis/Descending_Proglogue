using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Features
{
    public class FeatureManager : MonoBehaviour
    {
        public static FeatureManager Instance { get; private set; }

        private List<WorldFeature> _features = null;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple UnitManagers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            _features = new List<WorldFeature>();
        }

        public void OnRegisterFeature(WorldFeature feature)
        {
            if (feature == null) return;
            
            _features.Add(feature);
        }
    }
}