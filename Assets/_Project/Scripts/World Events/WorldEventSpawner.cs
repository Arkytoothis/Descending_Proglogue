using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Game_Events
{
    public class WorldEventSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _model = null;

        [SerializeField] private bool _isOccupied = false;

        public bool IsOccupied
        {
            get => _isOccupied;
            set => _isOccupied = value;
        }

        public void Show()
        {
            _model.SetActive(true);
        }

        public void Hide()
        {
            _model.SetActive(false); 
        }
    }
}