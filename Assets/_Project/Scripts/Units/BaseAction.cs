using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using Descending.Tiles;
using UnityEngine;

namespace Descending.Units
{
    public enum ActionTypes { Move, Interact, Melee_Attack, Ranged_Attack, Use_Ability, Use_Item, Number, None }
    
    public abstract class BaseAction : MonoBehaviour
    {
        public static event EventHandler OnActionStarted;
        public static event EventHandler OnActionCompleted;
        
        [SerializeField] protected Sprite _icon = null;
        [SoundGroup, SerializeField] protected string _performActionSound;
        
        protected Unit _unit = null;
        protected bool _isActive = false;
        protected Action onActionComplete;
        
        public Unit Unit => _unit;
        public Sprite Icon => _icon;

        protected  virtual void Awake()
        {
            _unit = GetComponentInParent<Unit>();
        }

        public abstract string GetName();
        public abstract void PerformAction(MapPosition mapPosition, Action onActionComplete);
        public abstract List<MapPosition> GetValidActionGridPositions();
        public abstract int GetActionPointCost();

        public virtual bool IsValidActionGridPosition(MapPosition mapPosition)
        {
            List<MapPosition> validGridPositions = GetValidActionGridPositions();
            return validGridPositions.Contains(mapPosition);
        }

        protected void ActionStart(Action onActionComplete)
        {
            _isActive = true;
            this.onActionComplete = onActionComplete;
            
            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }

        protected void ActionComplete()
        {
            _isActive = false;
            onActionComplete();
            
            OnActionCompleted?.Invoke(this, EventArgs.Empty);
        }

        public EnemyAction GetBestEnemyAction()
        {
            List<EnemyAction> actions = new List<EnemyAction>();
            List<MapPosition> validGridPositions = GetValidActionGridPositions();

            foreach (MapPosition gridPosition in validGridPositions)
            {
                EnemyAction enemyAction = GetEnemyAction(gridPosition);
                actions.Add(enemyAction);
            }

            if (actions.Count > 0)
            {
                actions.Sort((EnemyAction a, EnemyAction b) => b.ActionValue - a.ActionValue);

                return actions[0];
            }
            else
            {
                return null;
            }
        }

        public abstract EnemyAction GetEnemyAction(MapPosition mapPosition);
    }
}
