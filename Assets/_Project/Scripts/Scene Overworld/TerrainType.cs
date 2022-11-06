using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

namespace Descending.Scene_Overworld
{
    [System.Serializable]
    public class TerrainType
    {
        public int Index;
        [Range(0.0f, 1.0f)] 
        public float Threshold;
        public Gradient ColorGradient;
    }
}