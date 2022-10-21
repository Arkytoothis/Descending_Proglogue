using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Descending.Equipment
{
    [CreateAssetMenu(fileName = "Projectile Definition", menuName = "Descending/Definition/Projectile Definition")]
    public class ProjectileDefinition : ScriptableObject
    {
        public GameObject Prefab = null;
        public float Speed = 30f;
        public int MinDamage = 1;
        public int MaxDamage = 10;
        public DamageTypeDefinition DamageType = null;
    }
}