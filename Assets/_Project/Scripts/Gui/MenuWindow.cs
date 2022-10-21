using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Gui
{
    public class MenuWindow : GameWindow
    {
        public override void Setup()
        {
            Close();
        }

        public override void Open()
        {
            _container.SetActive(true);
            _isOpen = true;
        }

        public override void Close()
        {
            _container.SetActive(false);
            _isOpen = false;
        }
    }
}