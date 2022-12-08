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
        
        public override void Interact()
        {
            SaveManager.Instance.SaveState();
            onSetDungeonButtonInteractable.Invoke(true);
        }
    }
}