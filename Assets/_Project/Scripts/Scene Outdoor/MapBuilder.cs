using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Interactables;
using Descending.Tiles;
using Descending.Units;
using DunGen;
using UnityEngine;

namespace Descending.Scene_Outdoor
{
    public class MapBuilder : MonoBehaviour
    {
        [SerializeField] private CombatCameraController _cameraController = null;
        [SerializeField] private List<GameObject> _spawnPlotPrefabs = null;
        [SerializeField] private List<GameObject> _plotPrefabs = null;
        [SerializeField] private Transform _plotsParent = null;
        [SerializeField] private int _width = 0;
        [SerializeField] private int _height = 0;

        private GameObject[,] _plotObjects = null;
        
        public void Generate()
        {
            _plotObjects = new GameObject[_width, _height];
            
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (x == 0 && y == 0)
                    {
                        GameObject clone = Instantiate(_spawnPlotPrefabs[0], _plotsParent);
                        clone.transform.position = transform.position;
                        
                        _plotObjects[x, y] = clone;
                    }
                    else
                    {
                        GameObject clone = Instantiate(_plotPrefabs[0], _plotsParent);
                        clone.transform.position = transform.position + new Vector3(x * 10, 0, y * 10);
                        
                        _plotObjects[x, y] = clone;
                    }
                }
            }
            
            UnitManager.Instance.SpawnHeroes();
            UnitManager.Instance.SpawnEnemies();
            
            StartCoroutine(Finish_Coroutine());
        }

        private IEnumerator Finish_Coroutine()
        {
            yield return 0;
            
            _cameraController.transform.position = UnitManager.Instance.HeroUnits[0].transform.position;
            PathfindingManager.Instance.Scan();
            InteractableManager.Instance.Setup();
            UnitManager.Instance.SelectHeroDefault();
        }
    }
}