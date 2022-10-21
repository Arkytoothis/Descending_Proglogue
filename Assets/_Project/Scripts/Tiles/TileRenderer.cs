using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Tiles
{
    public class TileRenderer : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _borderRenderer = null;
        [SerializeField] private MeshRenderer _backgroundRenderer = null;

        public void Show(Material borderMaterial, Material backgroundMaterial)
        {
            _borderRenderer.enabled = true;
            _borderRenderer.material = borderMaterial;
            _backgroundRenderer.enabled = true;
            _backgroundRenderer.material = backgroundMaterial;
        }

        public void Hide()
        {
            _borderRenderer.enabled = false;
            _backgroundRenderer.enabled = false;
        }
    }
}
