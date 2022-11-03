using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Descending.Tiles
{
    public class PathfindingManager : MonoBehaviour
    {
        public static PathfindingManager Instance { get; private set; }
        
        private const int MOVE_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;
        
        [SerializeField] private GameObject _tileDebugPrefab;
        [SerializeField] private Transform _tilesParent;
        [SerializeField] private float _raycastLength = 10f;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private LayerMask _obstaclesLayerMask;
        [SerializeField] private bool _showDebugTiles = false;
        
        private int _width;
        private int _height;
        private float _cellSize;
        private TileMap<PathNode> tileMap;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple UnitActionSystems " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        public void Setup(int width, int height, float cellSize)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            tileMap = new TileMap<PathNode>(_width, _height, _cellSize, (TileMap<PathNode> g, MapPosition gridPosition) => new PathNode(gridPosition));

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    GetNode(x, y).SetIsWalkable(false);
                }
            }
        }

        [Button]
        public void Scan()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    MapPosition mapPosition = new MapPosition(x, y);
                    Vector3 worldPosition = MapManager.Instance.GetWorldPosition(mapPosition);
                    if (Physics.Raycast(worldPosition + Vector3.up * _raycastLength, Vector3.down, _raycastLength * 2f, _groundLayerMask))
                    {
                        GetNode(x, y).SetIsWalkable(true);
                    }

                    if (Physics.Raycast(worldPosition + Vector3.up * _raycastLength, Vector3.down, _raycastLength * 2f, _obstaclesLayerMask))
                    {
                        GetNode(x, y).SetIsWalkable(false);
                    }
                }
            }

            if (_showDebugTiles)
            {
                tileMap.CreateDebugObjects(_tileDebugPrefab, _tilesParent);
            }
        }

        public List<MapPosition> FindPath(MapPosition startMapPosition, MapPosition endMapPosition, out int pathLength)
        {
            List<PathNode> openList = new List<PathNode>();
            List<PathNode> closedList = new List<PathNode>();
            PathNode startNode = tileMap.GetGridObject(startMapPosition);
            PathNode endNode = tileMap.GetGridObject(endMapPosition);
            
            openList.Add(startNode);

            for (int x = 0; x < tileMap.Width; x++)
            {
                for (int y = 0; y < tileMap.Height; y++)
                {
                    MapPosition mapPosition = new MapPosition(x, y);
                    PathNode pathNode = tileMap.GetGridObject(mapPosition);
                    pathNode.SetGCost(int.MaxValue);
                    pathNode.SetHCost(0);
                    pathNode.CalculateFCost();
                    pathNode.ResetPreviousNode();
                }
            }
            
            startNode.SetGCost(0);
            startNode.SetHCost(GetDistance(startMapPosition, endMapPosition));
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openList);
                if (currentNode == endNode)
                {
                    pathLength = endNode.FCost;
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (PathNode neighborNode in GetNeighborList(currentNode))
                {
                    if (closedList.Contains(neighborNode))
                    {
                        continue;
                    }

                    if (!neighborNode.IsWalkable)
                    {
                        closedList.Add(neighborNode);
                        continue;
                    }
                    
                    int gCost = currentNode.GCost + GetDistance(currentNode.MapPosition, neighborNode.MapPosition);
                    if (gCost < neighborNode.GCost)
                    {
                        neighborNode.SetPreviousNode(currentNode);
                        neighborNode.SetGCost(gCost);
                        neighborNode.SetHCost(GetDistance(neighborNode.MapPosition, endMapPosition));
                        neighborNode.CalculateFCost();

                        if (!openList.Contains(neighborNode))
                        {
                            openList.Add(neighborNode);
                        }
                    }
                }
            }

            pathLength = 0;
            return null;
        }

        public int GetDistance(MapPosition a, MapPosition b)
        {
            MapPosition position = a - b;
            int xDistance = Mathf.Abs(position.X);
            int yDistance = Mathf.Abs(position.Y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_COST * remaining;
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodes)
        {
            PathNode lowestFCostNode = pathNodes[0];

            for (int i = 0; i < pathNodes.Count; i++)
            {
                if (pathNodes[i].FCost < lowestFCostNode.FCost)
                {
                    lowestFCostNode = pathNodes[i];
                }
            }

            return lowestFCostNode;
        }

        private List<PathNode> GetNeighborList(PathNode currentNode)
        {
            List<PathNode> neighborList = new List<PathNode>();
            MapPosition mapPosition = currentNode.MapPosition;

            if (mapPosition.X - 1 >= 0)
            {
                neighborList.Add(GetNode(mapPosition.X - 1, mapPosition.Y));

                if (mapPosition.Y - 1 >= 0)
                {
                    neighborList.Add(GetNode(mapPosition.X - 1, mapPosition.Y - 1));
                }

                if (mapPosition.Y + 1 < tileMap.Height)
                {
                    neighborList.Add(GetNode(mapPosition.X - 1, mapPosition.Y + 1));
                }
            }

            if (mapPosition.X + 1 < tileMap.Width)
            {
                neighborList.Add(GetNode(mapPosition.X + 1, mapPosition.Y));
                
                if (mapPosition.Y - 1 >= 0)
                {
                    neighborList.Add(GetNode(mapPosition.X + 1, mapPosition.Y - 1));
                }

                if (mapPosition.Y + 1 < tileMap.Height)
                {
                    neighborList.Add(GetNode(mapPosition.X + 1, mapPosition.Y + 1));
                }
            }

            if (mapPosition.Y - 1 >= 0)
            {
                neighborList.Add(GetNode(mapPosition.X, mapPosition.Y - 1));
            }

            if (mapPosition.Y + 1 < tileMap.Height)
            {
                neighborList.Add(GetNode(mapPosition.X, mapPosition.Y + 1));
            }

            return neighborList;
        }

        private PathNode GetNode(int x, int y)
        {
            return tileMap.GetGridObject(new MapPosition(x, y));
        }

        private List<MapPosition> CalculatePath(PathNode endNode)
        {
            List<PathNode> pathNodes = new List<PathNode>();
            pathNodes.Add(endNode);
            PathNode currentNode = endNode;

            while (currentNode.PreviousNode != null)
            {
                pathNodes.Add(currentNode.PreviousNode);
                currentNode = currentNode.PreviousNode;
            }

            pathNodes.Reverse();

            List<MapPosition> gridPositions = new List<MapPosition>();
            foreach (PathNode node in pathNodes)
            {
                gridPositions.Add(node.MapPosition);
            }

            return gridPositions;
        }

        public bool IsGridPositionWalkable(MapPosition mapPosition)
        {
            return tileMap.GetGridObject(mapPosition).IsWalkable;
        }

        public void SetIsGridPositionWalkable(MapPosition mapPosition, bool isWalkable)
        {
            tileMap.GetGridObject(mapPosition).SetIsWalkable(isWalkable);
        }

        public bool HasPath(MapPosition startPosition, MapPosition endPosition)
        {
            return FindPath(startPosition, endPosition, out int pathLength) != null;
        }

        public int GetPathLength(MapPosition startPosition, MapPosition endPosition)
        {
            if (FindPath(startPosition, endPosition, out int pathLength) != null)
            {
                return pathLength;
            }
            else
            {
                return int.MaxValue;
            }
        }
    }
}