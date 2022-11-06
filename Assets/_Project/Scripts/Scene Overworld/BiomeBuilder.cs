using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Scene_Overworld
{
    public class BiomeBuilder : MonoBehaviour
    {
        public BiomeRow[] BiomeRows;

        public Texture2D BuildTexture(TerrainType[,] heatMapTypes, TerrainType[,] moistureTypes)
        {
            int size = heatMapTypes.GetLength(0);
            Color[] pixels = new Color[size * size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    int index = (x * size) + y;
                    int heatMapIndex = heatMapTypes[x, y].Index;
                    int moistureMapIndex = moistureTypes[x, y].Index;

                    Biome biome = BiomeRows[moistureMapIndex].Biomes[heatMapIndex];
                    pixels[index] = biome.Color;
                }
            }

            Texture2D texture = new Texture2D(size, size);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;
            texture.SetPixels(pixels);
            texture.Apply();
            
            return texture;
        }

        public Biome GetBiome(TerrainType heatTerrain, TerrainType moistureTerrain)
        {
            Biome biome = BiomeRows[moistureTerrain.Index].Biomes[heatTerrain.Index];
            return biome;
        }
    }

    [System.Serializable]
    public class BiomeRow
    {
        public Biome[] Biomes;
    }
}