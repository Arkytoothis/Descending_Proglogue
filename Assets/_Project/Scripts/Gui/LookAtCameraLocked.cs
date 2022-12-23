using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Gui
{
    public class LookAtCameraLocked : MonoBehaviour
    {
        private Transform _cameraTransform;

        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _cameraTransform.rotation * Vector3.forward, _cameraTransform.rotation * Vector3.up);
        }
    }
}
