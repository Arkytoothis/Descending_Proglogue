using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Dungeons
{
    public class DungeonManager : MonoBehaviour
    {
        public static DungeonManager Instance { get; private set; }
        
        [SerializeField] private List<DungeonRoom> _rooms = null;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple Turn Managers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;

            _rooms = new List<DungeonRoom>();
        }
        
        public void RegisterDungeonRoom(DungeonRoom room)
        {
            room.SetIndex(_rooms.Count);
            _rooms.Add(room);
            
        }
    }
}