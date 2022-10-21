using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Descending.Tiles
{
    public class TileDebugObject : MonoBehaviour
    {
        [SerializeField] private TMP_Text _gridLabel = null;
        
        private object _gridObject = null;

        public virtual void SetGridObject(object gridObject)
        {
            _gridObject = gridObject;
        }

        public override string ToString()
        {
            return _gridObject.ToString();
        }

        protected virtual void Update()
        {
            _gridLabel.SetText(_gridObject.ToString());
        }
    }
}
