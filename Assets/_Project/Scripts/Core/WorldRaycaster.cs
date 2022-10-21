using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Core
{
    public class WorldRaycaster : MonoBehaviour
    {
        private static WorldRaycaster instance;

        [SerializeField] private GameObject _cursor;
        [SerializeField] private LayerMask _groundMask;

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            _cursor.transform.position = GetWorldPosition();
        }

        public static Vector3 GetWorldPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMousePosition());
            Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, instance._groundMask);
            return hit.point;
        }
    }
}
