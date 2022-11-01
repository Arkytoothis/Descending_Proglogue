using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using DunGen;
using UnityEngine;

namespace Descending.Dungeons
{
    public class DungeonCharacter : MonoBehaviour
    {
        private HeroUnit _hero = null;

        private void Awake()
        {
            _hero = GetComponent<HeroUnit>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hero == null) return;
            
            if (other.CompareTag("Dungeon Room"))
            {
                DungeonRoom room = other.GetComponent<DungeonRoom>();
                
                if (room != null)
                {
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
                    room.RemoveHeroFromRoom(_hero);
                }
            }
        }
    }
}