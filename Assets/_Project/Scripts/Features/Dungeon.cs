using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Descending.Features
{
    public class Dungeon : WorldFeature
    {
        public override void Interact()
        {
            Debug.Log("Interacting with Dungeon");
            SceneManager.LoadScene((int)GameScenes.Combat_Indoor);
        }
    }
}