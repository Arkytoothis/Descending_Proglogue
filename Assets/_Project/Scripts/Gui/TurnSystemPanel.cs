using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Combat;
using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Descending.Gui
{
    public class TurnSystemPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _turnLabel = null;
        [SerializeField] private Button _button = null;
        [SerializeField] private GameObject _enemyTurnBlocker = null;
        
        public void Setup()
        {
            UpdateEnemyTurnBlocker();
            UpdateEndTurnButton();
        }

        public void EndTurn_Click()
        {
            TurnManager.Instance.NextTurn();
        }

        private void UpdateLabel()
        {
            _turnLabel.SetText("Turn: " + TurnManager.Instance.TurnNumber);
        }

        private void UpdateEnemyTurnBlocker()
        {
            _enemyTurnBlocker.SetActive(!TurnManager.Instance.IsPlayerTurn);
        }

        private void UpdateEndTurnButton()
        {
            _button.gameObject.SetActive(TurnManager.Instance.IsPlayerTurn);
        }

        public void OnTurnChanged(bool b)
        {
            UpdateLabel();
            UpdateEnemyTurnBlocker();
            UpdateEndTurnButton();
        }
    }
}