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
    public class VillagePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameLabel = null;
        [SerializeField] private Button _interactButton = null;
        [SerializeField] private Village _village = null;

        private void Awake()
        {
            _nameLabel.SetText(_village.Definition.Name);
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
            _village.Interact();
        }
    }
}