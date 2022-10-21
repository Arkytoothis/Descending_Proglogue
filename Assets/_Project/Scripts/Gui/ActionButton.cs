using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Descending
{
    public class ActionButton : MonoBehaviour
    {
        [SerializeField] private Button _button = null;
        [SerializeField] private TMP_Text _actionLabel = null;
        [SerializeField] private GameObject _border = null;

        private BaseAction _action = null;
        
        public void SetAction(BaseAction action)
        {
            _action = action;
            _actionLabel.SetText(action.GetName().ToUpper());
            _button.onClick.AddListener(ActionButton_OnClick);
        }

        private void ActionButton_OnClick()
        {
            ActionManager.Instance.SetSelectedAction(_action);
        }

        public void UpdateSelectedVisual()
        {
            _border.SetActive(_action == ActionManager.Instance.SelectedAction);
        }
    }
}