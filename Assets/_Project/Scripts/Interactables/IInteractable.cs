using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Interactables
{
    public interface IInteractable
    {
        public void Interact(Action onComplete);
        public string GetName();
    }
}