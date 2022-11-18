using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using UnityEngine;

namespace Descending.Combat
{
    public class ExitZone : MonoBehaviour
    {
        private int _heroesInZone = 0;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hero Unit"))
            {
                _heroesInZone++;
                Debug.Log("Heroes in Zone: " + _heroesInZone);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Hero Unit"))
            {
                _heroesInZone--;
                Debug.Log("Heroes in Zone: " + _heroesInZone);
            }
        }
    }
}