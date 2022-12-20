using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Tiles;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Descending.Combat
{
    public class CombatRaycaster : MonoBehaviour
    {
        private static CombatRaycaster instance;

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
            
            _currentMapPosition = MapManager.Instance.GetGridPosition(GetMouseWorldPosition());
            _cursor.transform.position = MapManager.Instance.GetWorldPosition(_currentMapPosition);
            
            if (MapManager.Instance.GetGridPosition(GetMouseWorldPosition()) != _lastMapPosition)
            {
                onDisplayCurrentTile.Invoke(_currentMapPosition);
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
