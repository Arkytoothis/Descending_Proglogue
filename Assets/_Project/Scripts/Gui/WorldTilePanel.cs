using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Features;
using Descending.Scene_Overworld;
using Descending.Tiles;
using Descending.Units;
using TMPro;
using UnityEngine;

namespace Descending.Gui
{
    public class WorldTilePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameLabel = null;
        [SerializeField] private TMP_Text _positionLabel = null;
        [SerializeField] private TMP_Text _threatLevelLabel = null;
        [SerializeField] private TMP_Text _featureLabel = null;

        public void Setup()
        {
        }
        
        public void OnSetTile(GameObject worldTIleObject)
        {
            _positionLabel.SetText("Map Position   X: -1  Y: -1");
            WorldTile tile = worldTIleObject.GetComponent<WorldTile>();
            
            if (tile == null) return;
            
            _nameLabel.SetText("WorldTIle - " + tile.Name);
            _positionLabel.SetText("X: " + tile.X + "   Y: " + tile.Y);
            _threatLevelLabel.SetText("Threat Level: " + tile.ThreatLevel);
            
            if (tile.Feature != null)
            {
                _featureLabel.SetText("Feature: " + tile.Feature.name);
            }
            else
            {
                _featureLabel.SetText("Feature: none");
            }
        }
    }
}