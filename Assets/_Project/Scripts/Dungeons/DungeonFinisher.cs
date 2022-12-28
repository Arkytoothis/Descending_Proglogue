using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Interactables;
using Descending.Tiles;
using Descending.Units;
using DunGen;
using DunGen.Adapters;
using UnityEngine;

namespace Descending.Dungeons
{
    public class DungeonFinisher : BaseAdapter
    {
        [SerializeField] private CombatCameraController _cameraController = null;

        protected override void Run(DungeonGenerator generator)
        {
            //Debug.Log("Finishing Dungeon");
            HeroManager_Combat.Instance.SpawnHeroes();
            
            StartCoroutine(Finish_Coroutine());
        }

        private IEnumerator Finish_Coroutine()
        {
            yield return 0;
            
            _cameraController.transform.position = HeroManager_Combat.Instance.HeroUnits[0].transform.position;
            PathfindingManager.Instance.Scan();
            InteractableManager.Instance.Setup();
            HeroManager_Combat.Instance.SyncHeroes();
            HeroManager_Combat.Instance.SelectHeroDefault();
        }
    }
}
