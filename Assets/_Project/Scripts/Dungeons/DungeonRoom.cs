using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Interactables;
using Descending.Units;
using UnityEngine;

namespace Descending.Dungeons
{
    public class DungeonRoom : MonoBehaviour
    {
        [SerializeField] private List<GameLight> _roomLights = null;
        [SerializeField] private int _roomIndex = -1;

        [SerializeField] private List<HeroUnit> _heroesInRoom = null;

        private bool _lightsActive = false;
        private bool _isEmpty = true;

        public bool LightsActive => _lightsActive;
        public bool IsEmpty => _isEmpty;

        private void Awake()
        {
            _heroesInRoom = new List<HeroUnit>();
        }

        private void Start()
        {
            DungeonManager.Instance.RegisterDungeonRoom(this);
            DeactivateLights();
        }

        public void SetIndex(int index)
        {
            _roomIndex = index;
        }

        public void ActivateLights()
        {
            if (_roomLights == null || _lightsActive == true) return;
            
            _lightsActive = true;
            for (int i = 0; i < _roomLights.Count; i++)
            {
                _roomLights[i].Activate();
            }
        }

        public void DeactivateLights()
        {
            if (_roomLights == null) return;

            _lightsActive = false;
            for (int i = 0; i < _roomLights.Count; i++)
            {
                _roomLights[i].Deactivate();
            }
        }

        public void AddHeroToRoom(HeroUnit hero)
        {
            if (_heroesInRoom.Contains(hero) == false)
            {
                //Debug.Log(hero.GetShortName() + " entered " + name);
                _heroesInRoom.Add(hero);

                CheckIfEmpty();
            }
        }

        public void RemoveHeroFromRoom(HeroUnit hero)
        {
            if (_heroesInRoom.Contains(hero) == true)
            {
                //Debug.Log(hero.GetShortName() + " left " + name);
                _heroesInRoom.Remove(hero);
                
                CheckIfEmpty();
            }
        }

        private void CheckIfEmpty()
        {
            if (_heroesInRoom.Count > 0)
            {
                _isEmpty = false;
                ActivateLights();
            }
            else
            {
                _isEmpty = true;
                DeactivateLights();
            }
        }
    }
}
