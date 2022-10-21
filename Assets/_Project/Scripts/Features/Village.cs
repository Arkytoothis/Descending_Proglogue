using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Features
{
    public class Village : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Village Triggered");
            }
        }
    }
}