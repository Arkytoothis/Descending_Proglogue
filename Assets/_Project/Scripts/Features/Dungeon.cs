using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Features
{
    public class Dungeon : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Dungeon Triggered");
            }
        }
    }
}