using System.Collections;
using System.Collections.Generic;
using Descending.Tiles;
using Descending.Units;
using UnityEngine;

namespace Descending.Party
{
    [System.Serializable]
    public class PartyPositionSaveData
    {
        [SerializeField] private Vector3 _worldPosition;

        public Vector3 WorldPosition => _worldPosition;

        public PartyPositionSaveData()
        {
            _worldPosition = Vector3.zero;
        }

        public PartyPositionSaveData(Vector3 worldPosition)
        {
            _worldPosition = worldPosition;
        }
    }
}