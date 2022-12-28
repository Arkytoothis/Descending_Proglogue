using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Combat;
using Descending.Core;
using Descending.Tiles;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Descending.Units
{
    public class ActionManager : MonoBehaviour
    {
        public static ActionManager Instance { get; private set; }
        
        [SerializeField] private LayerMask _playerUnitMask;

        private bool _isBusy = false;
        private BaseAction _selectedAction = null;
        
        public BaseAction SelectedAction => _selectedAction;

        public event EventHandler OnSelectedActionChanged;
        public event EventHandler OnActionStarted;
        public event EventHandler<bool> OnBusyChanged;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple Action Managers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        public void Setup()
        {
            //SetSelectedUnit(_selectedUnit);
            //UpdateSelectedVisual();
        }

        private void Update()
        {
            if (_isBusy) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (!TurnManager.Instance.IsPlayerTurn) return;
            if (TryHandleUnitSelection()) return;
            
            HandleSelectedAction();
        }

        private void HandleSelectedAction()
        {
            if (HeroManager_Combat.Instance.SelectedHero == null || _selectedAction == null) return;
            
            if (InputManager.Instance.GetRightMouseDown())
            {
                MapPosition mouseMapPosition = MapManager.Instance.GetGridPosition(CombatRaycaster.GetMouseWorldPosition());
                if (_selectedAction.IsValidActionGridPosition(mouseMapPosition))
                {
                    if (HeroManager_Combat.Instance.SelectedHero.TryPerformAction(_selectedAction))
                    {
                        SetBusy();
                        _selectedAction.PerformAction(mouseMapPosition, ClearBusy);
                        OnActionStarted?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }
        
        private bool TryHandleUnitSelection()
        {
            if (InputManager.Instance.GetLeftMouseDown())
            {
                Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMousePosition());

                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _playerUnitMask))
                {
                    if (hit.transform.TryGetComponent<Unit>(out Unit unit))
                    {
                        if (unit == HeroManager_Combat.Instance.SelectedHero)
                        {
                            return false;
                        }

                        if (unit.IsEnemy)
                        {
                            return false;
                        }
                        
                        HeroManager_Combat.Instance.SelectHero(unit as HeroUnit);
                        return true;
                    }
                }
            }

            return false;
        }

        public void SetSelectedAction(BaseAction action)
        {
            _selectedAction = action;
            UpdateSelectedVisual();
            
            OnSelectedActionChanged?.Invoke(_selectedAction, EventArgs.Empty);
        }
        
        private void SetBusy()
        {
            _isBusy = true;
            OnBusyChanged.Invoke(this, _isBusy);
        }

        private void ClearBusy()
        {
            _isBusy = false;
            OnBusyChanged.Invoke(this, _isBusy);
        }

        private void UpdateSelectedVisual()
        {
        }
    }
}
