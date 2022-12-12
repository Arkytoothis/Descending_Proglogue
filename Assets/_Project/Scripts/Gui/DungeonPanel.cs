using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Features;
using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Descending.Gui
{
    public class DungeonPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameLabel = null;
        [SerializeField] private Button _interactButton = null;
        [SerializeField] private Dungeon _dungeon = null;

        private void Awake()
        {
            _nameLabel.SetText(_dungeon.Definition.Name);
            HideInteractButton();
        }

        public void ShowInteractButton()
        {
            _interactButton.gameObject.SetActive(true);
        }

        public void HideInteractButton()
        {
            _interactButton.gameObject.SetActive(false);
        }

        public void InteractButton_Click()
        {
            Debug.Log("Interacting with " + _dungeon.Definition.Name);
            _dungeon.Interact();
        }
    }
}