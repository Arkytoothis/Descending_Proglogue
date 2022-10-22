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

namespace Descending.Units
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] protected Transform _hitTransform = null;
        [SerializeField] protected Transform _cameraMount = null;
        [SerializeField] protected Transform _cameraTarget = null;
        [SerializeField] protected Transform _modelParent = null;
        [SerializeField] protected UnitAnimator _unitAnimator = null;
        [SerializeField] protected UnitData _unitData = null;
        [SerializeField] protected AttributesController _attributes = null;
        [SerializeField] protected SkillsController _skills = null;
        [SerializeField] protected InventoryController _inventory = null;
        [SerializeField] protected AbilityController _abilities = null;
        [SerializeField] protected RagdollSpawner _ragdollSpawner = null;
        
        [SerializeField] protected UnitWorldPanel _worldPanel = null;
        
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
        
        public UnitData UnitData => _unitData;
        public AttributesController Attributes => _attributes;
        public SkillsController Skills => _skills;
        public InventoryController Inventory => _inventory;
        public AbilityController Abilities => _abilities;

        public HealthSystem HealthSystem => _healthSystem;

        public bool IsActive => _isActive;

        private void Awake()
        {
            _healthSystem = GetComponent<HealthSystem>();
            _actions = GetComponents<BaseAction>();
        }

        private void Start()
        {
            currentMapPosition = MapManager.Instance.GetGridPosition(transform.position);
            MapManager.Instance.AddUnitAtGridPosition(currentMapPosition, this);
            TurnManager.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
            UnitManager.Instance.UnitSpawned(this);
        }

        private void Update()
        {
            MapPosition newMapPosition = MapManager.Instance.GetGridPosition(transform.position);

            if (newMapPosition != currentMapPosition)
            {
                MapPosition oldPosition = currentMapPosition;
                currentMapPosition = newMapPosition;
                MapManager.Instance.UnitMovedGridPosition(this, oldPosition, newMapPosition);
            }
        }

        public void SetupHero(Genders gender, RaceDefinition race, ProfessionDefinition profession, int listIndex)
        {
            _isEnemy = false;
            _modelParent.ClearTransform();
            GameObject clone = Instantiate(race.PrefabMale, _modelParent);
            BodyRenderer worldRenderer = clone.GetComponent<BodyRenderer>();
            worldRenderer.SetupBody(gender, race, profession);
            _unitAnimator = GetComponent<UnitAnimator>();
            _unitAnimator.Setup(clone.GetComponent<Animator>());
            
            ((HeroData)_unitData).Setup(gender, race, profession, worldRenderer, listIndex);
            _attributes.Setup(race, profession);
            _skills.Setup(_attributes, race, profession);
            _inventory.Setup(null, worldRenderer, gender, race, profession);
            _abilities.Setup(race, profession, _skills);
            
            _healthSystem.Setup(100);
                
            _worldPanel.Setup(this);
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

        public string GetName()
        {
            return gameObject.name;
        }
    }
}
