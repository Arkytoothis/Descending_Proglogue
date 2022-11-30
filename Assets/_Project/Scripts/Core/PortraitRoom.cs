using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Party;
using Descending.Units;
using UnityEngine;

namespace Descending.Core
{
    public class PortraitRoom : MonoBehaviour
    {
        public static PortraitRoom Instance { get; private set; }
        
        [SerializeField] private GameObject _portraitMountPrefab = null;
        [SerializeField] private Transform _portraitMountsParent = null;

        private List<PortraitMount> _portraits = null;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple Portrait Rooms " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            _portraits = new List<PortraitMount>();
        }
        
         public void Setup()
         {
             //Debug.Log("PortraitRoom.Setup");
             if (_portraits != null)
             {
                 for (int i = 0; i < _portraits.Count; i++)
                 {
                     _portraits[i].ClearMount();
                 }
             }
             
             for (int i = 0; i < UnitManager.Instance.HeroUnits.Count; i++)
             {
                 GameObject clone = Instantiate(_portraitMountPrefab, _portraitMountsParent);
                 clone.transform.localPosition = new Vector3(i * 10f, 0, 0);

                 PortraitMount mount = clone.GetComponent<PortraitMount>();
                 mount.Setup(UnitManager.Instance.HeroUnits[i]);
                 _portraits.Add(mount);
             }
         }

         public void SyncParty()
         {
             if (_portraits == null) return;
             
             //Debug.Log("Party Synced - Portrait Room");
             for (int i = 0; i < _portraits.Count; i++)
             {
                 _portraits[i].SetModel(UnitManager.Instance.HeroUnits[i]);
                 _portraits[i].Refresh();
             }
         }
    }
}
