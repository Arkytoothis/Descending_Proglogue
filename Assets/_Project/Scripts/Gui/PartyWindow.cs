using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using UnityEngine;

namespace Descending.Gui
{
    public class PartyWindow : GameWindow
    {
        public override void Setup(WindowManager manager)
        {
            _manager = manager;
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

        public void OnSyncParty(bool b)
        {
            for (int i = 0; i < UnitManager.Instance.PlayerUnits.Count; i++)
            {
                
            }
        }
    }
}