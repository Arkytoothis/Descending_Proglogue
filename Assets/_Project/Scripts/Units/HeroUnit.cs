using System.Collections;
using System.Collections.Generic;
using Descending.Attributes;
using Descending.Core;
using Descending.Equipment;
using Descending.Gui;
using Descending.Tiles;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Units
{
    public class HeroUnit : Unit
    {
        [SerializeField] protected HeroData _heroData = null;
        [SerializeField] private GameObject _worldModel = null;
        [SerializeField] private GameObject _portraitModel = null;

        [SerializeField] protected BoolEvent onSyncParty = null;

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
            
            _attributes.CalculateAttributes();
            _abilities.Setup(race, profession, _skills);
            _actionController.SetupActions();
            _damageSystem.Setup(this);
            _unitEffects.Setup();
            _worldPanel.Setup(this);

            var children = _portraitModel.GetComponentsInChildren<Transform>(includeInactive: true);
            foreach (var child in children)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Portrait Light");
            }
        }

        public void LoadHero(HeroSaveData saveData)
        {
            RaceDefinition race = Database.instance.Races.GetRace(saveData.RaceKey);
            ProfessionDefinition profession = Database.instance.Profession.GetProfession(saveData.ProfessionKey);
            
            _isEnemy = false;
            _modelParent.ClearTransform();
            _worldModel = Instantiate(race.PrefabMale, _modelParent);
            _worldRenderer = _worldModel.GetComponent<BodyRenderer>();
            _worldRenderer.LoadBody(saveData);

            _portraitModel = Instantiate(race.PrefabMale, null);
            _portraitRenderer = _portraitModel.GetComponent<BodyRenderer>();
            _portraitRenderer.LoadBody(saveData);

            _unitAnimator = GetComponent<UnitAnimator>();
            _unitAnimator.Setup(_worldModel.GetComponent<Animator>());

            _heroData.LoadData(saveData, _worldRenderer);
            _attributes.Setup(race, profession);
            _skills.Setup(_attributes, race, profession);
            _inventory.Setup(_portraitRenderer, _worldRenderer, saveData.Gender, race, profession);
            
            _attributes.CalculateAttributes();
            _abilities.Setup(race, profession, _skills);
            _actionController.SetupActions();
            _damageSystem.Setup(this);
            _unitEffects.Setup();
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

        public string GetFirstName()
        {
            return _heroData.Name.FirstName;
        }

        public override Item GetMeleeWeapon()
        {
            return _inventory.GetMeleeWeapon();
        }

        public override Item GetRangedWeapon()
        {
            return _inventory.GetRangedWeapon();
        }

        public override void Damage(GameObject attacker, DamageTypeDefinition damageType, int damage, string vital)
        {
            if (_isAlive == false) return;

            _damageSystem.TakeDamage(attacker, damage, vital);
            CombatTextHandler.Instance.DisplayCombatText(new CombatText(_combatTextTransform.position, damage.ToString(), "default"));

            if (GetHealth() <= 0)
            {
                Dead();
            }

            onSyncParty.Invoke(true);
        }

        public override void RestoreVital(string vital, int amount)
        {
            if (_isAlive == false) return;

            _damageSystem.RestoreVital(vital, amount);
            onSyncParty.Invoke(true);
        }

        public override void UseResource(string vital, int amount)
        {
            if (_isAlive == false) return;

            _damageSystem.UseResource(vital, amount);
            onSyncParty.Invoke(true);
        }

        protected override void Dead()
        {
            _isAlive = false;
            MapManager.Instance.RemoveUnitAtGridPosition(currentMapPosition, this);
            UnitManager.Instance.UnitDead(this);
            Destroy(gameObject);
        }

        public void SetPortrait(PortraitMount portraitMount)
        {
            _portrait = portraitMount;
        }

        public void AddExperience(int experience)
        {
            _heroData.AddExperience(experience);
            onSyncParty.Invoke(true);
        }

        public override void SpendActionPoints(int actionPointCost)
        {
            _attributes.ModifyVital("Actions", actionPointCost, true);
            _worldPanel.UpdateActionPoints();
            onSyncParty.Invoke(true);
        }
    }
    
}