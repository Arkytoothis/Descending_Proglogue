using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Abilities;
using Descending.Attributes;
using Descending.Combat;
using Descending.Equipment;
using Descending.Gui;
using Descending.Tiles;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Units
{
    public abstract class Unit : MonoBehaviour
    {
        [SerializeField] protected GameObject _selectionIndicator = null;
        [SerializeField] protected Transform _hitTransform = null;
        [SerializeField] protected Transform _cameraMount = null;
        [SerializeField] protected Transform _cameraTarget = null;
        [SerializeField] protected Transform _modelParent = null;
        [SerializeField] protected UnitAnimator _unitAnimator = null;
        [SerializeField] protected AttributesController _attributes = null;
        [SerializeField] protected SkillsController _skills = null;
        [SerializeField] protected InventoryController _inventory = null;
        [SerializeField] protected AbilityController _abilities = null;
        [SerializeField] protected RagdollSpawner _ragdollSpawner = null;
        
        [SerializeField] protected UnitWorldPanel _worldPanel = null;

        [SerializeField] protected BoolEvent onSyncParty = null;
        
        protected bool _isEnemy = false;
        protected HealthSystem _healthSystem;
        protected MapPosition currentMapPosition;
        protected BaseAction[] _actions = null;
        protected bool _isActive = false;
        
        public MapPosition CurrentMapPosition => currentMapPosition;
        public BaseAction[] Actions => _actions;
        public bool IsEnemy => _isEnemy;
        public Transform HitTransform => _hitTransform;
        public Transform CameraMount => _cameraMount;
        public Transform CameraTarget => _cameraTarget;
        public AttributesController Attributes => _attributes;
        public SkillsController Skills => _skills;
        public InventoryController Inventory => _inventory;
        public AbilityController Abilities => _abilities;

        public HealthSystem HealthSystem => _healthSystem;

        public bool IsActive => _isActive;

        public abstract string GetFullName();
        public abstract string GetShortName();
        
        private void Awake()
        {
            _healthSystem = GetComponent<HealthSystem>();
            _actions = GetComponents<BaseAction>();
        }

        private void Start()
        {
            if (MapManager.Instance != null)
            {
                currentMapPosition = MapManager.Instance.GetGridPosition(transform.position);
                MapManager.Instance.AddUnitAtGridPosition(currentMapPosition, this);
            }

            if (TurnManager.Instance != null)
            {
                TurnManager.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
            }

            Deselect();
        }

        private void Update()
        {
            if (MapManager.Instance == null) return;
            
            MapPosition newMapPosition = MapManager.Instance.GetGridPosition(transform.position);

            if (newMapPosition != currentMapPosition)
            {
                MapPosition oldPosition = currentMapPosition;
                currentMapPosition = newMapPosition;
                MapManager.Instance.UnitMovedGridPosition(this, oldPosition, newMapPosition);
            }
        }

        public int GetActionsCurrent()
        {
            return _attributes.GetVital("Actions").Current;
        }
        
        public bool TryPerformAction(BaseAction action)
        {
            if (HasActionPoints(action))
            {
                SpendActionPoints(action.GetActionPointCost());
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool HasActionPoints(BaseAction action)
        {
            if (GetActionsCurrent() >= action.GetActionPointCost())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SpendActionPoints(int actionPointCost)
        {
            _attributes.ModifyVital("Actions", actionPointCost);
            _worldPanel.UpdateActionPoints(this);
            onSyncParty.Invoke(true);
        }

        private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
        {
            if((_isEnemy && !TurnManager.Instance.IsPlayerTurn) ||
               (!_isEnemy && TurnManager.Instance.IsPlayerTurn))
            {
                _attributes.RefreshActions();
                _worldPanel.UpdateActionPoints(this);
            }
        }

        public void Damage(GameObject attacker, int damage)
        {
            _healthSystem.TakeDamage(attacker, damage);

            if (GetHealth() <= 0)
            {
                Dead();
            }
            
            onSyncParty.Invoke(true);
        }

        private void Dead()
        {
            MapManager.Instance.RemoveUnitAtGridPosition(currentMapPosition, this);
            UnitManager.Instance.UnitDead(this);
            _ragdollSpawner.Activate(_healthSystem);
            Destroy(gameObject);
        }

        public float GetHealth()
        {
            return _healthSystem.GetHealthNormalized();
        }

        public T GetAction<T>() where T : BaseAction
        {
            foreach (BaseAction action in _actions)
            {
                if (action is T)
                {
                    return (T)action;
                }
            }

            return null;
        }

        public void Select()
        {
            _selectionIndicator.SetActive(true);
        }

        public void Deselect()
        {
            _selectionIndicator.SetActive(false);
        }
    }
}
