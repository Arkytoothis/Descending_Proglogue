using Descending.Core;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Abilities
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Ability Definition", menuName = "Descending/Definition/Ability Definition")]
    public class AbilityDefinition : ScriptableObject
    {
        [SerializeField, HideLabel] private AbilityDetails _details = null;
        [SerializeReference] private AbilityEffects _effects = null;

        public AbilityDetails Details => _details;
        public AbilityEffects Effects => _effects;
    }
}