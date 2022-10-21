using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Tiles
{
    [System.Serializable]
    public class TileMap<T>
    {
        private int _width = 0;
        private int _height = 0;
        private float _cellSize = 0f;
        private T[,] _gridObjects = null;

        public int Width => _width;
        public int Height => _height;
        public float CellSize => _cellSize;

        public TileMap(int width, int height, float cellSize, Func<TileMap<T>, MapPosition, T> createGridObject)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _gridObjects = new T[_width, _height];
            
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    MapPosition mapPosition = new MapPosition(x, y);
                    _gridObjects[x, y] = createGridObject(this, mapPosition);
                }
            }
        }

        public Vector3 GetWorldPosition(MapPosition mapPosition)
        {
            return new Vector3(mapPosition.X, 0, mapPosition.Y) * _cellSize;
        }

        public MapPosition GetGridPosition(Vector3 worldPosition)
        {
            return new MapPosition( Mathf.RoundToInt(worldPosition.x / _cellSize), Mathf.RoundToInt(worldPosition.z / _cellSize));
        }

        public void CreateDebugObjects(GameObject debugPrefab, Transform parent)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    MapPosition mapPosition = new MapPosition(x, y);
                    GameObject clone = GameObject.Instantiate(debugPrefab, GetWorldPosition(mapPosition) + new Vector3(0.5f, 0, 0.5f), Quaternion.identity);
                    clone.transform.SetParent(parent);
                    TileDebugObject tileDebugObject = clone.GetComponent<TileDebugObject>();
                    tileDebugObject.SetGridObject(GetGridObject(mapPosition));
                }
            }
        }

        public T GetGridObject(MapPosition mapPosition)
        {
            //Debug.Log("mapPosition.X: " + mapPosition.X + " mapPosition.Y: " + mapPosition.Y);
            return _gridObjects[mapPosition.X, mapPosition.Y];
        }

        public bool IsValidGridPosition(MapPosition mapPosition)
        {
            return mapPosition.X >= 0 && mapPosition.Y >= 0 && mapPosition.X < _width && mapPosition.Y < _height;
        }
    }
}
