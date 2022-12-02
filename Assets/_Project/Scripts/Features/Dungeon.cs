using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using UnityEngine.SceneManagement;

namespace Descending.Features
{
    public class Dungeon : WorldFeature
    {
        public override void Interact()
        {
            SaveManager.Instance.SaveState();
            SceneManager.LoadScene((int)GameScenes.Combat_Indoor);
        }
    }
}