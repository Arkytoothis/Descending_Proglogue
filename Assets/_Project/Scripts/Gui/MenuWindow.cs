using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Gui
{
    public class MenuWindow : GameWindow
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

        public void ResumeButton_OnClick()
        {
            Debug.Log("Resuming");
            _manager.CLoseAll();
        }

        public void SaveButton_OnClick()
        {
            Debug.Log("Saving");
            _manager.CLoseAll();
        }

        public void LoadButton_OnClick()
        {
            Debug.Log("Loading");
            _manager.CLoseAll();
        }

        public void OptionsButton_OnClick()
        {
            Debug.Log("Options");
            _manager.CLoseAll();
        }

        public void ExitToMenuButton_OnClick()
        {
            Debug.Log("Exiting To Menu");
            _manager.CLoseAll();
        }

        public void ExitButton_OnClick()
        {
            Debug.Log("Exiting");
            _manager.CLoseAll();
        }
    }
}