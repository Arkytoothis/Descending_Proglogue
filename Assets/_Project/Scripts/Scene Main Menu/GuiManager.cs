using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Gui;
using UnityEngine;

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
            PartyBuilder.Instance.SpawnHero(0, Database.instance.Races.GetRandomRace(), Database.instance.Profession.GetRandomProfession());
        }

        public void SaveButtonClick()
        {
            SaveManager.Instance.SaveState();
        }

        public void LoadButtonClick()
        {
            SaveManager.Instance.LoadState();
        }
    }
}