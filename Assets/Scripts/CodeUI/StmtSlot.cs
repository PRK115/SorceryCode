using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CodeUI
{
    public class StmtSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
        public StmtBlock OwnerBlock;

        private Image image;

        protected void Awake()
        {
            image = GetComponent<Image>();
            OwnerBlock = GetComponentInParent<StmtBlock>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;
            Block draggedBlock = eventData.pointerDrag.GetComponent<Block>();
            if (draggedBlock != null)
            {
                if (draggedBlock is StmtBlock)
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
            if (draggedBlock != null)
            {
                image.color = Color.white;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("OnDrop");
            Block draggedBlock = eventData.pointerDrag.GetComponent<Block>();
            if (draggedBlock != null)
            {
                Debug.Log(draggedBlock);
                image.color = Color.white;

                if (draggedBlock is StmtBlock)
                {
                    Debug.Log(draggedBlock);
                    draggedBlock.IsInSlot = true;

                    if (OwnerBlock is ScopedBlock)
                    {
                        ScopedBlock Scope = (ScopedBlock) OwnerBlock;
                        Scope.AddBlock(draggedBlock);
                    }
                    else
                    {
                        OwnerBlock.ParentScope.AddBlock(draggedBlock);
                    }
                }
            }
        }
    }
}
