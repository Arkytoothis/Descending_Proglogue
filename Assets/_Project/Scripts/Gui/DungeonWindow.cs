using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Descending.Gui
{
    public class DungeonWindow : GameWindow
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

        public void EnterDungeonButtonClick()
        {
            SceneManager.LoadScene((int)GameScenes.Combat_Indoor);
        }
    }
}