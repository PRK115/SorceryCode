using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeUI
{
    public class ScopedBlock : StmtBlock
    {
        private List<Block> blocks = new List<Block>();

        public GameObject blockListRoot;

        protected override void Start()
        {
            base.Start();
            UpdateBlocks();
        }

        private void UpdateBlocks()
        {
            blocks = GetComponentsInChildren<Block>().ToList();
            blocks.Sort((b1, b2) =>
                (int)(b1.rectTransform.anchoredPosition.y - b2.rectTransform.anchoredPosition.y));
        }

        public void AddBlock(Block block)
        {
            block.transform.SetParent(blockListRoot.transform);
            block.OriginalParent = blockListRoot.transform;
            block.OriginalPosition = block.transform.position;
            UpdateBlocks();
        }
    }
}
