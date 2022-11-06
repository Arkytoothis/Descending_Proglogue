using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Descending.Scene_Overworld
{
    public class WorldGenerator : MonoBehaviour
    {
        [SerializeField] private int _sampleSize = 10;
        [SerializeField] private int _seed = 0;
        [SerializeField] private bool _randomizeSeed = true;
        [SerializeField] private bool _displayTextures = true;
        [SerializeField] private float _scale = 1.1f;
        [SerializeField] private float _heatScale = 4f;
        [SerializeField] private float _moistureScale = 4f;
        [SerializeField] private float _tileOffsetX = 5f;
        [SerializeField] private float _tileOffsetY = 4.35f;
        [SerializeField] private float _tileHeightModifier = 2f;
        [SerializeField] private RawImage _heightMapImage = null;
        [SerializeField] private RawImage _heatMapImage = null;
        [SerializeField] private RawImage _moistureMapImage = null;
        [SerializeField] private RawImage _falloffMapImage = null;
        [SerializeField] private RawImage _biomeMapImage = null;
        [SerializeField] private AnimationCurve _heightCurve = null;
        [SerializeField] private BiomeBuilder _biomeBuilder = null;
        [SerializeField] private GameObject[] _tilePrefabs = null;
        [SerializeField] private Transform _tilesParent = null;

        [SerializeField] private Wave[] _heightWaves = null;
        [SerializeField] private Wave[] _heatWaves = null;
        [SerializeField] private Wave[] _moistureWaves = null;
        
        [SerializeField] private TerrainType[] _heightTerrainTypes = null;
        [SerializeField] private TerrainType[] _heatTerrainTypes = null;
        [SerializeField] private TerrainType[] _moistureTerrainTypes = null;
        [SerializeField] private TerrainType[] _falloffMapTypes = null;

        private float[,] _heightMap = null;
        private float[,] _heatMap = null;
        private float[,] _moistureMap = null;
        private float[,] _falloffMap = null;
        private TerrainData[,] _dataMap = null;
        
        public void BuildWorld()
        {
            CreateHeightMap();
            ApplyFalloff();
            CreateHeatAndMoistureMaps();
            SpawnTiles();
        }

        private void CreateHeightMap()
        {
            if (_randomizeSeed == true)
            {
                _seed = Random.Range(-100000, 100001);
            }
            
            _heightMap = NoiseGenerator.GenerateNoiseMap(_seed, _sampleSize, _scale, _heightWaves);
        }
        
        private void CreateHeatAndMoistureMaps()
        {
            _heatMap = GenerateHeatMap(_heightMap);
            _moistureMap = GenerateMoistureMap(_heightMap);

            TerrainType[,] heatTerrainMap = TextureGenerator.CreateTerrainMap(_heatMap, _heatTerrainTypes);
            TerrainType[,] moistureTerrainMap = TextureGenerator.CreateTerrainMap(_moistureMap, _moistureTerrainTypes);
            CreateDataMap(heatTerrainMap, moistureTerrainMap);
            CreateTextures(heatTerrainMap, moistureTerrainMap);
        }
        
        private void SpawnTiles()
        {
            for (int x = 0; x < _sampleSize; x++)
            {
                for (int y = 0; y <  _sampleSize; y++)
                {
                    if(_heightMap[x,y] <= _heightTerrainTypes[1].Threshold) continue;

                    float xPosition = 0;
                    float yPosition = _heightMap[x, y] * _tileHeightModifier;
                    float zPosition = 0;
                    
                    if (y % 2 == 0)
                    {
                        xPosition = x * _tileOffsetX;
                        zPosition = y * _tileOffsetY;
                    }
                    else
                    {
                        xPosition = x * _tileOffsetX + _tileOffsetX / 2;
                        zPosition = y * _tileOffsetY;
                    }

                    int tileIndex = 0;

                    if (_heightMap[x, y] >= _heightTerrainTypes[4].Threshold)
                    {
                        tileIndex = 3;
                    }
                    else if (_heightMap[x, y] >= _heightTerrainTypes[3].Threshold)
                    {
                        tileIndex = 3;
                    }
                    else if (_heightMap[x, y] >= _heightTerrainTypes[2].Threshold)
                    {
                        tileIndex = 0;
                    }
                    else
                    {
                        tileIndex = 1;
                    }
                    
                    if (_dataMap[x, y].Biome.Name == "grass")
                    {
                        SpawnTile(_tilePrefabs[tileIndex], xPosition, yPosition, zPosition);
                    }
                    else if (_dataMap[x, y].Biome.Name == "desert")
                    {
                        SpawnTile(_tilePrefabs[tileIndex], xPosition, yPosition, zPosition);
                    }
                    else if (_dataMap[x, y].Biome.Name == "forest")
                    {
                        SpawnTile(_tilePrefabs[tileIndex], xPosition, yPosition, zPosition);
                    }
                    else if (_dataMap[x, y].Biome.Name == "rainforest")
                    {
                        SpawnTile(_tilePrefabs[tileIndex], xPosition, yPosition, zPosition);
                    }
                    else if (_dataMap[x, y].Biome.Name == "tundra")
                    {
                        SpawnTile(_tilePrefabs[tileIndex], xPosition, yPosition, zPosition);
                    }
                }
            }    
        }
        
        private void CreateTextures(TerrainType[,] heatTerrainMap, TerrainType[,] moistureTerrainMap)
        {
            if (_displayTextures == false)
            {
                _falloffMapImage.gameObject.SetActive(false);
                _heightMapImage.gameObject.SetActive(false);
                _heatMapImage.gameObject.SetActive(false);
                _moistureMapImage.gameObject.SetActive(false);
                _biomeMapImage.gameObject.SetActive(false);
                return;
            }
            
            Texture2D falloffMapTexture = TextureGenerator.BuildTexture(_falloffMap, _falloffMapTypes);
            _falloffMapImage.texture = falloffMapTexture;
            Texture2D heightMapTexture = TextureGenerator.BuildTexture(_heightMap, _heightTerrainTypes);
            _heightMapImage.texture = heightMapTexture;
            Texture2D heatMapTexture = TextureGenerator.BuildTexture(_heatMap, _heatTerrainTypes);
            _heatMapImage.texture = heatMapTexture;
            Texture2D moistureMapTexture = TextureGenerator.BuildTexture(_moistureMap, _moistureTerrainTypes);
            _moistureMapImage.texture = moistureMapTexture;
            Texture2D biomeMapTexture = _biomeBuilder.BuildTexture(heatTerrainMap, moistureTerrainMap);
            _biomeMapImage.texture = biomeMapTexture;
        }

        private void ApplyFalloff()
        {
            _falloffMap = NoiseGenerator.GenerateFalloffMap(_sampleSize);
            
            for (int x = 0; x < _sampleSize; x++)
            {
                for (int y = 0; y < _sampleSize; y++)
                {
                    _heightMap[x, y] -= 0.5f * _falloffMap[x, y];
                    //_heightMap[x, y] = Mathf.Clamp(_heightMap[x, y], 0.0f, 1f);
                }
            }
        }
        
        private void SpawnTile(GameObject prefab, float x, float y, float z)
        {
            GameObject clone = Instantiate(prefab, _tilesParent);
            clone.transform.position = new Vector3(x, y, z);
        }

        private void CreateDataMap(TerrainType[,] heatTerrainMap, TerrainType[,] moistureTerrainMap)
        {
            _dataMap = new TerrainData[_sampleSize, _sampleSize];
            for (int x = 0; x < _sampleSize; x++)
            {
                for (int y = 0; y < _sampleSize; y++)
                {
                    TerrainData data = new TerrainData();
                    data.X = x;
                    data.Y = y;
                    data.HeatTerrainType = heatTerrainMap[x, y];
                    data.MoistureTerrainType = moistureTerrainMap[x, y];
                    data.Biome = _biomeBuilder.GetBiome(data.HeatTerrainType, data.MoistureTerrainType);
                    _dataMap[x, y] = data;
                }
            }
        }
        
        public float[,] GenerateHeatMap(float[,] heightmap)
        {
            float[,] uniformMap = NoiseGenerator.GenerateUniformNoiseMap(_sampleSize, 1f);
            float[,] randomMap = NoiseGenerator.GenerateNoiseMap(_seed, _sampleSize, _heatScale, _heatWaves);
            float[,] map = new float[_sampleSize, _sampleSize];

            for (int x = 0; x < _sampleSize; x++)
            {
                for (int y = 0; y < _sampleSize; y++)
                {
                    map[x, y] = (0.5f * randomMap[x, y]) * uniformMap[x, y];
                    map[x, y] += 0.5f * heightmap[x, y];

                    map[x, y] = Mathf.Clamp(map[x, y], 0.0f, 1f);
                }
            }
            
            return map;
        }
        
        public float[,] GenerateMoistureMap(float[,] heightmap)
        {
            float[,] randomMap = NoiseGenerator.GenerateNoiseMap(_seed, _sampleSize, _moistureScale, _moistureWaves);

            for (int x = 0; x < _sampleSize; x++)
            {
                for (int y = 0; y < _sampleSize; y++)
                {
                    randomMap[x, y] -= 0.5f * heightmap[x, y];
                }
            }
            
            return randomMap;
        }
    }
}