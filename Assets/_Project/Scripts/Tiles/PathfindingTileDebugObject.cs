using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Descending.Tiles
{
    public class PathfindingTileDebugObject : TileDebugObject
    {
        [SerializeField] private TextMeshPro _fCostLabel;
        [SerializeField] private TextMeshPro _hCostLabel;
        [SerializeField] private TextMeshPro _gCostLabel;
        [SerializeField] private SpriteRenderer _isWalkableRenderer;
        [SerializeField] private Color _walkableColor;
        [SerializeField] private Color _unwalkableColor;

        private PathNode _pathNode;
        
        public override void SetGridObject(object gridObject)
        {
            base.SetGridObject(gridObject);
            _pathNode = (PathNode) gridObject;
        }

        protected override void Update()
        {
            base.Update();
            _fCostLabel.SetText(_pathNode.FCost.ToString());
            _hCostLabel.SetText(_pathNode.HCost.ToString());
            _gCostLabel.SetText(_pathNode.GCost.ToString());
            _isWalkableRenderer.color = _pathNode.IsWalkable ? _walkableColor : _unwalkableColor;
        }
    }
}