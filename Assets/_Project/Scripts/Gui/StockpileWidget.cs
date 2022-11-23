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
    public class StockpileWidget : DragableItemWidget, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
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
                _stackSizeLabel.SetText("1");
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

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            DragCursor.Instance.BeginDrag(eventData, this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (DragCursor.Instance.IsDragging == true)
            {
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (DragCursor.Instance.IsDragging == true)
            {
                if (_item != null &&_item.ItemDefinition.Key != "")
                {
                    Item tempItem = new Item(_item);
                    
                    StockpileManager.Instance.SetItem(DragCursor.Instance.DragItem, _index);
                    SetItem(DragCursor.Instance.DragItem);
                    
                    StockpileManager.Instance.SetItem(tempItem, DragCursor.Instance.StartDragWidget.Index);
                    DragCursor.Instance.StartDragWidget.SetItem(tempItem);
                }
                else
                {
                    StockpileManager.Instance.SetItem(DragCursor.Instance.DragItem, _index);
                    SetItem(DragCursor.Instance.DragItem);

                    StockpileManager.Instance.ClearItem(DragCursor.Instance.StartDragWidget.Index);
                    DragCursor.Instance.StartDragWidget.Clear();
                }
                
                DragCursor.Instance.EndDrag(eventData);
            }
        }
    }
}