using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Descending.Gui
{
    public class ResourcesPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinsLabel = null;
        [SerializeField] private TMP_Text _suppliesLabel = null;
        [SerializeField] private TMP_Text _materialsLabel = null;
        [SerializeField] private TMP_Text _gemsLabel = null;

        public void Setup()
        {
        }

        public void UpdateCoins(int coins)
        {
            _coinsLabel.SetText("Coins: " + coins);
        }

        public void UpdateSupplies(int supplies)
        {
            _suppliesLabel.SetText("Supplies: " + supplies);
        }

        public void UpdateMaterials(int materials)
        {
            _materialsLabel.SetText("Materials: " + materials);
        }

        public void UpdateGems(int gems)
        {
            _gemsLabel.SetText("Gems: " + gems);
        }
    }
}