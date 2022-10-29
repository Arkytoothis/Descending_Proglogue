using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Descending.Core
{
    public class WorldRaycaster : MonoBehaviour
    {
        private static WorldRaycaster instance;

        [SerializeField] private GameObject _cursor;
        [SerializeField] private LayerMask _groundMask;

        [SerializeField] private MapPositionEvent onDisplayCurrentTile = null;

        private MapPosition _lastMapPosition;
        private MapPosition _currentMapPosition;
        
        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            if (MapManager.Instance.GetGridPosition(GetMouseWorldPosition()) != _lastMapPosition)
            {
                _currentMapPosition = MapManager.Instance.GetGridPosition(GetMouseWorldPosition());
                onDisplayCurrentTile.Invoke(_currentMapPosition);
                _cursor.transform.position = MapManager.Instance.GetWorldPosition(_currentMapPosition);
                _lastMapPosition = _currentMapPosition;
            }
        }

        public static Vector3 GetMouseWorldPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMousePosition());
            Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, instance._groundMask);
            return hit.point;
        }
    }
}
