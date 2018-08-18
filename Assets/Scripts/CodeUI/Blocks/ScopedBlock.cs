using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CodeUI
{
    public class ScopedBlock : StmtBlock, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
        public List<Block> Blocks { get; private set; } = new List<Block>();

        public Image blockListPanel;

        private Block draggedBlock;

        protected virtual bool IsBlockValid(Block block) => true;

        protected override void Start()
        {
            base.Start();
            UpdateBlocks();
        }

        protected void UpdateBlocks()
        {
            Blocks.Clear();
            foreach (Transform child in blockListPanel.transform)
            {
                Block block = child.GetComponent<Block>();
                if (block != null && IsBlockValid(block))
                {
                    Blocks.Add(block);
                }
            }
            Blocks.Sort((b1, b2) =>
                (int)(b2.rectTransform.anchoredPosition.y - b1.rectTransform.anchoredPosition.y));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("OnPointerEnter");
            if (eventData.pointerDrag == null) return;
            Block block = eventData.pointerDrag.GetComponent<Block>();
            if (block != null)
            {
                if (IsBlockValid(block))
                {
                    draggedBlock = block;
                    blockListPanel.color = new Color(0.0f, 1.0f, 0.0f, 0.3f);
                }
                else
                {
                    blockListPanel.color = new Color(1.0f, 0.0f, 0.0f, 0.3f);
                }
                
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("OnPointerExit");
            blockListPanel.color = Color.white;
            UpdateBlocks();
            if (eventData.pointerDrag == null) return;
            Block block = eventData.pointerDrag.GetComponent<Block>();
            if (block != null)
            {
                draggedBlock = null;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("OnDrop");
            UpdateBlocks();
            if (eventData.pointerDrag == null) return;
            Block block = eventData.pointerDrag.GetComponent<Block>();
            if (block != null)
            {
                draggedBlock = null;
                blockListPanel.color = Color.white;
                if (IsBlockValid(block))
                {
                    block.SetRealBlock();
                }
            }
        }

        void Update()
        {
            if (draggedBlock != null)
            {
                draggedBlock.SetPlaceholderBlock(blockListPanel.transform, draggedBlock.transform.position);
            }
        }
    }
}
