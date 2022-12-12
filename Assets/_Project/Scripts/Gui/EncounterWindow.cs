using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Encounters;
using Descending.Features;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Descending.Gui
{
    public class EncounterWindow : GameWindow
    {
        [SerializeField] private TMP_Text _nameLabel = null;
        [SerializeField] private TMP_Text _threatLabel = null;
        [SerializeField] private TMP_Text _difficultyLabel = null;
        [SerializeField] private TMP_Text _groupLabel = null;
        
        private Encounter _encounter = null;
        
        public override void Setup(WindowManager manager)
        {
            _manager = manager;
            
            Close();
        }

        public override void Open()
        {
            gameObject.SetActive(true);
            _isOpen = true;
            
            _nameLabel.SetText("Encounter");
            _threatLabel.SetText("Threat Level: ");
            _difficultyLabel.SetText("Difficulty: ");
            _groupLabel.SetText("Enemy Group: ");
        }

        public override void Close()
        {
            gameObject.SetActive(false);
            _isOpen = false;
        }

        public void StartEncounterButtonClick()
        {
            SceneManager.LoadScene((int)GameScenes.Combat_Outdoor);
        }

        public void SetEncounter(Encounter encounter)
        {
            _encounter = encounter;
        }
    }
}