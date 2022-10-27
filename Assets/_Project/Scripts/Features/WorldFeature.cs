using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Features
{
    public abstract class WorldFeature : MonoBehaviour
    {
        [SerializeField] protected Transform _interactionTransform = null;

        public Transform InteractionTransform => _interactionTransform;

        public abstract void Interact();
    }
}