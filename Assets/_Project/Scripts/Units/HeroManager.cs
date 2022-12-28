using System.Collections;
using System.Collections.Generic;
using Descending.Attributes;
using Descending.Tiles;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Units
{
    public abstract class HeroManager : MonoBehaviour
    {
        [SerializeField] protected GameObject _heroPrefab = null;
        [SerializeField] protected Transform _heroesParent = null;
        [SerializeField] protected List<HeroUnit> _heroUnits = null;
        [SerializeField] protected bool _loadData = false;
        

        [SerializeField] protected BoolEvent onSyncParty = null;
        [SerializeField] protected GameObjectEvent onSelectHero = null;
        
        protected PlayerSpawner _playerSpawner = null;
        protected HeroUnit _selectedHero = null;
        
        public List<HeroUnit> HeroUnits => _heroUnits;
        public HeroUnit SelectedHero => _selectedHero;
        
        public abstract void Setup();
        public abstract void SyncHeroes();
        public abstract void UnitSpawned(Unit unit);
        public abstract void UnitDead(Unit unit);
        public abstract HeroUnit GetHero(int index);
        public abstract void SelectHeroDefault();
        public abstract void RefreshSelectedHero();
        public abstract void SelectHero(HeroUnit hero);
        public abstract void AwardExperience(int experience);
        public abstract void RecalculateHeroAttributes();
        public abstract void SaveState();
        public abstract void LoadState();
        public abstract void GenerateHeroes();
        protected abstract void SpawnHero(MapPosition mapPosition, int listIndex, RaceDefinition race, ProfessionDefinition profession);
        protected abstract void LoadHero(MapPosition mapPosition, HeroSaveData saveData);
        public abstract void SetLoadData(bool loadData);
        public abstract void SpawnHeroes();
    }
}