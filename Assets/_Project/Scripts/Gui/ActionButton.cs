using System.Collections;
using System.Collections.Generic;
using Descending.Abilities;
using Descending.Core;
using Descending.Equipment;
using Descending.Units;
using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Descending
{
    public class ActionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Button _button = null;
        [SerializeField] private Image _iconImage = null;
        [SerializeField] private TMP_Text _actionLabel = null;
        [SerializeField] private GameObject _border = null;

        [SerializeField] private AbilityEvent onDisplayAbilityTooltip = null;
        [SerializeField] private ItemEvent onDisplayItemTooltip = null;
        
        private BaseAction _action = null;
        private Ability _ability = null;
        private Item _item = null;
        
        public void SetAction(BaseAction action)
        {
            _action = action;
            _iconImage.sprite = action.Icon;
            _actionLabel.SetText(action.GetName().ToUpper());
            _button.onClick.AddListener(ActionButton_OnClick);
        }

        public void SetAbility(Ability ability)
        {
            _ability = ability;
            _iconImage.sprite = ability.Definition.Details.Icon;
            
            if(ability.AbilityType == AbilityType.Power)
                _actionLabel.SetText("USE");
            else if(ability.AbilityType == AbilityType.Spell)
                _actionLabel.SetText("CAST");
        }

        public void SetItem(Item item)
        {
            _item = item;
        }
        
        private void ActionButton_OnClick()
        {
            ActionManager.Instance.SetSelectedAction(_action);
        }

        public void UpdateSelectedVisual()
        {
            _border.SetActive(_action == ActionManager.Instance.SelectedAction);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_ability != null)
            {
                onDisplayAbilityTooltip.Invoke(_ability);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onDisplayAbilityTooltip.Invoke(null);
        }
    }
}