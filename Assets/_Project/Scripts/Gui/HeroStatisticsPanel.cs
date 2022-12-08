using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using UnityEngine;
using UnityEngine.Serialization;

namespace Descending.Gui
{
    public class HeroStatisticsPanel : MonoBehaviour
    {
        [SerializeField] private StatisticWidget _aimWidget = null;
        [SerializeField] private StatisticWidget _attackWidget = null;
        [SerializeField] private StatisticWidget _focusWidget = null;
        [SerializeField] private StatisticWidget _mightModifierWidget = null;
        [SerializeField] private StatisticWidget _finesseModifierWidget = null;
        [SerializeField] private StatisticWidget _magicModifierWidget = null;
        [SerializeField] private StatisticWidget _blockWidget = null;
        [SerializeField] private StatisticWidget _dodgeWidget = null;
        [SerializeField] private StatisticWidget _willpowerWidget = null;
        [SerializeField] private StatisticWidget _movementWidget = null;
        [SerializeField] private StatisticWidget _perceptionWidget = null;
        [SerializeField] private StatisticWidget _criticalHitWidget = null;
        [SerializeField] private StatisticWidget _criticalDamageWidget = null;
        [SerializeField] private StatisticWidget _fumbleWidget = null;

        public void Setup()
        {
            
        }
        
        public void DisplayHero(HeroUnit hero)
        {
            _mightModifierWidget.SetAttribute(hero.Attributes.GetStatistic("Might Modifier"));
            _finesseModifierWidget.SetAttribute(hero.Attributes.GetStatistic("Finesse Modifier"));
            _magicModifierWidget.SetAttribute(hero.Attributes.GetStatistic("Magic Modifier"));
            _blockWidget.SetAttributePercent(hero.Attributes.GetStatistic("Block"));
            _dodgeWidget.SetAttributePercent(hero.Attributes.GetStatistic("Dodge"));
            _willpowerWidget.SetAttributePercent(hero.Attributes.GetStatistic("Willpower"));
            _perceptionWidget.SetAttributePercent(hero.Attributes.GetStatistic("Perception"));
            _movementWidget.SetAttribute(hero.Attributes.GetStatistic("Movement"));
        }
    }
}