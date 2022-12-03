using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Gui
{
    public class TavernPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _container = null;
        
        public void Setup()
        {
            
        }
        
        public void Show()
        {
            _container.SetActive(true);
        }

        public void Hide()
        {
            _container.SetActive(false);
        }
    }
}