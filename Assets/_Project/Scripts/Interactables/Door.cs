using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using DG.Tweening;
using UnityEngine;

namespace Descending.Interactables
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform _doorTransform;
        [SerializeField] private bool _isOpen = false;
        [SerializeField] private float _openDuration = 0.5f;
        [SerializeField] private float _closeDuration = 0.5f;
        [SerializeField] private float _openAngle = 100f;
        [SerializeField] private float _closeAngle = -100f;
        
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
            //Debug.Log("Interacting with Door");
            onComplete = onInteractionComplete;
            _isInteracting = true;
            _timer = 0.5f;
            
            if (_isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        public string GetName()
        {
            return "Door";
        }

        private void Open()
        {
            _isOpen = true;
            PathfindingManager.Instance.SetIsGridPositionWalkable(mapPosition, true);
            _doorTransform.DORotate(new Vector3(0f, _openAngle, 0f), _openDuration, RotateMode.LocalAxisAdd);
        }

        private void Close()
        {
            _isOpen = false;
            PathfindingManager.Instance.SetIsGridPositionWalkable(mapPosition, false);
            _doorTransform.DORotate(new Vector3(0f, _closeAngle, 0f), _closeDuration, RotateMode.LocalAxisAdd);
        }
    }
}
