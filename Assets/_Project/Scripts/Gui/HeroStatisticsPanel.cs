using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using UnityEngine;
using UnityEngine.Serialization;

namespace Descending.Gui
{
    public class HeroStatisticsPanel : MonoBehaviour
    {
        [SerializeField] private StatisticWidget _mightDamageWidget = null;
        [SerializeField] private StatisticWidget _finesseDamageWidget = null;
        [SerializeField] private StatisticWidget _magicDamageWidget = null;
        [SerializeField] private StatisticWidget _blockWidget = null;
        [SerializeField] private StatisticWidget _dodgeWidget = null;
        [SerializeField] private StatisticWidget _willpowerWidget = null;
        [SerializeField] private StatisticWidget _movementWidget = null;
        [SerializeField] private StatisticWidget _perceptionWidget = null;

        public void Setup()
        {
            
        }
        
        public void DisplayHero(HeroUnit hero)
        {
            _mightDamageWidget.SetAttribute(hero.Attributes.GetStatistic("Might Damage"));
            _finesseDamageWidget.SetAttribute(hero.Attributes.GetStatistic("Finesse Damage"));
            _magicDamageWidget.SetAttribute(hero.Attributes.GetStatistic("Magic Damage"));
            _blockWidget.SetAttributePercent(hero.Attributes.GetStatistic("Block"));
            _dodgeWidget.SetAttributePercent(hero.Attributes.GetStatistic("Dodge"));
            _willpowerWidget.SetAttributePercent(hero.Attributes.GetStatistic("Willpower"));
            _perceptionWidget.SetAttributePercent(hero.Attributes.GetStatistic("Perception"));
            _movementWidget.SetAttribute(hero.Attributes.GetStatistic("Movement"));
        }
    }
}