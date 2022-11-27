using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Abilities;
using Descending.Attributes;
using Descending.Combat;
using Descending.Core;
using Descending.Equipment;
using Descending.Gui;
using Descending.Tiles;
using UnityEngine;
using Attribute = Descending.Attributes.Attribute;

namespace Descending.Units
{
    public abstract class Unit : MonoBehaviour
    {
        [SerializeField] protected GameObject _selectionIndicator = null;
        [SerializeField] protected Transform _hitTransform = null;
        [SerializeField] protected Transform _combatTextTransform = null;
        [SerializeField] protected Transform _projectileSpawnPoint = null;
        [SerializeField] protected Transform _cameraMount = null;
        [SerializeField] protected Transform _cameraTarget = null;
        [SerializeField] protected Transform _modelParent = null;
        [SerializeField] protected UnitAnimator _unitAnimator = null;
        [SerializeField] protected AttributesController _attributes = null;
        [SerializeField] protected SkillsController _skills = null;
        [SerializeField] protected InventoryController _inventory = null;
        [SerializeField] protected AbilityController _abilities = null;
        [SerializeField] protected RagdollSpawner _ragdollSpawner = null;
        [SerializeField] protected ActionController _actionController = null;
        [SerializeField] protected UnitWorldPanel _worldPanel = null;
        [SerializeField] protected UnitEffects _unitEffects = null;
        
        public abstract void SpendActionPoints(int actionPointCost);
        
        protected bool _isEnemy = false;
        protected DamageSystem _damageSystem;
        protected MapPosition currentMapPosition;
        protected bool _isActive = false;
        protected bool _isAlive = false;
        
        public MapPosition CurrentMapPosition => currentMapPosition;
        public bool IsEnemy => _isEnemy;
        public Transform HitTransform => _hitTransform;
        public Transform CombatTextTransform => _combatTextTransform;
        public Transform CameraMount => _cameraMount;
        public Transform CameraTarget => _cameraTarget;
        public AttributesController Attributes => _attributes;
        public SkillsController Skills => _skills;
        public InventoryController Inventory => _inventory;
        public AbilityController Abilities => _abilities;
        public ActionController ActionController => _actionController;
        public DamageSystem DamageSystem => _damageSystem;
        public UnitAnimator UnitAnimator => _unitAnimator;
        public Transform ProjectileSpawnPoint => _projectileSpawnPoint;
        public UnitEffects UnitEffects => _unitEffects;

        public bool IsActive => _isActive;
        public bool IsAlive => _isAlive;

        public abstract string GetFullName();
        public abstract string GetShortName();
        public abstract Item GetMeleeWeapon();
        public abstract Item GetRangedWeapon();
        public abstract void Damage(GameObject attacker, DamageTypeDefinition damageType, int damage, string vital);
        public abstract void RestoreVital(string vital, int damage);
        public abstract void UseResource(string vital, int damage);
        protected abstract void Dead();
        
        private void Awake()
        {
            _damageSystem = GetComponent<DamageSystem>();
            _isAlive = true;
        }

        private void Start()
        {
            if (MapManager.Instance != null)
            {
                currentMapPosition = MapManager.Instance.GetGridPosition(transform.position);
                MapManager.Instance.AddUnitAtGridPosition(currentMapPosition, this);
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

        public Attribute GetActions()
        {
            return _attributes.GetVital("Actions");
        }

        public Attribute GetArmor()
        {
            return _attributes.GetVital("Armor");
        }

        public Attribute GetLife()
        {
            return _attributes.GetVital("Life");
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
            if (GetActions().Current >= action.GetActionPointCost())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void OnTurnChanged(bool b)
        {
            if((_isEnemy && !TurnManager.Instance.IsPlayerTurn) ||
               (!_isEnemy && TurnManager.Instance.IsPlayerTurn))
            {
                _attributes.RefreshActions();
                _worldPanel.UpdateActionPoints();
                _unitEffects.NextTurn();
            }
        }

        public float GetHealth()
        {
            return _attributes.GetVital("Life").Current;
        }

        public void Select()
        {
            _selectionIndicator.SetActive(true);
        }

        public void Deselect()
        {
            _selectionIndicator.SetActive(false);
        }

        public T GetAction<T>() where T : BaseAction
        {
            return _actionController.GetAction<T>();
        }

        public void AddUnitEffect(AbilityEffect abilityEffect)
        {
            _unitEffects.AddEffect(abilityEffect);
        }

        public void RecalculateAttributes()
        {
            _attributes.CalculateModifiers();
        }

        public void SyncWorldPanel()
        {
            _worldPanel.Sync();
        }
    }
}
