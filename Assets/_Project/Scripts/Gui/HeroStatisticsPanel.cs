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
            _mightDamageWidget.SetAttribute(hero.Attributes.GetStatistic("Might Damage").Current);
            _finesseDamageWidget.SetAttribute(hero.Attributes.GetStatistic("Finesse Damage").Current);
            _magicDamageWidget.SetAttribute(hero.Attributes.GetStatistic("Magic Damage").Current);
            _blockWidget.SetAttribute(hero.Attributes.GetStatistic("Block").Current + "%");
            _dodgeWidget.SetAttribute(hero.Attributes.GetStatistic("Dodge").Current + "%");
            _willpowerWidget.SetAttribute(hero.Attributes.GetStatistic("Willpower").Current + "%");
            _perceptionWidget.SetAttribute(hero.Attributes.GetStatistic("Perception").Current + "%");
            _movementWidget.SetAttribute(hero.Attributes.GetStatistic("Movement").Current.ToString());
        }
    }
}