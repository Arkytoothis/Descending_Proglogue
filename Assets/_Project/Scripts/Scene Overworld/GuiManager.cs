using System.Collections;
using System.Collections.Generic;
using Descending.Gui;
using UnityEngine;

namespace Descending.Overworld
{
    public class GuiManager : MonoBehaviour
    {
        [SerializeField] private PartyPanel_Overworld  _partyPanel = null;
        [SerializeField] private WindowManager _windowManager = null;
        [SerializeField] private GameObject _tooltipPrefab = null;

        private Tooltip _tooltip = null;
        
        public void Setup()
        {
            _windowManager.Setup();
            _partyPanel.Setup();
            SpawnTooltip();
        }

        private void SpawnTooltip()
        {
            GameObject clone = Instantiate(_tooltipPrefab, null);
            _tooltip = clone.GetComponentInChildren<Tooltip>();
            _tooltip.Setup();
        }
    }
}