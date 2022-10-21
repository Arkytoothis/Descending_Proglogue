using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Gui
{
    public class GuiManager : MonoBehaviour
    {
        [SerializeField] private ActionButtonsPanel _actionButtonsPanel = null;
        [SerializeField] private TurnSystemPanel _turnSystemPanel = null;
        
        public void Setup()
        {
            _actionButtonsPanel.Setup();
            _turnSystemPanel.Setup();
        }
    }
}