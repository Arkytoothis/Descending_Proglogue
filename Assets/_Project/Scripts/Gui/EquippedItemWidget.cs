using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Equipment;
using Descending.Units;
using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Descending.Gui
{
    public class EquippedItemWidget : DragableItemWidget, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
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

        private void Clear()
        {
            _iconImage.sprite = Database.instance.BlankSprite;
            _stackSizeLabel.SetText("");
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            onDisplayItemTooltip.Invoke(_item);
            DragCursor.Instance.SetCurrentWidget(this);
            eventData.pointerPress = gameObject;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onDisplayItemTooltip.Invoke(null);
            DragCursor.Instance.SetCurrentWidget(null);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_item.Key == "") return;
            
            DragCursor.Instance.StartDrag(this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(DragCursor.Instance.IsDragging == true)
            {
                Debug.Log("EndDrag EquippedItemWidget");
                
                if (DragCursor.Instance.CurrentWidget == null)
                {
                    //StockpileManager.Instance.SetItem(DragCursor.Instance.DragItem, DragCursor.Instance.StartDragWidget.Index);
                    return;
                }

                if (DragCursor.Instance.CurrentWidget.Item.Key == "")
                {
                    //StockpileManager.Instance.SetItem(DragCursor.Instance.DragItem, DragCursor.Instance.CurrentWidget.Index);
                    
                    StockpileManager.Instance.ClearItem(DragCursor.Instance.StartDragWidget.Index);
                }
                else
                {
                    //Item tempItem = new Item(DragCursor.Instance.CurrentWidget.Item);
                    //StockpileManager.Instance.SetItem(DragCursor.Instance.DragItem, DragCursor.Instance.CurrentWidget.Index);
                    //StockpileManager.Instance.SetItem(tempItem, DragCursor.Instance.StartDragWidget.Index);
                }

                StockpileManager.Instance.SyncStockpile();
                DragCursor.Instance.EndDrag();
            }
        }
    }
}