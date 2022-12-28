using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Combat;
using Descending.Units;
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

        public void OnTurnChanged(bool b)
        {
            DeselectAll();
            
            if (!TurnManager.Instance.IsPlayerTurn)
            {
                _currentState = AiStates.Acting;
                _timer = _actionDelay;
            }
        }

        private void DeselectAll()
        {
            foreach (Unit enemyUnit in EnemyManager.Instance.EnemyUnits)
            {
                enemyUnit.Deselect();
            } 
        }
        private void SelectEnemy(Unit enemy)
        {
            foreach (Unit enemyUnit in EnemyManager.Instance.EnemyUnits)
            {
                if (enemy == enemyUnit)
                {
                    enemyUnit.Select();
                }
                else
                {
                    enemyUnit.Deselect();
                }
            }
        }
        private bool TryPerformAction(Action onActionComplete)
        {
            foreach (EnemyUnit enemyUnit in EnemyManager.Instance.EnemyUnits)
            {
                SelectEnemy(enemyUnit);
                
                if (TryPerformAction(enemyUnit, onActionComplete))
                {
                    return true;
                }
            }

            return false;
        }
        
        private bool TryPerformAction(EnemyUnit enemyUnit, Action onActionComplete)
        {
            if (enemyUnit.IsActive == false || enemyUnit.IsAlive == false || enemyUnit.Behavior == null) return false;
            
            BaseAction bestAction = null;
            MapPosition targetPosition = new MapPosition();
            bestAction = enemyUnit.Behavior.ProcessAction(enemyUnit, ref targetPosition);
            
            if(bestAction == null)
            {
                Debug.Log("bestAction == null");
                return false;
            }
            else 
            {
                if (enemyUnit.TryPerformAction(bestAction))
                {
                    bestAction.PerformAction(targetPosition, onActionComplete);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}