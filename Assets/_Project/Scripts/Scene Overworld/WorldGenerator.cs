using System.Collections;
using System.Collections.Generic;
using System.IO;
using Descending.Core;
using Descending.Features;
using Descending.Party;
using Descending.Units;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Descending.Scene_Overworld
{
    public enum HeightTypes { Sand, Grass, Hills, Mountains, Mountain_Peak}
        
    public enum TileTypes { Sand, Grass, Hills, Mountains, Forest, Desert, Water_Shallow, Water_Deep}
    
    public class WorldGenerator : MonoBehaviour
    {
        public const int MAX_THREAT_LEVEL = 10;
        [SerializeField] private PartyController _partyController = null;
        [SerializeField] private PortraitRoom _portraitRoom = null;
        [SerializeField] private int _sampleSize = 10;
        [SerializeField] private int _seed = 0;
        [SerializeField] private bool _randomizeSeed = true;
        [SerializeField] private bool _displayTextures = true;
        [SerializeField] private float _scale = 1.1f;
        [SerializeField] private float _heatScale = 4f;
        [SerializeField] private float _moistureScale = 4f;
        [SerializeField] private float _tileOffsetX = 5f;
        [SerializeField] private float _tileOffsetY = 4.35f;
        [SerializeField] private float _heatModifier = 0.5f;
        [SerializeField] private float _moistureModifier = 0.5f;
        [SerializeField] private float _falloffMultiplier = 0.5f;
        [SerializeField] private RawImage _heightMapImage = null;
        [SerializeField] private RawImage _heatMapImage = null;
        [SerializeField] private RawImage _moistureMapImage = null;
        [SerializeField] private RawImage _falloffMapImage = null;
        [SerializeField] private RawImage _biomeMapImage = null;
        [SerializeField] private BiomeBuilder _biomeBuilder = null;
        [SerializeField] private FeaturePlacer _featurePlacer = null;
        [SerializeField] private GameObject[] _tilePrefabs = null;
        [SerializeField] private Transform _tilesParent = null;
        [SerializeField] private float _threatModifier = 10f;
        [SerializeField] private int _safeZoneSize = 2;
        [SerializeField] private int _forestSpawnChance = 80;
        [SerializeField] private int _minStartingX = 20;

        [SerializeField] private Wave[] _heightWaves = null;
        [SerializeField] private Wave[] _heatWaves = null;
        [SerializeField] private Wave[] _moistureWaves = null;

        [SerializeField] private TerrainType[] _heightTerrainTypes = null;
        [SerializeField] private TerrainType[] _heatTerrainTypes = null;
        [SerializeField] private TerrainType[] _moistureTerrainTypes = null;
        [SerializeField] private TerrainType[] _falloffMapTypes = null;
        
        [SerializeField] private int _maxTries = 20;
        [SerializeField] private int _groundTiles = 0;
        [SerializeField] private int _mountainTiles = 0;
        [SerializeField] private int _forestTiles = 0;
        [SerializeField] private int _minGroundTiles = 300;
        [SerializeField] private int _minMountainTiles = 50;
        [SerializeField] private bool _loadData = false;
        
        //[SerializeField] private int _maxMountainTiles = 70;
        //[SerializeField] private int _minForestTiles = 50;

        private float[,] _heightMap = null;
        private float[,] _heatMap = null;
        private float[,] _moistureMap = null;
        private float[,] _falloffMap = null;
        private TerrainData[,] _dataMap = null;
        private WorldTile[,] _tiles = null;
        private List<WorldTile> _spawnableTiles = null;
        private List<WorldTile> _shoreTiles = null;
        [SerializeField] private List<WorldTile> _startingTiles = null;
        private WorldTile _startingTile = null;
        private List<List<WorldTile>> _tilesByThreatLevel = null;
        private int _highestThreatLevel = 0;
        private int _tries = 0;

        public int SampleSize => _sampleSize;
        public WorldTile[,] Tiles => _tiles;

        public void Setup()
        {
            if (_loadData == true)
            {
                LoadState();
            }
            else
            {
                BuildWorld();
            }
        }
        
        public void BuildWorld()
        {
            for (_tries = 1; _tries < _maxTries; _tries++)
            {
                CreateHeightMap();
                ApplyFalloff();
                CreateHeatAndMoistureMaps();
                SpawnTiles();

                if (_groundTiles >= _minGroundTiles && 
                    //_forestTiles >= _minForestTiles && 
                    _mountainTiles >= _minMountainTiles)// && _mountainTiles <= _maxMountainTiles)
                {
                    StartCoroutine(FinalizeBuild());
                    break;
                }
            }
            
            //Debug.Log("Tries: " + _tries);
        }

        public void LoadWorld(WorldSaveData saveData)
        {
            if (_tiles != null)
            {
                ClearTiles();
            }
            
            _featurePlacer.ClearFeatures();
            
            _tiles = new WorldTile[saveData.Size, saveData.Size];
            _spawnableTiles = new List<WorldTile>();
            _startingTiles = new List<WorldTile>();
            _shoreTiles = new List<WorldTile>();
            _tilesParent.ClearTransform();
            _groundTiles = 0;
            _mountainTiles = 0;
            _forestTiles = 0;
            
            for (int x = 0; x < saveData.Size; x++)
            {
                for (int y = 0; y < saveData.Size; y++)
                {
                    float xPosition = 0;
                    float yPosition = 1f;
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

                    LoadTile(saveData.TileSaveData[x,y], x, y, new Vector3(xPosition, yPosition, zPosition));

                    if (saveData.TileSaveData[x, y].FeatureKey != "")
                    {
                        FeatureDefinition featureDefinition = Database.instance.Features.GetFeature(saveData.TileSaveData[x, y].FeatureKey);
                        _featurePlacer.PlaceFeature(featureDefinition, _tiles[x, y]);
                    }
                }
            }
            
            StartCoroutine(FinalizeLoad());
        }

        private void ClearTiles()
        {
            for (int x = 0; x < _tiles.GetLength(0); x++)
            {
                for (int y = 0; y < _tiles.GetLength(1); y++)
                {
                    Destroy(_tiles[x, y]);
                }
            }    
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
            _tiles = new WorldTile[_sampleSize, _sampleSize];
            _spawnableTiles = new List<WorldTile>();
            _startingTiles = new List<WorldTile>();
            _shoreTiles = new List<WorldTile>();
            _tilesParent.ClearTransform();
            _groundTiles = 0;
            _mountainTiles = 0;
            _forestTiles = 0;
            
            for (int x = 0; x < _sampleSize; x++)
            {
                for (int y = 0; y < _sampleSize; y++)
                {
                    float xPosition = 0;
                    float yPosition = 1f;
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

                    int tileIndex = -1;
                    
                    if (_heightMap[x, y] >= _heightTerrainTypes[(int)HeightTypes.Mountain_Peak].Threshold)
                    {
                        tileIndex = (int)TileTypes.Mountains;
                        _mountainTiles++;
                    }
                    else if (_heightMap[x, y] >= _heightTerrainTypes[(int)HeightTypes.Mountains].Threshold)
                    {
                        tileIndex = (int)TileTypes.Mountains;
                        _mountainTiles++;
                    }
                    else if (_heightMap[x, y] >= _heightTerrainTypes[(int)HeightTypes.Hills].Threshold)
                    {
                        tileIndex = (int)TileTypes.Hills;
                        _groundTiles++;
                    }
                    else if (_heightMap[x, y] >= _heightTerrainTypes[(int)HeightTypes.Grass].Threshold)
                    {
                        if (_dataMap[x, y].Biome.Name == "forest" || _dataMap[x, y].Biome.Name == "rainforest")
                        {
                            if (Random.Range(0, 100) < _forestSpawnChance)
                            {
                                tileIndex = (int)TileTypes.Forest;
                                _forestTiles++;
                                _groundTiles++;
                            }
                            else
                            {
                                tileIndex = (int)TileTypes.Grass;
                                _groundTiles++;
                            }
                        }
                        else if (_dataMap[x, y].Biome.Name == "desert")
                        {
                            tileIndex = (int)TileTypes.Desert;
                            _groundTiles++;
                        }
                        else
                        {
                            tileIndex = (int)TileTypes.Grass;
                            _groundTiles++;
                        }
                        
                    }
                    else if (_heightMap[x, y] >= _heightTerrainTypes[(int)HeightTypes.Sand].Threshold)
                    {
                        tileIndex = (int)TileTypes.Sand;
                    }
                    else
                    {
                        tileIndex = (int)TileTypes.Water_Shallow;
                    }

                    SpawnTile(tileIndex, x, y, new Vector3(xPosition, yPosition, zPosition));
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
                    _heightMap[x, y] -= _falloffMultiplier * _falloffMap[x, y];
                    //_heightMap[x, y] = Mathf.Clamp(_heightMap[x, y], 0.0f, 1f);
                }
            }
        }

        private void SpawnTile(int tileIndex, int tileX, int tileY, Vector3 spawnPosition)
        {
            GameObject clone = Instantiate(_tilePrefabs[tileIndex], _tilesParent);
            clone.transform.position = spawnPosition;
            
            WorldTile tile = clone.GetComponent<WorldTile>();
            tile.Setup(tileIndex, tileX, tileY);
            _tiles[tileX, tileY] = tile;

            if (tile.IsSpawnable)
            {
                _spawnableTiles.Add(tile);
            }
        }
        
        private void LoadTile(WorldTileSaveData saveData, int tileX, int tileY, Vector3 spawnPosition)
        {
            GameObject clone = Instantiate(_tilePrefabs[saveData.TileIndex], _tilesParent);
            clone.transform.position = spawnPosition;
            
            WorldTile tile = clone.GetComponent<WorldTile>();
            tile.LoadTile(saveData, tileX, tileY);
            
            _tiles[tileX, tileY] = tile;
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

        private float[,] GenerateHeatMap(float[,] heightmap)
        {
            float[,] uniformMap = NoiseGenerator.GenerateUniformNoiseMap(_sampleSize, 1f);
            float[,] randomMap = NoiseGenerator.GenerateNoiseMap(_seed, _sampleSize, _heatScale, _heatWaves);
            float[,] map = new float[_sampleSize, _sampleSize];

            for (int x = 0; x < _sampleSize; x++)
            {
                for (int y = 0; y < _sampleSize; y++)
                {
                    map[x, y] = (0.5f * randomMap[x, y]) * uniformMap[x, y];
                    map[x, y] += _heatModifier * heightmap[x, y];

                    map[x, y] = Mathf.Clamp(map[x, y], 0.0f, 1f);
                }
            }

            return map;
        }

        private float[,] GenerateMoistureMap(float[,] heightmap)
        {
            float[,] randomMap = NoiseGenerator.GenerateNoiseMap(_seed, _sampleSize, _moistureScale, _moistureWaves);

            for (int x = 0; x < _sampleSize; x++)
            {
                for (int y = 0; y < _sampleSize; y++)
                {
                    randomMap[x, y] -= _moistureModifier * heightmap[x, y];
                }
            }

            return randomMap;
        }

        private IEnumerator FinalizeBuild()
        {
            yield return null;
            _shoreTiles = new List<WorldTile>();
            
            for (int x = 0; x < _sampleSize; x++)
            {
                for (int y = 0; y < _sampleSize; y++)
                {
                    if (_tiles[x, y] != null && _tiles[x,y].IsWater == false)
                    {
                        _tiles[x,y].FindNeighbors(_tiles);
                    }
                }
            }
            
            for (int x = 0; x < _sampleSize; x++)
            {
                for (int y = 0; y < _sampleSize; y++)
                {
                    if (_tiles[x, y] != null)
                    {
                        if (_tiles[x, y].NeighborTiles.Count < 6 && _tiles[x,y].IsSpawnable)
                        {
                            _shoreTiles.Add(_tiles[x, y]);
                            
                            if (_tiles[x, y].Name == "Forest" && x >= _minStartingX)
                            {
                                _startingTiles.Add(_tiles[x, y]);
                            }
                        }
                    }
                }
            }

            ScanAstar();
            SpawnStartingVillage();
            CalculateThreatLevels();
            SpawnVillages();
            SpawnDungeons();
            SpawnParty();
        }
        
        private IEnumerator FinalizeLoad()
        {
            yield return null;
            _shoreTiles = new List<WorldTile>();
            
            for (int x = 0; x < _sampleSize; x++)
            {
                for (int y = 0; y < _sampleSize; y++)
                {
                    if (_tiles[x, y] != null && _tiles[x,y].IsWater == false)
                    {
                        _tiles[x,y].FindNeighbors(_tiles);
                    }
                }
            }
            
            for (int x = 0; x < _sampleSize; x++)
            {
                for (int y = 0; y < _sampleSize; y++)
                {
                    if (_tiles[x, y] != null)
                    {
                        if (_tiles[x, y].NeighborTiles.Count < 6 && _tiles[x,y].IsSpawnable)
                        {
                            _shoreTiles.Add(_tiles[x, y]);
                            
                            if (_tiles[x, y].Name == "Forest" && x >= _minStartingX)
                            {
                                _startingTiles.Add(_tiles[x, y]);
                            }
                        }
                    }
                }
            }

            ScanAstar();
        }

        private void SpawnStartingVillage()
        {
            int tileIndex = Random.Range(0, _startingTiles.Count);
            _startingTile = _startingTiles[tileIndex];
            _startingTiles.RemoveAt(tileIndex);
            _featurePlacer.PlaceFeature(Database.instance.Features.GetFeature("Village"), _startingTile);
        }
        

        private void SpawnVillages()
        {
            for (int i = 2; i < MAX_THREAT_LEVEL; i+=2)
            {
                int tileIndex = Random.Range(0, _tilesByThreatLevel[i].Count);
                WorldTile tile = _tilesByThreatLevel[i][tileIndex];
                _featurePlacer.PlaceFeature(Database.instance.Features.GetFeature("Village"), tile);
            }
        }

        private void SpawnDungeons()
        {
            for (int i = 1; i < _highestThreatLevel; i++)
            {
                int tileIndex = Random.Range(0, _tilesByThreatLevel[i].Count);
                WorldTile tile = _tilesByThreatLevel[i][tileIndex];
                _tilesByThreatLevel[i].RemoveAt(tileIndex);
                _featurePlacer.PlaceFeature(Database.instance.Features.GetFeature("Dungeon"), tile);
            }

            for (int i = 0; i < 5; i++)
            {
                if (_tilesByThreatLevel[_highestThreatLevel].Count == 0) break;
                
                int tileIndex = Random.Range(0, _tilesByThreatLevel[_highestThreatLevel].Count);
                WorldTile tile = _tilesByThreatLevel[_highestThreatLevel][tileIndex];
                _tilesByThreatLevel[_highestThreatLevel].RemoveAt(tileIndex);
                _featurePlacer.PlaceFeature(Database.instance.Features.GetFeature("Dungeon"), tile);
            }
        }

        private void ScanAstar()
        {
            AstarPath.active.Scan();
        }
        
        private void CalculateThreatLevels()
        {
            _tilesByThreatLevel = new List<List<WorldTile>>();
            _highestThreatLevel = 0;
            
            for (int i = 0; i <= MAX_THREAT_LEVEL; i++)
            {
                _tilesByThreatLevel.Add(new List<WorldTile>());
            }
            
            for (int x = 0; x < _sampleSize; x++)
            {
                for (int y = 0; y < _sampleSize; y++)
                {
                    if(_tiles[x,y] == null || _tiles[x,y].IsWater == true || _tiles[x,y].IsSpawnable == false) continue;
                    
                    float distance = Vector3.Distance(_startingTile.transform.position, _tiles[x, y].transform.position);
                    int threatLevel = Mathf.FloorToInt(distance / _threatModifier) - _safeZoneSize;
                    if (threatLevel < 0) threatLevel = 0;
                    else if (threatLevel > MAX_THREAT_LEVEL) threatLevel = MAX_THREAT_LEVEL;
                    
                    _tiles[x,y].SetThreatLevel(threatLevel);
                    _tilesByThreatLevel[threatLevel].Add(_tiles[x,y]);

                    if (threatLevel > _highestThreatLevel) _highestThreatLevel = threatLevel;
                }
            }
        }

        public void SpawnParty()
        {
            _partyController.transform.position = _startingTile.transform.position;
            _partyController.SetPartyLeader(0);
        }

        public void SaveState()
        {
            WorldSaveData saveData = new WorldSaveData(this);
            byte[] bytes = SerializationUtility.SerializeValue(saveData, DataFormat.JSON);
            File.WriteAllBytes(Database.instance.WorldDataFilePath, bytes);
        }
        
        public void LoadState()
        {
            if (!File.Exists(Database.instance.WorldDataFilePath)) return; // No state to load
	
            byte[] bytes = File.ReadAllBytes(Database.instance.WorldDataFilePath);
            WorldSaveData saveData = SerializationUtility.DeserializeValue<WorldSaveData>(bytes, DataFormat.JSON);
            LoadWorld(saveData);
        }

        public void SetLoadData(bool loadData)
        {
            _loadData = loadData;
        }
    }

    [System.Serializable]
    public class WorldSaveData
    {
        [SerializeField] private int _size = 0;
        [SerializeField] private WorldTileSaveData[,] _tileSaveData = null;

        public int Size => _size;
        public WorldTileSaveData[,] TileSaveData => _tileSaveData;

        public WorldSaveData(WorldGenerator worldGenerator)
        {
            _size = worldGenerator.SampleSize;
            _tileSaveData = new WorldTileSaveData[_size, _size];

            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    WorldTile tile = worldGenerator.Tiles[x, y];
                    WorldTileSaveData tileSaveData = new WorldTileSaveData(tile);
                    _tileSaveData[x, y] = tileSaveData;
                }
            }
        }
    }
}