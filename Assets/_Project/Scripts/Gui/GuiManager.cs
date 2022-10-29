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
        
        public void Setup()
        {
            _windowManager.Setup();
            _actionButtonsPanel.Setup();
            _turnSystemPanel.Setup();
            _partyPanel.Setup();
            _tilePanel.Setup();
        }
    }
}