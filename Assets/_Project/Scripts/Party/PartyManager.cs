using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Attributes;
using Descending.Core;
using Descending.Units;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Party
{
    public class PartyManager : MonoBehaviour
    {
        public static PartyManager Instance { get; private set; }

        [SerializeField] private GameObject _heroPrefab = null;
        [SerializeField] private Transform _heroesParent = null;
        [SerializeField] private PartyController _partyController = null;
        [SerializeField] private float _scaleFactor = 15f;
        [SerializeField] private List<HeroUnit> _heroes = null;

        [SerializeField] private BoolEvent onSyncParty = null;
        [SerializeField] private GameObjectEvent onSelectHero = null;
        
        public List<HeroUnit> Heroes => _heroes;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple Party Managers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;

            _heroes = new List<HeroUnit>();
        }

        public void Setup()
        {
            SpawnHeroes();  
            _partyController.SetPartyLeader(0);  
        }
        
        public void SpawnHeroes()
        {
            _heroes = new List<HeroUnit>();
            SpawnHero(0, Database.instance.Races.GetRace("Half Orc"), Database.instance.Profession.GetProfession("Soldier"));
            SpawnHero(1, Database.instance.Races.GetRace("Wild Elf"), Database.instance.Profession.GetProfession("Scout"));
            SpawnHero(2, Database.instance.Races.GetRace("Valarian"), Database.instance.Profession.GetProfession("Apprentice"));
            
            PortraitRoom.Instance.Setup();
            onSyncParty.Invoke(true);
        }
        
        private void SpawnHero(int listIndex, RaceDefinition race, ProfessionDefinition profession)
        {
            //Debug.Log("Spawning Hero at " + mapPosition.ToString());
            GameObject clone = Instantiate(_heroPrefab, _heroesParent);
            
            HeroUnit heroUnit = clone.GetComponent<HeroUnit>();
            heroUnit.SetupHero(Genders.Male, race, profession, listIndex);
            heroUnit.WorldModel.transform.localScale = new Vector3(_scaleFactor, _scaleFactor, _scaleFactor);
            clone.name = "Hero: " + heroUnit.GetFullName();
            
            _heroes.Add(heroUnit);
        }

        public void HideHeroes(Transform parent)
        {
            for (int i = 0; i < _heroes.Count; i++)
            {
                _heroes[i].WorldModel.transform.SetParent(parent, false);
            }
        }

        public void SetPartyLeader(GameObject heroObject)
        {
            HeroUnit hero = heroObject.GetComponent<HeroUnit>();
            _partyController.SetPartyLeader(hero.HeroData.ListIndex);
            onSelectHero.Invoke(heroObject);
        }
    }
}