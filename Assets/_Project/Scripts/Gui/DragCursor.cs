using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Equipment;
using UnityEngine;
using UnityEngine.UI;

namespace Descending.Gui
{
    public class DragCursor : MonoBehaviour
    {
        public static DragCursor Instance { get; private set; }
        
        [SerializeField] private Image _iconImage = null;

        private DragableItemWidget _currentWidget = null;
        private Item _dragItem = null;
        private DragableItemWidget _startDragWidget = null;
        private bool _isDragging = false;
    
        public bool IsDragging => _isDragging;
        public DragableItemWidget CurrentWidget => _currentWidget;
        public Item DragItem => _dragItem;
        public DragableItemWidget StartDragWidget => _startDragWidget;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple Drag Cursors " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }


        private void Update()
        {
            _iconImage.rectTransform.position = Input.mousePosition;

            if (Input.GetMouseButtonUp(0))
            {
                if (_isDragging == true)
                {
                    Clear();
                }
            }
        }

        public void SetCurrentWidget(DragableItemWidget widget)
        {
            _currentWidget = widget;
        }
        
        public void StartDrag(DragableItemWidget startDragWidget)
        {
            _startDragWidget = startDragWidget;
            _dragItem = _startDragWidget.Item;
            _iconImage.sprite = _dragItem.ItemDefinition.Icon;
            _isDragging = true;
        }
        
        public void EndDrag()
        {
            Clear();
        }

        private void Clear()
        {
            _iconImage.sprite = Database.instance.BlankSprite;
            _startDragWidget = null;
            _dragItem = null;
            _isDragging = false;
            _currentWidget = null;
        }
    }
}