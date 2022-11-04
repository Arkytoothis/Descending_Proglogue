using System.Collections;
using System.Collections.Generic;
using Descending.Party;
using Descending.Units;
using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Descending.Gui
{
    public class PartyMemberWidget : MonoBehaviour
    {
        [SerializeField] private Button _button = null;
        [SerializeField] private TMP_Text _nameLabel = null;
        [SerializeField] private Image _borderImage = null;
        [SerializeField] private RawImage _portraitImage = null;
        [SerializeField] private GameObject _vitalBarsContainer = null;
        [SerializeField] private Color _selectedColor = Color.white;
        [SerializeField] private Color _deselectedColor = Color.white;
        [SerializeField] private List<VitalBar> _vitalBars = null;
        [SerializeField] private VitalBar _experienceBar = null;

        private HeroUnit _hero = null;
        
        public void Setup(HeroUnit hero)
        {
            Clear();
            
            if (hero != null)
            {
                _hero = hero;
                _button.interactable = true;
                //_borderImage.color = Color.gray;
                _nameLabel.SetText(hero.GetShortName());
                _portraitImage.gameObject.SetActive(true);
                _portraitImage.texture = hero.Portrait.RtClose;
                
                _vitalBarsContainer.SetActive(true);
                _vitalBars[0].UpdateData(hero.Attributes.GetVital("Actions").Current, hero.Attributes.GetVital("Actions").Maximum);
                _vitalBars[1].UpdateData(hero.Attributes.GetVital("Armor").Current, hero.Attributes.GetVital("Armor").Maximum);
                _vitalBars[2].UpdateData(hero.Attributes.GetVital("Life").Current, hero.Attributes.GetVital("Life").Maximum);
                _vitalBars[3].UpdateData(hero.Attributes.GetVital("Stamina").Current, hero.Attributes.GetVital("Stamina").Maximum);
                _vitalBars[4].UpdateData(hero.Attributes.GetVital("Magic").Current, hero.Attributes.GetVital("Magic").Maximum);
                
                _experienceBar.gameObject.SetActive(true);
                _experienceBar.UpdateData(hero.HeroData.Experience, hero.HeroData.ExpToNextLevel);
            }
        }

        public void Clear()
        {
            _hero = null;
            _button.interactable = false;
            _borderImage.color = new Color(0.25f, 0.25f, 0.25f, 1f);
            _nameLabel.SetText("");
            _portraitImage.gameObject.SetActive(false);
            _vitalBarsContainer.SetActive(false);
            _experienceBar.gameObject.SetActive(false);
        }

        public void Select()
        {
            _borderImage.color = _selectedColor;
        }

        public void Deselect()
        {
            _borderImage.color = _deselectedColor;
        }

        public void OnClick()
        {
            UnitManager.Instance.SetPartyLeader(_hero.gameObject);
        }
    }
}