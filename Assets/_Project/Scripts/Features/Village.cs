using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Units;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Descending.Features
{
    public class Village : WorldFeature
    {
        [SerializeField] private BoolEvent onSetVillageButtonInteractable = null;
        
        public override void Interact()
        {
            Debug.Log("Interacting with Village");
            UnitManager.Instance.SavePartyPosition();
            onSetVillageButtonInteractable.Invoke(true);
        }
    }
}