using System.Collections;
using System.Collections.Generic;
using Descending.Features;
using UnityEngine;

namespace Descending.Gui
{
    public class VillageWindow : GameWindow
    {
        [SerializeField] private GameObject _marketPanelPrefab = null;
        [SerializeField] private GameObject _tavernPanelPrefab = null;
        [SerializeField] private GameObject _guildHallPanelPrefab = null;
        [SerializeField] private GameObject _wizardTowerPanelPrefab = null;
        [SerializeField] private Transform _panelsParent = null;
        
        private MarketPanel _marketPanel = null;
        private TavernPanel _tavernPanel = null;
        private GuildHallPanel _guildHallPanel = null;
        private WizardTowerPanel _wizardTowerPanel = null;
        
        public override void Setup(WindowManager manager)
        {
            _manager = manager;
            SpawnMarketPanel();
            SpawnTavernPanel();
            SpawnGuildHallPanel();
            SpawnWizardTowerPanel();
            Close();
        }

        public override void Open()
        {
            gameObject.SetActive(true);
            MarketButtonClick();
            _isOpen = true;
        }

        public override void Close()
        {
            gameObject.SetActive(false);
            _isOpen = false;
        }

        public void MarketButtonClick()
        {
            _marketPanel.Show();
            _tavernPanel.Hide();
            _guildHallPanel.Hide();
            _wizardTowerPanel.Hide();
        }

        public void TavernButtonClick()
        {
            _marketPanel.Hide();
            _tavernPanel.Show();
            _guildHallPanel.Hide();
            _wizardTowerPanel.Hide();
        }

        public void GuildHallButtonClick()
        {
            _marketPanel.Hide();
            _tavernPanel.Hide();
            _guildHallPanel.Show();
            _wizardTowerPanel.Hide();
        }

        public void WizardTowerButtonClick()
        {
            _marketPanel.Hide();
            _tavernPanel.Hide();
            _guildHallPanel.Hide();
            _wizardTowerPanel.Show();
        }

        private void SpawnMarketPanel()
        {
            GameObject clone = Instantiate(_marketPanelPrefab, _panelsParent);
            _marketPanel = clone.GetComponent<MarketPanel>();
            _marketPanel.Setup();
        }

        private void SpawnTavernPanel()
        {
            GameObject clone = Instantiate(_tavernPanelPrefab, _panelsParent);
            _tavernPanel = clone.GetComponent<TavernPanel>();
            _tavernPanel.Setup();
        }

        private void SpawnGuildHallPanel()
        {
            GameObject clone = Instantiate(_guildHallPanelPrefab, _panelsParent);
            _guildHallPanel = clone.GetComponent<GuildHallPanel>();
            _guildHallPanel.Setup();
        }

        private void SpawnWizardTowerPanel()
        {
            GameObject clone = Instantiate(_wizardTowerPanelPrefab, _panelsParent);
            _wizardTowerPanel = clone.GetComponent<WizardTowerPanel>();
            _wizardTowerPanel.Setup();
        }

        public void SetVillage(Village village)
        {
            _marketPanel.DisplayVillage(village);
        }
    }
}