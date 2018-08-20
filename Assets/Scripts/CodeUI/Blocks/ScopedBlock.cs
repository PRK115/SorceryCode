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

        protected bool DynamicHeight = false;

        private GraphicRaycaster raycaster;
        private PointerEventData pointerEventData;
        private EventSystem eventSystem;

        protected virtual bool IsBlockValid(Block block) => true;

        protected override void Awake()
        {
            base.Awake();
            raycaster = FindObjectOfType<GraphicRaycaster>();
            eventSystem = FindObjectOfType<EventSystem>();
        }

        protected override void Start()
        {
            draggedBlock = CodeUIElement.Instance.DraggedBlock;
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;
            Block block = eventData.pointerDrag.GetComponent<Block>();
            if (block != null)
            {
                if (IsBlockValid(block) && block.IsMovable)
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
            blockListPanel.color = Color.white;
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
            blockListPanel.color = Color.white;
            if (eventData.pointerDrag == null) return;
            Block block = eventData.pointerDrag.GetComponent<Block>();
            if (block != null && draggedBlock != null)
            {
                draggedBlock = null;
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
            if (draggedBlock != null)
            {
                pointerEventData = new PointerEventData(eventSystem);
                pointerEventData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointerEventData, results);
                List<Block> hoveredBlocks = results
                    .Select(res => res.gameObject.GetComponent<Block>())
                    .Where(b => b != null && b != draggedBlock)
                    .ToList();

                if (hoveredBlocks.Count > 0)
                {
                    int maxDepth = hoveredBlocks.Max(b => b.Depth);
                    if (maxDepth == Depth)
                    {
                        draggedBlock.SetPlaceholderBlock(blockListPanel.transform, draggedBlock.transform.position);
                        blockListPanel.color = new Color(0.0f, 1.0f, 0.0f, 0.3f);
                        return;
                    }
                }
                blockListPanel.color = Color.white;
            }
        }
    }
}
