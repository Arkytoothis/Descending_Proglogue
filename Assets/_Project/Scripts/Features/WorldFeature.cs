using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Features
{
    public abstract class WorldFeature : MonoBehaviour
    {
        [SerializeField] protected FeatureDefinition _definition = null;
        [SerializeField] protected Transform _interactionTransform = null;

        public FeatureDefinition Definition => _definition;
        public Transform InteractionTransform => _interactionTransform;

        public abstract void RegisterFeature(WorldFeature feature);
        public abstract void Interact();
        public abstract void Leave();
    }
}