using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using Descending.Units;
using TMPro;
using UnityEngine;

namespace Descending.Gui
{
    public class TilePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _mapPositionLabel = null;
        [SerializeField] private TMP_Text _unitLabel = null;
        [SerializeField] private TMP_Text _treasureLabel = null;
        [SerializeField] private TMP_Text _trapLabel = null;
        [SerializeField] private TMP_Text _interactableLabel = null;

        public void Setup()
        {
        }
        
        public void OnSetTile(MapPosition mapPosition)
        {
            Tile tile = MapManager.Instance.GetTile(mapPosition);
            if (tile == null) return;
            
            _mapPositionLabel.SetText("Map Position   X: " + tile.MapPosition.X + "   Y: " + tile.MapPosition.Y);

            Unit unit = tile.GetUnit();
            
            if (tile.GetUnit() != null)
            {
                _unitLabel.SetText("Unit: " + unit.GetShortName());
            }
            else
            {
                _unitLabel.SetText("Unit: none");
            }

            int coinValue = tile.GetCoinValue();
            int gemValue = tile.GetGemValue();
            string treasureText = "Treasure: ";

            if (coinValue > 0)
            {
                treasureText += coinValue + " coins ";
                if (gemValue > 0) treasureText += ", ";
            }
            if (gemValue > 0)
            {
                treasureText += gemValue + " gems";
            }
            if (coinValue == 0 && gemValue == 0)
            {
                treasureText = "Treasure: none"; 
            }
            
            _treasureLabel.SetText(treasureText);

            if (tile.Interactable != null)
            {
                _interactableLabel.SetText("Interactable: " + tile.Interactable.GetName());
            }
            else if (tile.Damageable != null)
            {
                _interactableLabel.SetText("Damageable: " + tile.Damageable.GetName());
            }
            else
            {
                _interactableLabel.SetText("Interactable: none");
            }
            
            _trapLabel.SetText("Trap: none");
        }
    }
}