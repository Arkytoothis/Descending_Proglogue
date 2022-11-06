using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Descending.Scene_Overworld;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Descending.Core
{
    public class TextureGenerator : MonoBehaviour
    {
        public static Texture2D BuildTexture(float[,] noiseMap, TerrainType[] terrainTypes)
        {
            Color[] pixels = new Color[noiseMap.Length];

            int pixelLength = noiseMap.GetLength(0);

            for (int x = 0; x < pixelLength; x++)
            {
                for (int y = 0; y < pixelLength; y++)
                {
                    int index = (x * pixelLength) + y;

                    for (int t = 0; t < terrainTypes.Length; t++)
                    {
                        if (noiseMap[x, y] < terrainTypes[t].Threshold)
                        {
                            float minValue = t == 0 ? 0 : terrainTypes[t - 1].Threshold;
                            float maxValue = terrainTypes[t].Threshold;
                            
                            pixels[index] = terrainTypes[t].ColorGradient.Evaluate(1f - (maxValue - noiseMap[x,y] ) / (maxValue - minValue));
                            break;
                        }
                    }
                }
            }

            Texture2D texture = new Texture2D(pixelLength, pixelLength);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;
            texture.SetPixels(pixels);
            texture.Apply();
            
            return texture;
        }

        public static TerrainType[,] CreateTerrainMap(float[,] noiseMap, TerrainType[] terrainTypes)
        {
            int size = noiseMap.GetLength(0);
            TerrainType[,] outputMap = new TerrainType[size, size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int t = 0; t < terrainTypes.Length; t++)
                    {
                        outputMap[x, y] = terrainTypes[0];
                        
                        if (noiseMap[x, y] <= terrainTypes[t].Threshold)
                        {
                            outputMap[x, y] = terrainTypes[t];
                            break;
                        }
                    }
                }
            }

            return outputMap;
        }
    }
}