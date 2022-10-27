using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Units
{
    public class UnitSelectionIndicator : MonoBehaviour
    {
        [SerializeField] private float _rotateSpeed = 10f;
        
        private void Update()
        {
            transform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime);
        }
    }
}
