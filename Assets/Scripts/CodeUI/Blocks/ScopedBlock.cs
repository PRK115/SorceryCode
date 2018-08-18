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
        public List<StmtBlock> Blocks { get; private set; } = new List<StmtBlock>();

        public GameObject blockListRoot;
        public Image blockListPanel;

        private Block draggedBlock;

        protected override void Start()
        {
            base.Start();
            UpdateBlocks();
        }

        protected void UpdateBlocks()
        {
            Blocks.Clear();
            foreach (Transform child in blockListRoot.transform)
            {
                StmtBlock stmtBlock = child.GetComponent<StmtBlock>();
                if (stmtBlock != null)
                {
                    Blocks.Add(stmtBlock);
                }
            }
            Blocks.Sort((b1, b2) =>
                (int)(b1.rectTransform.anchoredPosition.y - b2.rectTransform.anchoredPosition.y));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;
            Block block = eventData.pointerDrag.GetComponent<Block>();
            if (block != null)
            {
                draggedBlock = block;
                if (block is StmtBlock)

                {
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
            if (eventData.pointerDrag == null) return;
            Block block = eventData.pointerDrag.GetComponent<Block>();
            if (block != null)
            {
                draggedBlock = null;
                blockListPanel.color = Color.white;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;
            Block block = eventData.pointerDrag.GetComponent<Block>();
            if (block != null)
            {
                draggedBlock = null;
                blockListPanel.color = Color.white;
                if (block is StmtBlock)
                {
                    block.SetRealBlock();
                    UpdateBlocks();
                }
            }
        }

        void Update()
        {
            if (draggedBlock != null)
            {
                int originalIndex = draggedBlock.OriginalSiblingIndex;
                draggedBlock.SetPlaceholderBlock(blockListRoot.transform, draggedBlock.transform.position, Blocks);
            }
        }
    }
}
