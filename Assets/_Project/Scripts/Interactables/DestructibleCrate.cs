using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using UnityEngine;

namespace Descending.Interactables
{
    public class DestructibleCrate : MonoBehaviour, IDamagable
    {
        public static event EventHandler OnAnyInteractableDestroyed;

        [SerializeField] private GameObject _debrisEffectPrefab = null;
        [SerializeField] private GameObject _smokeEffectPrefab = null;
        
        private MapPosition mapPosition;
        private int _health = 10;
        private int _maxHealth = 10;

        public MapPosition MapPosition => mapPosition;
        public int Health => _health;

        private void Start()
        {
            mapPosition = MapManager.Instance.GetGridPosition(transform.position);
        }

        public void Damage(int damage)
        {
            _health -= damage;

            Debug.Log(name + " takes " + damage + " damage, " + _health + " health remaining");
            if (_health <= 0)
            {
                Instantiate(_debrisEffectPrefab, transform.position, Quaternion.identity);
                Instantiate(_smokeEffectPrefab, transform.position, Quaternion.identity);
                OnAnyInteractableDestroyed?.Invoke(this, EventArgs.Empty);
                Destroy(gameObject);
            }
        }
    }
}
