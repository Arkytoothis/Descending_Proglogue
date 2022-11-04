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
        }
        
         public void Setup()
         {
             _portraits = new List<PortraitMount>();
             //Debug.Log("Loading PartyManager");
             for (int i = 0; i < UnitManager.Instance.HeroUnits.Count; i++)
             {
                 GameObject clone = Instantiate(_portraitMountPrefab, _portraitMountsParent);
                 //clone.transform.SetParent(_portraitMountsParent, false);
                 clone.transform.localPosition = new Vector3(i * 10f, 0, 0);

                 PortraitMount mount = clone.GetComponent<PortraitMount>();
                 mount.Setup(UnitManager.Instance.HeroUnits[i] as HeroUnit);
                 _portraits.Add(mount);
             }
         }

         public void SyncParty()
         {
             //Debug.Log("Party Synced - Portrait Room");
             for (int i = 0; i < UnitManager.Instance.HeroUnits.Count; i++)
             {
                 _portraits[i].Refresh();
             }
         }
    }
}
