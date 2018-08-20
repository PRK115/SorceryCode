using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CodeUI
{
    public class ScopedBlock : StmtBlock, IDropHandler
    {
        public List<Block> Blocks { get; private set; } = new List<Block>();

        public Image blockListPanel;

        protected bool DynamicHeight = false;

        protected virtual bool IsBlockValid(Block block) => true;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            UpdateBlocks();
        }

        public void UpdateBlocks()
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

        public void OnDrop(PointerEventData eventData)
        {
            blockListPanel.color = Color.white;
            if (eventData.pointerDrag == null) return;
            Block block = eventData.pointerDrag.GetComponent<Block>();
            if (block != null)
            {
                if (IsBlockValid(block))
                {
                    if (DynamicHeight)
                    {
                        // TODO: Hardcoded, ugly ugly code
                        rectTransform.SetSizeWithCurrentAnchors(
                            RectTransform.Axis.Vertical, OriginalHeight + (Blocks.Count + 1) * 35);
                    }
                    block.SetRealBlock();
                    block.ContainedSlot = null;
                    block.ContainedScopedBlock = this;
                    block.Depth = this.Depth + 1;
                }
            }
        }

        void Update()
        {
            var draggedBlock = CodeUIElement.Instance.DraggedBlock;

            if (draggedBlock != null && draggedBlock.IsMovable)
            {
                var hoveredBlocks = CodeUIElement.Instance.HoveredBlocks;
                if (hoveredBlocks.Count > 0 && hoveredBlocks.Contains(this))
                {
                    int maxDepth = hoveredBlocks.Max(b => b.Depth);
                    if (maxDepth == this.Depth)
                    {
                        if (IsBlockValid(draggedBlock))
                        {
                            draggedBlock.SetPlaceholderBlock(blockListPanel.transform, draggedBlock.transform.position);
                            blockListPanel.color = new Color(0.0f, 1.0f, 0.0f, 0.3f);
                        }
                        else
                        {
                            blockListPanel.color = new Color(1.0f, 0.0f, 0.0f, 0.3f);
                        }
                    }
                    else
                    {
                        blockListPanel.color = Color.white;
                    }
                }
                else
                {
                    blockListPanel.color = Color.white;
                }
            }
        }
    }
}
