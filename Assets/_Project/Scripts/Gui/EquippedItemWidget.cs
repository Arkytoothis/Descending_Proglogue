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
    public class EquippedItemWidget : DragableItemWidget, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
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

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_item == null || _item.ItemDefinition.Key == "") return;
            
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                UnitManager.Instance.SelectedHero.Inventory.UnequipItem(_index, true);
                Clear();
                StockpileManager.Instance.SyncStockpile();
                UnitManager.Instance.SyncHeroes();
                UnitManager.Instance.RefreshSelectedHero();
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_item == null || _item.ItemDefinition.Key == "") return;
            
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
                if ((int)DragCursor.Instance.DragItem.ItemDefinition.EquipmentSlot != _index) return;
                if (DragCursor.Instance.DragItem.ItemDefinition.Category == ItemCategory.Accessories) return;
                
                HeroUnit hero = UnitManager.Instance.SelectedHero;
                
                if (_item != null && _item.Key != "")
                {
                    Item tempItem = new Item(_item);
                    hero.Inventory.EquipItem(DragCursor.Instance.DragItem, _index);
                    SetItem(DragCursor.Instance.DragItem);
                    
                    if (DragCursor.Instance.StartDragWidget.GetType() == typeof(StockpileWidget))
                    {
                        StockpileManager.Instance.SetItem(tempItem, DragCursor.Instance.StartDragWidget.Index);
                    }
                    
                    DragCursor.Instance.StartDragWidget.SetItem(tempItem);
                }
                else
                {
                    hero.Inventory.EquipItem(DragCursor.Instance.DragItem, _index);
                    SetItem(DragCursor.Instance.DragItem);

                    if (DragCursor.Instance.StartDragWidget.GetType() == typeof(StockpileWidget))
                    {
                        StockpileManager.Instance.ClearItem(DragCursor.Instance.StartDragWidget.Index);
                    }
                    
                    DragCursor.Instance.StartDragWidget.Clear();
                }
                
                DragCursor.Instance.EndDrag(eventData);
                StockpileManager.Instance.SyncStockpile();
                UnitManager.Instance.SyncHeroes();
                UnitManager.Instance.RefreshSelectedHero();
            }
        }
    }
}