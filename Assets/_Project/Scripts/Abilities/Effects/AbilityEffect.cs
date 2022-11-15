using Sirenix.OdinInspector;
using System.Collections.Generic;
using Descending.Core;
using Descending.Units;
using UnityEngine;

namespace Descending.Abilities
{
    [System.Serializable]
    public abstract class AbilityEffect
    {
        [SerializeField] protected AbilityEffectType _effectType = AbilityEffectType.None;
        [SerializeField] protected AbilityEffectAffects _affects = AbilityEffectAffects.None;

        public AbilityEffectType EffectType { get => _effectType; }
        public AbilityEffectAffects Affects { get => _affects; set => _affects = value; }

        public virtual string GetTooltipText() { return ""; }
        public virtual void Process(Unit user, List<Unit> targets) { }
    }
}