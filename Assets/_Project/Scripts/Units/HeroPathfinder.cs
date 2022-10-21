using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;
using UnityEngine;

namespace Descending.Units
{
    public class HeroPathfinder : MonoBehaviour
    {
        //[SerializeField] private RichAI _richAI = null;
        [SerializeField] private AIPath _aiPath = null;
        [SerializeField] private Seeker _seeker = null;
        //[SerializeField] private RVOController _rvoController = null;

        public void SetDestination(Vector3 destination)
        {
            //Debug.Log("Moving to: " + destination, gameObject);
            //_seeker.StartPath(transform.position, destination);
            
            var path = ABPath.Construct(transform.position, destination, null);

            // Make the path use a specific traversal provider
            path.traversalProvider = traversalProvider;

            // Calculate the path synchronously
            AstarPath.StartPath(path);
            path.BlockUntilCalculated();

            if (path.error) {
                Debug.Log("No path was found");
            } else {
                Debug.Log("A path was found with " + path.vectorPath.Count + " nodes");

                // Draw the path in the scene view
                for (int i = 0; i < path.vectorPath.Count - 1; i++) {
                    Debug.DrawLine(path.vectorPath[i], path.vectorPath[i + 1], Color.red);
                }
            }
        }

        public void Enable()
        {
            SetPathfindingActive(true);
            //_rvoController = gameObject.AddComponent<RVOController>();
        }

        public void Disable()
        {
            Destroy(_aiPath);
            Destroy(_seeker);
        }

        public void SetPathfindingActive(bool active)
        {
            _aiPath.enabled = active;
            _seeker.enabled = active;
        }
        
        public void SetRvoLocked(bool locked)
        {
            //_rvoController.locked = locked;
        }
        
        public BlockManager blockManager;
        public List<SingleNodeBlocker> obstacles;

        BlockManager.TraversalProvider traversalProvider;
        
        public void Start () {
            // Create a traversal provider which says that a path should be blocked by all the SingleNodeBlockers in the obstacles array
            traversalProvider = new BlockManager.TraversalProvider(blockManager, BlockManager.BlockMode.OnlySelector, obstacles);
        }
    }
} 