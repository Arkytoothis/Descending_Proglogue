using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Gui
{

    public class LookAtCamera : MonoBehaviour
    {
        private Transform _cameraTransform;

        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(_cameraTransform);
        }
    }
}
