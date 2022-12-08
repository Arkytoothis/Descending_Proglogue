using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Equipment;
using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Descending.Gui
{
    public class StockpileWidget_Market : DragableItemWidget, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField] private Image _iconImage = null;
        [SerializeField] private TMP_Text _stackSizeLabel = null;

        [SerializeField] private ItemEvent onDisplayItemTooltip = null;

        public void Setup(int index)
        {
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
            if (_item == null || _item.Key == "") return;
            
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                TrySellItem();
            }
        }

        private void TrySellItem()
        {
            ResourcesManager.Instance.AddCoins((int)(_item.GoldValue * 0.5f));
            ResourcesManager.Instance.AddGems((int)(_item.GemValue * 0.5f));
            StockpileManager.Instance.ClearItem(_index);
            StockpileManager.Instance.SyncStockpile();
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) return;
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