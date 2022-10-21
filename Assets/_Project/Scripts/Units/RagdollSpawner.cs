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
            _healthSystem.OnDead += HealthSystem_OnDead;
        }

        private void HealthSystem_OnDead(object sender, EventArgs e)
        {
            HealthSystem healthSystem = sender as HealthSystem;
            GameObject clone = Instantiate(_ragdollPrefab, transform.position, transform.rotation);
            UnitRagdoll ragdoll = clone.GetComponent<UnitRagdoll>();
            ragdoll.Setup(healthSystem.Attacker, _unitRootBone);
        }
    }
}