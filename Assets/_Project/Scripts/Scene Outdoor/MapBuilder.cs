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
        [SerializeField] private List<GameObject> _plotPrefabs = null;
        [SerializeField] private Transform _plotsParent = null;

        private GameObject[,] _plotObjects = null;
        private int _width = 0;
        private int _height = 0;
        
        public void Generate(int width, int height)
        {
            _width = width;
            _height = height;
            _plotObjects = new GameObject[_width, _height];
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    GameObject clone = Instantiate(_plotPrefabs[0], _plotsParent);
                    clone.transform.position = new Vector3(x * 10, 0, y * 10);
                    
                    _plotObjects[x, y] = clone;
                }
            }
            
            StartCoroutine(Finish_Coroutine());
        }
        
        [SerializeField] private CombatCameraController _cameraController = null;

        private IEnumerator Finish_Coroutine()
        {
            yield return 0;
            
            _cameraController.transform.position = UnitManager.Instance.HeroUnits[0].transform.position;
            UnitManager.Instance.SpawnHeroes();
            UnitManager.Instance.SpawnEnemies();
            UnitManager.Instance.SelectHero(UnitManager.Instance.GetHero(0));
            PathfindingManager.Instance.Scan();
            InteractableManager.Instance.Setup();
        }
    }
}