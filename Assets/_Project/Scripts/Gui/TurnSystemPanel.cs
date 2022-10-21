using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Combat;
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
            TurnManager.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
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

        private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
        {
            UpdateLabel();
            UpdateEnemyTurnBlocker();
            UpdateEndTurnButton();
        }

        private void UpdateEnemyTurnBlocker()
        {
            _enemyTurnBlocker.SetActive(!TurnManager.Instance.IsPlayerTurn);
        }

        private void UpdateEndTurnButton()
        {
            _button.gameObject.SetActive(TurnManager.Instance.IsPlayerTurn);
        }
    }
}