using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Gui
{
    public class WindowManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _gameWindowPrefabs = null;
        [SerializeField] private Transform _gameWindowsParent = null;

        [SerializeField] private BoolEvent onSetCameraControlsActive = null;
        
        private List<GameWindow> _windows = null;

        public void Setup()
        {
            _gameWindowsParent.ClearTransform();
            _windows = new List<GameWindow>();
            
            for (int i = 0; i < _gameWindowPrefabs.Count; i++)
            {
                GameObject clone = Instantiate(_gameWindowPrefabs[i], _gameWindowsParent);
                clone.name = _gameWindowPrefabs[i].name;

                GameWindow window = clone.GetComponent<GameWindow>();
                window.Setup(this);
                
                _windows.Add(window);
            }
            
            CLoseAll();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsAnyWindowOpen())
                {
                    CLoseAll();
                }
                else
                {
                    OpenWindow((int)GameWindows.Menu);
                }
            }
            
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (IsAnyWindowOpen())
                {
                    CLoseAll();
                }
                else
                {
                    OpenWindow((int)GameWindows.Party);
                }
            }
        }

        public void OpenWindow(int windowIndex)
        {
            CLoseAll();
            _windows[windowIndex].Open();
            onSetCameraControlsActive.Invoke(false);
        }

        public void CloseWindow(int windowIndex)
        {
            _windows[windowIndex].Close();
        }

        public void CLoseAll()
        {
            for (int i = 0; i < _windows.Count; i++)
            {
                CloseWindow(i);
            }
            
            onSetCameraControlsActive.Invoke(true);
        }
        
        public bool IsWindowOpen(int windowIndex)
        {
            return _windows[windowIndex].IsOpen;
        }

        public bool IsAnyWindowOpen()
        {
            for (int i = 0; i < _windows.Count; i++)
            {
                if (_windows[i].IsOpen == true)
                {
                    return true;
                }
            }

            return false;
        }

        public void OnToggleVillageWindow(bool b)
        {
            OpenWindow((int)GameWindows.Village);
        }

        public void OnToggleDungeonWindow(bool b)
        {
            OpenWindow((int)GameWindows.Dungeon);
        }
    }
}
