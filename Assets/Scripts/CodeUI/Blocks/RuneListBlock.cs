using System.Collections.Generic;
using UnityEngine;

namespace CodeUI
{
    public class RuneListBlock : ScopedBlock
    {
        protected override bool IsBlockValid(Block block) => block.IsRune;

        [SerializeField] private EntityBlock entityBlockPrefab;
        [SerializeField] private ChangeTypeBlock changeTypeBlockPrefab;
        [SerializeField] private MoveDirBlock moveDirBlockPrefab;

        private RuneStock runeStock;

        protected override void Awake()
        {
            base.Awake();
            runeStock = FindObjectOfType<RuneStock>();
            runeStock.OnRuneUpdate += RuneUpdate;
        }

        public void RuneUpdate(RuneType runeType, int runeCount)
        {
            if (runeCount == 0)
            {
                // Remove rune from list
                Blocks.RemoveAll(block => block.IsRune && block.runeType == runeType);
            }
            else if (runeCount == 1)
            {
                // Add rune to list
                Block runeBlock = null;
                if (runeType.type == RuneType.Type.Entity)
                {
                    EntityBlock block = Instantiate(entityBlockPrefab);
                    block.EntityType = runeType.Entity;
                    runeBlock = block;
                }
                else if (runeType.type == RuneType.Type.Adjective)
                {
                    ChangeTypeBlock block = Instantiate(changeTypeBlockPrefab);
                    block.ChangeType = runeType.adjective;
                    runeBlock = block;
                }
                else if (runeType.type == RuneType.Type.Direction)
                {
                    MoveDirBlock block = Instantiate(moveDirBlockPrefab);
                    block.Dir = runeType.direction;
                    runeBlock = block;
                }
                runeBlock.transform.SetParent(blockListPanel.transform);
                Blocks.Add(runeBlock);
                runeBlock.SetRuneCount(runeCount);
            }
            else
            {
                // Update rune count
                Block runeBlock = Blocks.Find(block => block.IsRune && block.runeType == runeType);
                runeBlock.SetRuneCount(runeCount);
            }
        }
    }
}