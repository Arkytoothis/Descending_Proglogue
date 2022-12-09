using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Features;
using UnityEngine;

namespace Descending.Party
{
    public class FeatureDetector : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("World Feature"))
            {
                WorldFeature feature = other.GetComponent<WorldFeature>();

                if (feature != null)
                {
                    feature.Interact();
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("World Feature"))
            {
                WorldFeature feature = other.GetComponent<WorldFeature>();

                if (feature != null)
                {
                    feature.Leave();
                }
            }
        }
    }
}