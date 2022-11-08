using System.Collections;
using System.Collections.Generic;
using Descending.Scene_Overworld;
using UnityEngine;

namespace Descending.Features
{
    public class FeaturePlacer : MonoBehaviour
    {
        [SerializeField] private Transform _featuresParent = null;
        
        public void PlaceFeature(FeatureDefinition definition, WorldTile tile)
        {
            //Debug.Log("Placing " + definition.Name);
            GameObject clone = Instantiate(definition.Prefab, _featuresParent);
            clone.transform.position = tile.transform.position;
            
            tile.SetFeature(clone.GetComponent<WorldFeature>());
        }
    }
}