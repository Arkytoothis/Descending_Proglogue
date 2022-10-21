using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Combat;
using Descending.Units;
using Descending.Core;
using Descending.Tiles;
using UnityEngine;

namespace Descending.Enemies
{
    public class EnemyAI : MonoBehaviour
    {
        public enum AiStates { Inactive, Acting, Busy }

        [SerializeField] private float _actionDelay = 1f;
        [SerializeField] private float _actionCompleteDelay = 0.5f;
        
        private AiStates _currentState = AiStates.Inactive;
        private float _timer = 0;

        public void Setup()
        {
            TurnManager.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        }

        private void Update()
        {
            if (TurnManager.Instance.IsPlayerTurn) return;

            switch (_currentState)
            {
                case AiStates.Inactive:
                    break;
                case AiStates.Acting:
                    _timer -= Time.deltaTime;
                    if (_timer <= 0)
                    {
                        if (TryPerformAction(ActionComplete))
                        {
                            _currentState = AiStates.Busy;
                        }
                        else
                        {
                            TurnManager.Instance.NextTurn();
                        }
                    }
            
                    break;
                case AiStates.Busy:
                    break;
            }
        }

        private void ActionComplete()
        {
            _timer = _actionCompleteDelay;
            _currentState = AiStates.Acting;
        }

        private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
        {
            if (!TurnManager.Instance.IsPlayerTurn)
            {
                _currentState = AiStates.Acting;
                _timer = _actionDelay;
            }
        }

        private bool TryPerformAction(Action onActionComplete)
        {
            foreach (Unit enemyUnit in UnitManager.Instance.EnemyUnits)
            {
                if (TryPerformAction(enemyUnit, onActionComplete))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryPerformAction(Unit enemyUnit, Action onActionComplete)
        {
            EnemyAction bestEnemyAction = null;
            BaseAction bestAction = null;
            
            foreach (BaseAction action in enemyUnit.Actions)
            {
                if (!enemyUnit.HasActionPoints(action))
                {
                    continue;
                }

                if (bestEnemyAction == null)
                {
                    bestEnemyAction = action.GetBestEnemyAction();
                    bestAction = action;
                }
                else
                {
                    EnemyAction testEnemyAction = action.GetBestEnemyAction();
                    if (testEnemyAction != null && testEnemyAction.ActionValue > bestEnemyAction.ActionValue)
                    {
                        bestEnemyAction = testEnemyAction;
                        bestAction = action;
                    }
                }
            }

            if (bestEnemyAction != null && enemyUnit.TryPerformAction(bestAction))
            {
                bestAction.PerformAction(bestEnemyAction._mapPosition, onActionComplete);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}