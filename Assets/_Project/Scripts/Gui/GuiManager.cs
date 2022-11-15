using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Gui
{
    public class GuiManager : MonoBehaviour
    {
        [SerializeField] private WindowManager _windowManager = null;
        [SerializeField] private ActionButtonsPanel _actionButtonsPanel = null;
        [SerializeField] private TurnSystemPanel _turnSystemPanel = null;
        [SerializeField] private PartyPanel _partyPanel = null;
        [SerializeField] private TilePanel _tilePanel = null;
        [SerializeField] private GameObject _tooltipPrefab = null;
        [SerializeField] private Transform _guiParent = null;

        private Tooltip _tooltip = null;
        
        public void Setup()
        {
            _windowManager.Setup();
            _actionButtonsPanel.Setup();
            _turnSystemPanel.Setup();
            _partyPanel.Setup();
            _tilePanel.Setup();
            SpawnTooltip();
        }

        private void SpawnTooltip()
        {
            GameObject clone = Instantiate(_tooltipPrefab, _guiParent);
            clone.transform.SetAsLastSibling();
            
            _tooltip = clone.GetComponentInChildren<Tooltip>();
            _tooltip.Setup();
        }
    }
}