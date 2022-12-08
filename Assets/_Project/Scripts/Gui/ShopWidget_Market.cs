using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Equipment;
using Descending.Features;
using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Descending.Gui
{
    public class ShopWidget_Market : DragableItemWidget, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField] private Image _iconImage = null;
        [SerializeField] private TMP_Text _stackSizeLabel = null;

        [SerializeField] private ItemEvent onDisplayItemTooltip = null;
        [SerializeField] private IntEvent onRemoveItemFromShop = null;

        private MarketData _marketData = null;
        
        public void Setup(Village village, int index)
        {
            _marketData = village.MarketData;
            _item = null;
            _index = index;
            _stackSizeLabel.SetText(_index.ToString());
        }

        public override void SetItem(Item item)
        {
            _item = item;

            if (_item != null)
            {
                _iconImage.sprite = item.Icon;

                if (_item.UsesLeft > 0)
                {
                    _stackSizeLabel.SetText(_item.UsesLeft + "/" + _item.MaxUses);
                }
                else
                {
                    _stackSizeLabel.SetText(_item.StackSize.ToString());
                }
            }
            else
            {
                Clear();
            }
        }

        public override void Clear()
        {
            _item = null;
            _iconImage.sprite = Database.instance.BlankSprite;
            _stackSizeLabel.SetText("");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onDisplayItemTooltip.Invoke(_item);
            eventData.pointerPress = gameObject;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onDisplayItemTooltip.Invoke(null);
        }
        

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_item == null) return;
            
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                TryBuyItem();
            }
        }

        private void TryBuyItem()
        {
            if (ResourcesManager.Instance.Coins >= _item.GoldValue && ResourcesManager.Instance.Gems >= _item.GemValue)
            {
                ResourcesManager.Instance.SpendCoins(_item.GoldValue);
                ResourcesManager.Instance.SPendGems(_item.GemValue);
                
                onRemoveItemFromShop.Invoke(_index);
                
                StockpileManager.Instance.AddItem(_item);
                StockpileManager.Instance.SyncStockpile();
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_item == null || _item.ItemDefinition.Key == "") return;
            if (eventData.button == PointerEventData.InputButton.Right) return;
            
            DragCursor.Instance.BeginDrag(eventData, this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) return;
            
            if (DragCursor.Instance.IsDragging == true)
            {
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) return;
            
            if (DragCursor.Instance.IsDragging == true)
            {
                // if (_item != null &&_item.ItemDefinition.Key != "")
                // {
                //     Item tempItem = new Item(_item);
                //     
                //     StockpileManager.Instance.SetItem(DragCursor.Instance.DragItem, _index);
                //     SetItem(DragCursor.Instance.DragItem);
                //
                //     if (DragCursor.Instance.StartDragWidget.GetType() == typeof(StockpileWidget))
                //     {
                //         StockpileManager.Instance.SetItem(tempItem, DragCursor.Instance.StartDragWidget.Index);
                //     }
                //     else if (DragCursor.Instance.StartDragWidget.GetType() == typeof(EquippedItemWidget))
                //     {
                //         UnitManager.Instance.SelectedHero.Inventory.EquipItem(tempItem, DragCursor.Instance.StartDragWidget.Index);
                //     }
                //     else if (DragCursor.Instance.StartDragWidget.GetType() == typeof(AccessoryWidget))
                //     {
                //         UnitManager.Instance.SelectedHero.Inventory.EquipAccessory(tempItem, DragCursor.Instance.StartDragWidget.Index);
                //     }
                //
                //     DragCursor.Instance.StartDragWidget.SetItem(tempItem);
                // }
                // else
                // {
                //     StockpileManager.Instance.SetItem(DragCursor.Instance.DragItem, _index);
                //     SetItem(DragCursor.Instance.DragItem);
                //
                //     if (DragCursor.Instance.StartDragWidget.GetType() == typeof(StockpileWidget))
                //     {
                //         StockpileManager.Instance.ClearItem(DragCursor.Instance.StartDragWidget.Index);
                //     }
                //     else if (DragCursor.Instance.StartDragWidget.GetType() == typeof(EquippedItemWidget))
                //     {
                //         UnitManager.Instance.SelectedHero.Inventory.UnequipItem(DragCursor.Instance.StartDragWidget.Index, false);
                //     }
                //     else if (DragCursor.Instance.StartDragWidget.GetType() == typeof(AccessoryWidget))
                //     {
                //         UnitManager.Instance.SelectedHero.Inventory.ClearAccessory(DragCursor.Instance.StartDragWidget.Index);
                //     }
                // }
                //
                // DragCursor.Instance.StartDragWidget.Clear();
                // DragCursor.Instance.EndDrag(eventData);
                // StockpileManager.Instance.SyncStockpile();
                // UnitManager.Instance.SyncHeroes();
                // UnitManager.Instance.RefreshSelectedHero();
            }
        }
    }
}