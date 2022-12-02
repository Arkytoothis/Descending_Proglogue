using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Units;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Descending.Features
{
    public class Village : WorldFeature
    {
        public override void Interact()
        {
            Debug.Log("Interacting with Village");
            UnitManager.Instance.SavePartyPosition();
        }
    }
}