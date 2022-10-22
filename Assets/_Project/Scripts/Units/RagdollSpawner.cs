using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Units
{
    public class RagdollSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _ragdollPrefab = null;
        [SerializeField] private HealthSystem _healthSystem = null;
        [SerializeField] private Transform _unitRootBone = null;

        private void Awake()
        {
            _healthSystem = GetComponent<HealthSystem>();
        }

        public void Activate(HealthSystem healthSystem)
        {
            GameObject clone = Instantiate(_ragdollPrefab, transform.position, transform.rotation);
            UnitRagdoll ragdoll = clone.GetComponent<UnitRagdoll>();
            ragdoll.Setup(healthSystem.Attacker, _unitRootBone);
        }
    }
}