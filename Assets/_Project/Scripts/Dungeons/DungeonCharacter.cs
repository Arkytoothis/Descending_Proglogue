using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using UnityEngine;

namespace Descending.Dungeons
{
    public class DungeonCharacter : MonoBehaviour
    {
        [SerializeField] private HeroUnit _hero = null;

        private void OnTriggerEnter(Collider other)
        {
            if (_hero == null) return;
            
            if (other.CompareTag("Dungeon Room"))
            {
                DungeonRoom room = other.GetComponent<DungeonRoom>();
                
                if (room != null)
                {
                    //Debug.Log("Room Entered");
                    room.AddHeroToRoom(_hero);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_hero == null) return;
            
            if (other.CompareTag("Dungeon Room"))
            {
                DungeonRoom room = other.GetComponent<DungeonRoom>();
                
                if (room != null)
                {
                    //Debug.Log("Room Exited");
                    room.RemoveHeroFromRoom(_hero);
                }
            }
        }
    }
}