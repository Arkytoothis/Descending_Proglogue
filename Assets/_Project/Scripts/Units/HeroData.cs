using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Attributes;
using Descending.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Descending.Units
{
    public class HeroData : UnitData
    {
        [SerializeField] private FantasyName _name = new FantasyName();
        [SerializeField] private Genders _gender = Genders.None;
        [SerializeField] private string _raceKey = "";
        [SerializeField] private string _professionKey = "";
        [SerializeField] private int _level = 0;
        [SerializeField] private int _experience = 0;
        [SerializeField] private int _expToNextLevel = 0;
        
        [SerializeField] private int _listIndex = -1;
        [SerializeField] private int _headIndex = -1;
        [SerializeField] private int _hairIndex = -1;
        [SerializeField] private int _earIndex = -1;
        [SerializeField] private int _facialHairIndex = -1;
        [SerializeField] private int _eyebrowIndex = -1;
        [SerializeField] private int _skinColorIndex = -1;
        [SerializeField] private int _hairColorIndex = -1;
        [SerializeField] private int _eyeColorIndex = -1;
        
        public FantasyName Name => _name;
        public Genders Gender => _gender;
        public string RaceKey => _raceKey;
        public string ProfessionKey => _professionKey;
        public int Level => _level;
        public int Experience => _experience;
        public int ExpToNextLevel => _expToNextLevel;

        public int ListIndex => _listIndex;
        public int HeadIndex => _headIndex;
        public int HairIndex => _hairIndex;
        public int EarIndex => _earIndex;
        public int FacialHairIndex => _facialHairIndex;
        public int EyebrowIndex => _eyebrowIndex;
        public int SkinColorIndex => _skinColorIndex;
        public int HairColorIndex => _hairColorIndex;
        public int EyeColorIndex => _eyeColorIndex;
        
        public RaceDefinition RaceDefinition => Database.instance.Races.GetRace(_raceKey);

        public void Setup(Genders gender, RaceDefinition race, ProfessionDefinition profession, BodyRenderer bodyRenderer, int listIndex)
        {
            _name = NameGenerator.Get(gender, race.Key, profession.Key);
            _gender = gender;
            _raceKey = race.Key;
            _professionKey = profession.Key;
            _level = 1;
            _experience = 0;
            _expToNextLevel = 1000;

            _listIndex = listIndex;
            _headIndex = bodyRenderer.HeadIndex;
            _hairIndex = bodyRenderer.HairIndex;
            _earIndex = bodyRenderer.EarIndex;
            _facialHairIndex = bodyRenderer.FacialHairIndex;
            _eyebrowIndex = bodyRenderer.EyebrowIndex;
            _skinColorIndex = bodyRenderer.SkinColorIndex;
            _hairColorIndex = bodyRenderer.HairColorIndex;
            _eyeColorIndex = bodyRenderer.EyeColorIndex;
        }

        public void LoadData(HeroDataSaveData saveData, BodyRenderer bodyRenderer)
        {
            _name = new FantasyName(saveData.Name);
            _gender = saveData.Gender;
            _raceKey = saveData.RaceKey;
            _professionKey = saveData.ProfessionKey;
            _level = saveData.Level;
            _experience = saveData.Experience;
            _expToNextLevel = saveData.ExpToNextLevel;

            _listIndex = saveData.ListIndex;
            _headIndex = bodyRenderer.HeadIndex;
            _hairIndex = bodyRenderer.HairIndex;
            _earIndex = bodyRenderer.EarIndex;
            _facialHairIndex = bodyRenderer.FacialHairIndex;
            _eyebrowIndex = bodyRenderer.EyebrowIndex;
            _skinColorIndex = bodyRenderer.SkinColorIndex;
            _hairColorIndex = bodyRenderer.HairColorIndex;
            _eyeColorIndex = bodyRenderer.EyeColorIndex;
        }

        public void AddExperience(int experience)
        {
            //Debug.Log(_name.ShortName + " gained " + experience + " experience");
            _experience += experience;
        }

        public override string GetName()
        {
            return _name.FullName;
        }
    }

    [System.Serializable]
    public class HeroDataSaveData
    {
        [SerializeField] private FantasyName _name = new FantasyName();
        [SerializeField] private Genders _gender = Genders.None;
        [SerializeField] private string _raceKey = "";
        [SerializeField] private string _professionKey = "";
        [SerializeField] private int _level = 0;
        [SerializeField] private int _experience = 0;
        [SerializeField] private int _expToNextLevel = 0;
        
        [SerializeField] private int _listIndex = -1;
        [SerializeField] private int _headIndex = -1;
        [SerializeField] private int _hairIndex = -1;
        [SerializeField] private int _earIndex = -1;
        [SerializeField] private int _facialHairIndex = -1;
        [SerializeField] private int _eyebrowIndex = -1;
        [SerializeField] private int _skinColorIndex = -1;
        [SerializeField] private int _tattooColorIndex = -1;
        [SerializeField] private int _hairColorIndex = -1;
        [SerializeField] private int _eyeColorIndex = -1;
        
        public FantasyName Name => _name;
        public Genders Gender => _gender;
        public string RaceKey => _raceKey;
        public string ProfessionKey => _professionKey;
        public int Level => _level;
        public int Experience => _experience;
        public int ExpToNextLevel => _expToNextLevel;
        public int ListIndex => _listIndex;
        public int HeadIndex => _headIndex;
        public int HairIndex => _hairIndex;
        public int EarIndex => _earIndex;
        public int FacialHairIndex => _facialHairIndex;
        public int EyebrowIndex => _eyebrowIndex;
        public int SkinColorIndex => _skinColorIndex;
        public int HairColorIndex => _hairColorIndex;
        public int EyeColorIndex => _eyeColorIndex;
        public int TattooColorIndex => _tattooColorIndex;

        // public HeroDataSaveData(Hero hero)
        // {
        //     _name = new FantasyName(hero.HeroData.Name);
        //     _gender = hero.HeroData.Gender;
        //     _raceKey = hero.HeroData.RaceKey;
        //     _professionKey = hero.HeroData.ProfessionKey;
        //     _level = hero.HeroData.Level;
        //     _experience = hero.HeroData.Experience;
        //     _expToNextLevel = hero.HeroData.ExpToNextLevel;
        //
        //     _listIndex = hero.HeroData.ListIndex;
        //     _headIndex = hero.PortraitRenderer.HeadIndex;
        //     _hairIndex = hero.PortraitRenderer.HairIndex;
        //     _earIndex = hero.PortraitRenderer.EarIndex;
        //     _facialHairIndex = hero.PortraitRenderer.FacialHairIndex;
        //     _eyebrowIndex = hero.PortraitRenderer.EyebrowIndex;
        //     _skinColorIndex = hero.PortraitRenderer.SkinColorIndex;
        //     _hairColorIndex = hero.PortraitRenderer.HairColorIndex;
        //     _eyeColorIndex = hero.PortraitRenderer.EyeColorIndex;
        //     _tattooColorIndex = hero.PortraitRenderer.TattooColorIndex;
        // }
    }
}