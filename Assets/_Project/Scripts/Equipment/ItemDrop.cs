using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
//using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Equipment
{
    public class ItemDrop : MonoBehaviour
    {
        [SerializeField] private string _itemKey = null;
        //[SerializeField] private ItemEvent onPickupItem = null;
        
        public void Setup(ItemDefinition itemDefinition)
        {
            _itemKey = itemDefinition.Key;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Pickup Sphere"))
            {
                //onPickupItem.Invoke(new Item(Database.instance.Items.GetItem(_itemKey)));
                //Debug.Log(_itemKey + " picked up");
                Destroy(gameObject);
            }
        }
    }
}
