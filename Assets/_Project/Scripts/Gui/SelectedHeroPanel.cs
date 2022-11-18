using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Equipment;
using Descending.Units;
using TMPro;
using UnityEngine;

namespace Descending.Gui
{
    public class SelectedHeroPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameLabel = null;
        [SerializeField] private TMP_Text _actionPointsLabel = null;
        [SerializeField] private TMP_Text _lifeLabel = null;
        [SerializeField] private TMP_Text _experienceLabel = null;
        
        [SerializeField] private HeroMeleeWidget _meleeWidget = null;
        [SerializeField] private HeroRangedWidget _rangedWidget = null;
        
        
        private HeroUnit _hero = null;
        
        public void Setup()
        {
            
        }
        
        public void SyncParty(bool b)
        {
            if (_hero == null) return;
            
            DisplayHero();
        }

        public void OnSelectHero(GameObject heroObject)
        {
            _hero = heroObject.GetComponent<HeroUnit>();
            DisplayHero();
        }

        private void DisplayHero()
        {
            _nameLabel.SetText(_hero.GetFullName());
            _actionPointsLabel.SetText("AP: " + _hero.GetActions().Current + "/" + _hero.GetActions().Maximum);
            _lifeLabel.SetText("Life: " + _hero.Attributes.GetVital("Life").Current + "/" + _hero.Attributes.GetVital("Life").Maximum);
            _experienceLabel.SetText("Exp: " + _hero.HeroData.Experience + "/" + _hero.HeroData.ExpToNextLevel);

            _meleeWidget.DisplayHero(_hero);
            _rangedWidget.DisplayHero(_hero);
        }
    }
}