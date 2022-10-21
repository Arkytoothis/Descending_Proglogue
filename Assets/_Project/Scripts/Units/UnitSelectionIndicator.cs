using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Units
{
    public class UnitSelectionIndicator : MonoBehaviour
    {
        [SerializeField] private Unit _unit = null;

        private MeshRenderer _meshRenderer = null;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            ActionManager.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
            UpdateIndicator();
        }

        private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs args)
        {
            UpdateIndicator();
        }

        private void UpdateIndicator()
        {
            if (ActionManager.Instance.SelectedUnit == _unit)
            {
                _meshRenderer.enabled = true;
            }
            else
            {
                _meshRenderer.enabled = false;
            }
        }

        private void OnDestroy()
        {
            ActionManager.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
        }
    }
}
