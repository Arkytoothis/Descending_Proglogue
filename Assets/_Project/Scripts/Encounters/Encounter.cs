using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Encounters
{
    public class Encounter : MonoBehaviour
    {
        [SerializeField] private EncounterEvent onTriggerEncounter = null;
        
        public void Trigger()
        {
            Debug.Log("Encounter Triggered");
            UnitManager.Instance.SavePartyPosition();
            onTriggerEncounter.Invoke(this);
        }
    }
}