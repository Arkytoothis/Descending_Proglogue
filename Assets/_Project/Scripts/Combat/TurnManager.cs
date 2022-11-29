using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Combat
{
    public class TurnManager : MonoBehaviour
    {
        public static TurnManager Instance { get; private set; }

        [SerializeField] private BoolEvent onTurnChanged = null;
        
        private int _turnNumber = 0;
        private bool _isPlayerTurn = true;

        public int TurnNumber => _turnNumber;
        public bool IsPlayerTurn => _isPlayerTurn;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple Turn Managers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        public void Setup()
        {
        }
        
        public void NextTurn()
        {
            _turnNumber++;
            _isPlayerTurn = !_isPlayerTurn;
            
            onTurnChanged.Invoke(true);
            UnitManager.Instance.SyncHeroes();
        }
    }
}