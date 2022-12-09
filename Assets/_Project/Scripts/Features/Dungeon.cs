using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Features
{
    public class Dungeon : WorldFeature
    {
        [SerializeField] private BoolEvent onSetDungeonButtonInteractable = null;

        public override void RegisterFeature(WorldFeature feature)
        {
            throw new NotImplementedException();
        }

        public override void Interact()
        {
            SaveManager.Instance.SaveState();
            onSetDungeonButtonInteractable.Invoke(true);
        }

        public override void Leave()
        {
            Debug.Log("Leaving Dungeon");
        }
    }
}