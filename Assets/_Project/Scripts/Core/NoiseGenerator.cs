using System.Collections;
using System.Collections.Generic;
using Descending.Scene_Overworld;
using UnityEngine;

namespace Descending.Core
{
    public class NoiseGenerator : MonoBehaviour
    {
        public static float[,] GenerateNoiseMap(int seed, int sampleSize, float scale, Wave[] waves)
        {
            float[,] noiseMap = new float[sampleSize, sampleSize];

            for (int x = 0; x < sampleSize; x++)
            {
                for (int y = 0; y < sampleSize; y++)
                {
                    float sampleX = (float)x / scale;
                    float sampleY = (float)y / scale;

                    float noise = 0;
                    float normalization = 0;

                    foreach (Wave wave in waves)
                    {
                        noise += wave.Amplitude * Mathf.PerlinNoise(sampleX * wave.Frequency + seed,
                            sampleY * wave.Frequency + seed);
                        normalization += wave.Amplitude;
                    }

                    noise /= normalization;
                    noiseMap[x, y] = noise;
                }
            }

            return noiseMap;
        }

        public static float[,] GenerateUniformNoiseMap(int size, float heightOffset)
        {
            float[,] noiseMap = new float[size, size];

            for (int x = 0; x < size; x++)
            {
                float xSample = x + heightOffset;
                float noise = Mathf.Abs(xSample) / (float)size;
                
                for (int y = 0; y < size; y++)
                {
                    noiseMap[x, size - y - 1] = noise;
                }
            }

            return noiseMap;
        }

        public static float[,] GenerateFalloffMap(int size)
        {
            float[,] map = new float[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    float x = i / (float)size * 2 - 1;
                    float y = j / (float)size * 2 - 1;
                    float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                    map[i, j] = value;
                }
            }

            return map;
        }
    }
}