using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Descending.Scene_Main_Menu
{
    public class GuiManager : MonoBehaviour
    {
        //[SerializeField] private WindowManager _windowManager = null;
        
        public void Setup()
        {
            //_windowManager.Setup();
        }

        public void GenerateButtonClick()
        {
            PartyBuilder.Instance.SpawnHeroes();
        }

        public void SaveButtonClick()
        {
            PartyBuilder.Instance.SaveState(Database.instance.PartyDataFilePath);
        }

        public void LoadButtonClick()
        {
            PartyBuilder.Instance.LoadState(Database.instance.PartyDataFilePath);
        }
        
        public void StartGameButton()
        {
            SceneManager.LoadScene((int)GameScenes.Overworld);
        }
        
        public void ExitGameButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        }
    }
}