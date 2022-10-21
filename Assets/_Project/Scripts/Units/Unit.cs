using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Abilities;
using Descending.Attributes;
using Descending.Combat;
using Descending.Core;
using Descending.Equipment;
using Descending.Tiles;
using UnityEngine;

namespace Descending.Units
{
    public class Unit : MonoBehaviour
    {
        public static event EventHandler OnAnyActionPointsChanged = null;
        public static event EventHandler OnAnyUnitSpawned = null;
        public static event EventHandler OnAnyUnitDead = null;

        [SerializeField] private Transform _hitTransform = null;
        [SerializeField] private Transform _cameraMount = null;
        [SerializeField] private Transform _cameraTarget = null;
        [SerializeField] private Transform _modelParent = null;
        [SerializeField] private int _maxActionPoints = 2;
        [SerializeField] private UnitAnimator _unitAnimator = null;
        [SerializeField] private UnitData _unitData = null;
        [SerializeField] private AttributesController _attributes = null;
        [SerializeField] private SkillsController _skills = null;
        [SerializeField] private InventoryController _inventory = null;
        [SerializeField] private AbilityController _abilities = null;

        protected bool _isEnemy = false;
        private HealthSystem _healthSystem;
        private MapPosition currentMapPosition;
        private BaseAction[] _actions = null;
        private int _actionPoints;
        
        public MapPosition CurrentMapPosition => currentMapPosition;
        public BaseAction[] Actions => _actions;
        public int ActionPoints => _actionPoints;
        public bool IsEnemy => _isEnemy;
        public Transform HitTransform => _hitTransform;
        public Transform CameraMount => _cameraMount;
        public Transform CameraTarget => _cameraTarget;
        
        public UnitData UnitData => _unitData;
        public AttributesController Attributes => _attributes;
        public SkillsController Skills => _skills;
        public InventoryController Inventory => _inventory;
        public AbilityController Abilities => _abilities;

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
            _healthSystem.OnDead += HealthSystem_OnDead;
            OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
            _actionPoints = _maxActionPoints;
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
            if (_actionPoints >= action.GetActionPointCost())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SpendActionPoints(int actionPoints)
        {
            _actionPoints -= actionPoints;
            OnAnyActionPointsChanged.Invoke(this, EventArgs.Empty);
        }

        private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
        {
            if((_isEnemy && !TurnManager.Instance.IsPlayerTurn) ||
               (!_isEnemy && TurnManager.Instance.IsPlayerTurn))
            {
                _actionPoints = _maxActionPoints;
                OnAnyActionPointsChanged.Invoke(this, EventArgs.Empty);
            }
        }

        public void Damage(GameObject attacker, int damage)
        {
            _healthSystem.TakeDamage(attacker, damage);
        }

        private void HealthSystem_OnDead(object sender, EventArgs e)
        {
            MapManager.Instance.RemoveUnitAtGridPosition(currentMapPosition, this);
            OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
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
