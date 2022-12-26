using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using Descending.Abilities;
using Descending.Core;
using Descending.Equipment;
using Descending.Units;
using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Descending
{
    public class ActionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Button _button = null;
        [SerializeField] private Image _iconImage = null;
        [SerializeField] private TMP_Text _hotkeyLabel = null;
        [SerializeField] private GameObject _border = null;
        
        [SoundGroup, SerializeField] private string _clickSound;

        [SerializeField] private AbilityEvent onDisplayAbilityTooltip = null;
        [SerializeField] private ItemEvent onDisplayItemTooltip = null;
        
        private BaseAction _action = null;
        private Ability _ability = null;
        private Item _item = null;
        
        public void SetAction(BaseAction action)
        {
            _action = action;
            _iconImage.sprite = action.Icon;
            _hotkeyLabel.SetText(action.GetName().ToUpper());
            _button.onClick.AddListener(ActionButton_OnClick);
        }

        public void SetAbility(Ability ability)
        {
            _ability = ability;
            _iconImage.sprite = ability.Definition.Icon;
            
            if(ability.AbilityType == AbilityType.Power)
                _hotkeyLabel.SetText("USE");
            else if(ability.AbilityType == AbilityType.Spell)
                _hotkeyLabel.SetText("CAST");
        }

        public void SetItem(Item item)
        {
            _item = item;
        }

        public void SetHotkey(string hotkey)
        {
            _hotkeyLabel.SetText(hotkey);    
        }

        public void SetIcon(Sprite icon)
        {
            _iconImage.sprite = icon;
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
            else if (_item != null)
            {
                onDisplayItemTooltip.Invoke(_item);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onDisplayAbilityTooltip.Invoke(null);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            MasterAudio.PlaySound(_clickSound);
        }
    }
}