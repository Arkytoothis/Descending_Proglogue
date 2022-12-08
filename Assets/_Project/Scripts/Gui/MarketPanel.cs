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
        [SerializeField] private Transform _shopWidgetsParent = null;

        private Village _village = null;
        private List<ShopWidget_Market> _shopWidgets = null;

        public void Setup()
        {
            _shopWidgets = new List<ShopWidget_Market>();
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

        public void OnSyncStockpile(bool b)
        {
            SyncStockpile();
        }

        public void DisplayVillage(Village village)
        {
            _village = village;
            SyncShopItems();
        }

        private void SyncShopItems()
        {
            _shopWidgetsParent.ClearTransform();
            _shopWidgets.Clear();

            if (_village == null) return;
            
            for (int i = 0; i < _village.MarketData.ShopItems.Count; i++)
            {
                GameObject clone = Instantiate(_shopWidgetPrefab, _shopWidgetsParent);
                ShopWidget_Market widget = clone.GetComponent<ShopWidget_Market>();
                widget.Setup(_village, i);
                widget.SetItem(_village.MarketData.ShopItems[i]);
            }
        }

        public void OnRemoveShopItem(int index)
        {
            _village.MarketData.ShopItems.RemoveAt(index);
            SyncShopItems();
        }
    }
}