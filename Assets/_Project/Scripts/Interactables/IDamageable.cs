using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Interactables
{
    public interface IDamageable
    {
        public void Damage(int damage);
        public string GetName();
    }
}