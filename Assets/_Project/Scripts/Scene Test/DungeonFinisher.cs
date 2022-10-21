using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Tiles;
using Descending.Units;
using DunGen;
using DunGen.Adapters;
using UnityEngine;

namespace Descending.Scene_Test
{
    public class DungeonFinisher : BaseAdapter
    {
        [SerializeField] private CombatCameraController _cameraController = null;

        protected override void Run(DungeonGenerator generator)
        {
            //Debug.Log("Finishing Dungeon");
            UnitManager.Instance.SpawnHeroes();
            UnitManager.Instance.SpawnEnemies();
            StartCoroutine(Finish_Coroutine());
        }

        private IEnumerator Finish_Coroutine()
        {
            yield return 0;
            
            _cameraController.transform.position = UnitManager.Instance.PlayerUnits[0].transform.position;
            PathfindingManager.Instance.Scan();
        }
    }
}
