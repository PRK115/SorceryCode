using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeUI
{
    public class ScopedBlock : StmtBlock
    {
        public List<StmtBlock> Blocks { get; private set; } = new List<StmtBlock>();

        public GameObject blockListRoot;

        protected override void Start()
        {
            base.Start();
            UpdateBlocks();
        }

        private void UpdateBlocks()
        {
            Blocks.Clear();
            foreach (Transform child in blockListRoot.transform)
            {
                Blocks.Add(child.GetComponent<StmtBlock>());
            }
            Blocks.Sort((b1, b2) =>
                (int)(b1.rectTransform.anchoredPosition.y - b2.rectTransform.anchoredPosition.y));
            Debug.Log($"{Blocks.Count} blocks");
        }

        public void AddBlock(StmtBlock block)
        {
            block.transform.SetParent(blockListRoot.transform);
            block.OriginalParent = blockListRoot.transform;
            block.OriginalPosition = block.transform.position;
            UpdateBlocks();
        }
    }
}
