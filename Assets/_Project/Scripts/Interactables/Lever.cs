using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using DG.Tweening;
using UnityEngine;

namespace Descending.Interactables
{
    public class Lever : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool _isActive = false;
        [SerializeField] private Transform _leverTransform = null;
        [SerializeField] private float _activateDuration = 0.5f;
        [SerializeField] private float _deactivateDuration = 0.5f;
        [SerializeField] private float _activateAngle = 50f;
        [SerializeField] private float _deactivateAngle = -50f;
        
        private MapPosition mapPosition;
        private Action onComplete;
        private float _timer;
        private bool _isInteracting;
        
        private void Start()
        {
            InteractableManager.Instance.RegisterInteractable(this);
        }

        public void Setup()
        {
            mapPosition = MapManager.Instance.GetGridPosition(transform.position);
            MapManager.Instance.SetInteractableAtGridPosition(mapPosition, this);
            PathfindingManager.Instance.SetIsGridPositionWalkable(mapPosition, false);
            _isInteracting = false;
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
            //Debug.Log("Interacting with Lever");
            onComplete = onInteractionComplete;
            _isInteracting = true;
            _timer = 0.5f;
            
            if (_isActive)
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
            return "Lever";
        }

        public void Activate()
        {
            _isActive = true;
            _leverTransform.DORotate(new Vector3(_activateAngle, 0f, 0f), _activateDuration, RotateMode.LocalAxisAdd);
        }

        public void Deactivate()
        {
            _isActive = false;
            _leverTransform.DORotate(new Vector3(_deactivateAngle, 0f, 0f), _deactivateDuration, RotateMode.LocalAxisAdd); 
        }
    }
}