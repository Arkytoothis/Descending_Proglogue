using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Gui
{
    public class VillageWindow : GameWindow
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
    }
}