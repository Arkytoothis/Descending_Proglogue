using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using UnityEngine;

namespace Descending.Gui
{
    public class PartyWindow : GameWindow
    {
        [SerializeField] private HeroListPanel _listPanel = null;
        [SerializeField] private HeroDetailsPanel _detailsPanel = null;
        [SerializeField] private HeroCharacteristicsPanel _characteristicsPanel = null;
        [SerializeField] private HeroVitalsPanel _vitalsPanel = null;
        [SerializeField] private HeroStatisticsPanel _statisticsPanel = null;
        
        public override void Setup(WindowManager manager)
        {
            _manager = manager;
            _listPanel.Setup(this);
            _detailsPanel.Setup();
            _characteristicsPanel.Setup();
            _vitalsPanel.Setup();
            _statisticsPanel.Setup();
            
            //SelectHero(UnitManager.Instance.GetHero(0));
            Close();
        }

        public override void Open()
        {
            gameObject.SetActive(true);
            _isOpen = true;
        }

        public override void Close()
        {
            gameObject.SetActive(false);
            _isOpen = false;
        }

        public void SelectHero(HeroUnit hero)
        {
            _detailsPanel.DisplayHero(hero);
            _characteristicsPanel.DisplayHero(hero);
            _vitalsPanel.DisplayHero(hero);
            _statisticsPanel.DisplayHero(hero);
        }
    }
}