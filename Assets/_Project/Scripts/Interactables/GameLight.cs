using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Descending.Interactables
{
    public class GameLight : MonoBehaviour, IInteractable
    {
        [SerializeField] private Light _light = null;
        [SerializeField] private bool _blocksMovement = false;
        [SerializeField] private List<GameObject> _effects = null;

        private MapPosition _mapPosition;
        private bool _isOn = false;
        private Action onComplete;
        private float _timer;
        private bool _isInteracting;
        
        private void Awake()
        {
            _light.enabled = false;
        }

        private void Start()
        {
            InteractableManager.Instance.RegisterInteractable(this);
        }

        public void Setup()
        {
            if (_blocksMovement == true)
            {
                _mapPosition = MapManager.Instance.GetGridPosition(transform.position);
                MapManager.Instance.SetInteractableAtGridPosition(_mapPosition, this);
                PathfindingManager.Instance.SetIsGridPositionWalkable(_mapPosition, false);
            }
        }
        
        public void Activate()
        {
            StartCoroutine(DelayedSetLightActive(true));
        }

        public void Deactivate()
        {
            _isOn = false;
            _light.enabled = false;

            foreach (GameObject effect in _effects)
            {
                effect.SetActive(false);
            }
        }

        public void SetLightShadows(LightShadows lightShadows)
        {
            _light.shadows = lightShadows;
        }
        
        private IEnumerator DelayedSetLightActive(bool active)
        {
            yield return new WaitForSeconds(0.1f);

            _isOn = true;
            _light.enabled = true;
            
            foreach (GameObject effect in _effects)
            {
                effect.SetActive(true);
            }
        }

        private void Update()
        {
            if (!_isInteracting) return;
            
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _isInteracting = false;
                onComplete();
            }
        }

        public void Interact(Action onInteractionComplete)
        {
            Debug.Log("Interacting with Light");
            onComplete = onInteractionComplete;
            _isInteracting = true;
            _timer = 0.5f;
            
            if (_isOn)
            {
                Deactivate();
            }
            else
            {
                Activate();
            }
        }

        public string GetName()
        {
            return "Light";
        }
    }
}