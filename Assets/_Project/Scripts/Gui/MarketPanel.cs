using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Features;
using UnityEngine;

namespace Descending.Gui
{
    public class MarketPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _container = null;
        [SerializeField] private StockpileWidget_Market[] _stockpileWidgets = null;
        [SerializeField] private GameObject _shopWidgetPrefab = null;
        [SerializeField] private Transform __shopWidgetsParent = null;

        private List<ShopWidget_Market> _shopWidgets = null;

        public void Setup()
        {
            
        }
        
        public void Show()
        {
            _container.SetActive(true);
            SyncStockpile();
        }

        public void Hide()
        {
            _container.SetActive(false);
        }

        public void SyncStockpile()
        {
            if (StockpileManager.Instance.Items == null) return;
            
            for (int i = 0; i < _stockpileWidgets.Length; i++)
            {
                _stockpileWidgets[i].Setup(i);
                _stockpileWidgets[i].SetItem(StockpileManager.Instance.GetItem(i));
            }
        }

        public void DisplayVillage(Village village)
        {
            Debug.Log("Displaying Village");
        }
    }
}