using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Thanks to https://github.com/Xander93/unity3d-draganddrop for drag-and-drop sample code
namespace CodeUI
{
    public class ExprSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
        public bool Filled = false;

        public Block OwnerBlock { get; set; }

        private RectTransform rectTransform;
        private Image image;

        public Block Block { get; private set; }

        public delegate bool CheckBlockValid(Block hoveredBlock);
        public CheckBlockValid CheckBlockValidCallback { get; set; }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            OwnerBlock = GetComponentInParent<Block>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;
            Block draggedBlock = eventData.pointerDrag.GetComponent<Block>();
            if (!Filled && draggedBlock != null)
            {
                if (CheckBlockValidCallback(draggedBlock))
                {
                    image.color = new Color(0.0f, 1.0f, 0.0f, 0.3f);
                }
                else
                {
                    image.color = new Color(1.0f, 0.0f, 0.0f, 0.3f);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;
            Block draggedBlock = eventData.pointerDrag.GetComponent<Block>();
            if (!Filled && draggedBlock != null)
            {
                image.color = Color.white;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            Block draggedBlock = eventData.pointerDrag.GetComponent<Block>();
            if (draggedBlock != null)
            {
                image.color = Color.white;

                if (!Filled && CheckBlockValidCallback(draggedBlock))
                {
                    Filled = true;
                    draggedBlock.IsInSlot = true;

                    // Change parent of dragged block
                    draggedBlock.transform.SetParent(this.transform);
                    draggedBlock.OriginalParent = this.transform;

                    // Snap dragged block to slot
                    RectTransform blockRect = draggedBlock.rectTransform;
                    blockRect.anchorMin = new Vector2(0, 0);
                    blockRect.anchorMax = new Vector2(1, 1);
                    blockRect.pivot = new Vector2(0.5f, 0.5f);
                    blockRect.sizeDelta = rectTransform.sizeDelta;
                    blockRect.offsetMin = new Vector2(0, 0);
                    blockRect.offsetMax = new Vector2(0, 0);
                    draggedBlock.OriginalPosition = draggedBlock.transform.position;

                    Block = draggedBlock;
                }
            }
            
        }
    }
}
