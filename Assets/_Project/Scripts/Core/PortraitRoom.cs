using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using UnityEngine;

namespace Descending.Core
{
    public class PortraitRoom : MonoBehaviour
    {
        [SerializeField] private GameObject _portraitMountPrefab = null;
        [SerializeField] private Transform _portraitMountsParent = null;

        private List<PortraitMount> _portraits = null;
        
         public void Setup()
         {
             _portraits = new List<PortraitMount>();
             
             for (int i = 0; i < UnitManager.Instance.PlayerUnits.Count; i++)
             {
                 GameObject clone = Instantiate(_portraitMountPrefab, _portraitMountsParent);
                 //clone.transform.SetParent(_portraitMountsParent, false);
                 clone.transform.localPosition = new Vector3(i * 10f, 0, 0);
                 
                 PortraitMount mount = clone.GetComponent<PortraitMount>();
                 //mount.Setup(UnitManager.Instance.PlayerUnits[i]);
                 _portraits.Add(mount);
             }
         }

         public void SyncParty()
         {
             Debug.Log("Party Synced - Portrait Room");
             // for (int i = 0; i < PartyManager.instance.CurrentHeroes.Count; i++)
             // {
             //     _portraits[i].Refresh();
             // }
         }
    }
}
