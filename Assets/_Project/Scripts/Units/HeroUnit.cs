using System.Collections;
using System.Collections.Generic;
using Descending.Attributes;
using Descending.Core;
using UnityEngine;

namespace Descending.Units
{
    public class HeroUnit : Unit
    {
        [SerializeField] protected HeroData _heroData = null;
        
        [SerializeField] private GameObject _worldModel = null;
        [SerializeField] private GameObject _portraitModel = null;
        private BodyRenderer _worldRenderer = null;
        private BodyRenderer _portraitRenderer = null;
        private PortraitMount _portrait = null;
        
        public GameObject PortraitModel => _portraitModel;
        public BodyRenderer PortraitRenderer => _portraitRenderer;
        public PortraitMount Portrait => _portrait;
        public HeroData HeroData => _heroData;
        public GameObject WorldModel => _worldModel;
        public BodyRenderer WorldRenderer => _worldRenderer;

        public void SetupHero(Genders gender, RaceDefinition race, ProfessionDefinition profession, int listIndex)
        {
            _isEnemy = false;
            _modelParent.ClearTransform();
            _worldModel = Instantiate(race.PrefabMale, _modelParent);
            _worldRenderer = _worldModel.GetComponent<BodyRenderer>();
            _worldRenderer.SetupBody(gender, race, profession);
            
            _portraitModel = Instantiate(race.PrefabMale, null);
            _portraitRenderer = _portraitModel.GetComponent<BodyRenderer>();
            _portraitRenderer.SetupBody(_worldRenderer, race, profession);
            
            _unitAnimator = GetComponent<UnitAnimator>();
            _unitAnimator.Setup(_worldModel.GetComponent<Animator>());
            
            _heroData.Setup(gender, race, profession, _worldRenderer, listIndex);
            _attributes.Setup(race, profession);
            _skills.Setup(_attributes, race, profession);
            _inventory.Setup(_portraitRenderer, _worldRenderer, gender, race, profession);
            _abilities.Setup(race, profession, _skills);
            
            _healthSystem.Setup(_attributes.GetVital("Life").Maximum);
                
            _worldPanel.Setup(this);

            var children = _portraitModel.GetComponentsInChildren<Transform>(includeInactive: true);
            foreach (var child in children)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Portrait Light");
            }
            
        }

        public override string GetFullName()
        {
            return _heroData.Name.FullName;
        }

        public override string GetShortName()
        {
            return _heroData.Name.ShortName;
        }

        
        public void SetPortrait(PortraitMount portraitMount)
        {
            _portrait = portraitMount;
        }
    }
}