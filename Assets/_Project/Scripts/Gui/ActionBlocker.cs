using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using UnityEngine;

namespace Descending.Gui
{
    public class ActionBlocker : MonoBehaviour
    {
        [SerializeField] private GameObject _container = null;

        private void Start()
        {
            ActionManager.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
            Hide();
        }

        private void Show()
        {
            _container.SetActive(true);    
        }

        private void Hide()
        {
            _container.SetActive(false);  
        }

        private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
        {
            if (isBusy)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
    }
}