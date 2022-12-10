using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Features
{
    public class FeatureManager : MonoBehaviour
    {
        public static FeatureManager Instance { get; private set; }

        [SerializeField] private List<WorldFeature> _features = new List<WorldFeature>();
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple UnitManagers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }
        
        public void Setup()
        {
        }

        public void RegisterFeature(WorldFeature feature)
        {
            if (feature == null) return;
            
            Debug.Log("Registering Feature: " + feature);
            _features.Add(feature);
            feature.Setup();
        }
    }
}