using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Features;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Descending.Gui
{
    public class DungeonWindow : GameWindow
    {
        [SerializeField] private TMP_Text _nameLabel = null;
        [SerializeField] private TMP_Text _threatLabel = null;
        [SerializeField] private TMP_Text _levelsLabel = null;
        [SerializeField] private TMP_Text _difficultyLabel = null;
        [SerializeField] private TMP_Text _dungeonTypeLabel = null;
        [SerializeField] private TMP_Text _groupLabel = null;
        
        private Dungeon _dungeon = null;
        
        public override void Setup(WindowManager manager)
        {
            _manager = manager;
            
            Close();
        }

        public override void Open()
        {
            gameObject.SetActive(true);
            _isOpen = true;
            
            _nameLabel.SetText(_dungeon.Definition.Name);
            _threatLabel.SetText("Threat Level: " + _dungeon.DungeonData.ThreatLevel);
            _levelsLabel.SetText("Levels: " + _dungeon.DungeonData.Levels);
            _difficultyLabel.SetText("Difficulty: " + _dungeon.DungeonData.Difficulty);
            _dungeonTypeLabel.SetText("Dungeon Type: " + _dungeon.DungeonData.DungeonType);
            _groupLabel.SetText("Enemy Group: " + _dungeon.DungeonData.Group);
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

        public void SetDungeon(Dungeon dungeon)
        {
            _dungeon = dungeon;
        }
    }
}