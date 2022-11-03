using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using UnityEngine;

namespace Descending.Gui
{
    public class HeroStatisticsPanel : MonoBehaviour
    {
        [SerializeField] private StatisticWidget _attackWidget = null;
        [SerializeField] private StatisticWidget _aimWidget = null;
        [SerializeField] private StatisticWidget _focusWidget = null;
        [SerializeField] private StatisticWidget _blockWidget = null;
        [SerializeField] private StatisticWidget _dodgeWidget = null;
        [SerializeField] private StatisticWidget _willpowerWidget = null;

        public void Setup()
        {
            
        }
        
        public void DisplayHero(HeroUnit hero)
        {
            _attackWidget.SetAttribute(hero.Attributes.GetStatistic("Attack"));
            _aimWidget.SetAttribute(hero.Attributes.GetStatistic("Aim"));
            _focusWidget.SetAttribute(hero.Attributes.GetStatistic("Focus"));
            _blockWidget.SetAttribute(hero.Attributes.GetStatistic("Block"));
            _dodgeWidget.SetAttribute(hero.Attributes.GetStatistic("Dodge"));
            _willpowerWidget.SetAttribute(hero.Attributes.GetStatistic("Willpower"));
        }
    }
}