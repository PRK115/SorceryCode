using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeUI
{
    public class RuneListBlock : ScopedBlock, IDropHandler
    {
        protected override bool IsBlockValid(Block block) => block.IsRune;

        [SerializeField] private EntityBlock entityBlockPrefab;
        [SerializeField] private ChangeTypeBlock changeTypeBlockPrefab;
        [SerializeField] private MoveDirBlock moveDirBlockPrefab;

        private RuneStock runeStock;

        public static RuneListBlock inst;

        protected override void Awake()
        {
            base.Awake();
            runeStock = FindObjectOfType<RuneStock>();
            //Debug.Log("runelistblock");
            runeStock.OnRuneUpdate += RuneUpdate;
            inst = this;
        }

        public void RuneUpdate(RuneType runeType, int runeCount, bool adding)
        {
            if (runeCount == 0)
            {
                // Remove rune from list
                Blocks.RemoveAll(block => block.IsRune && block.RuneType == runeType);
            }
            else if (runeCount == 1 && adding)
            {
                // Add rune to list
                RuneBlock runeBlock = null;
                if (runeType.type == RuneType.Type.Entity)
                {
                    runeBlock = Instantiate(entityBlockPrefab);
                }
                else if (runeType.type == RuneType.Type.Adjective)
                {
                    runeBlock = Instantiate(changeTypeBlockPrefab);
                }
                else if (runeType.type == RuneType.Type.Direction)
                {
                    runeBlock = Instantiate(moveDirBlockPrefab);
                }
                runeBlock.RuneType = runeType;
                BlockListRoot.inst.OnUse += runeBlock.SetUsed;
                BlockListRoot.inst.SetAllUnused += runeBlock.SetUnused;
                runeBlock.transform.SetParent(blockListPanel.transform);
                runeBlock.FindWhereYouBelong();
                Blocks.Add(runeBlock);
                runeBlock.SetRuneCount(runeCount);
            }
            else
            {
                // Update rune count
                var runeBlock = (RuneBlock) Blocks.Find(block => block.IsRune && block.RuneType == runeType);
                if(runeBlock != null)
                    runeBlock.SetRuneCount(runeCount);
            }
            //Debug.Log(runeCount);
        }

        public new void OnDrop(PointerEventData eventData)
        {
            Block draggedBlock = CodeUIElement.Instance.DraggedBlock;
            base.OnDrop(eventData);
            if(draggedBlock != null)
            {
                if(draggedBlock is RuneBlock)
                {
                    Block runeBlockInList = Blocks.Find(block => block.IsRune && block.RuneType == draggedBlock.RuneType);
                    if(runeBlockInList != null || true)
                    {
                        Destroy(draggedBlock.gameObject);
                    }

                }
                else
                    Destroy(draggedBlock.gameObject);
            }
        }
    }
}