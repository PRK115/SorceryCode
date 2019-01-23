using UnityEngine.EventSystems;
using System;

namespace CodeUI
{
    public class CommandListBlock : ScopedBlock, IDropHandler
    {
        public static CommandListBlock Inst;
        private new void Awake()
        {
            base.Awake();
            Inst = this;
        }
        public event Action OnCast;
        protected override bool IsBlockValid(Block block) => !block.IsRune;
        public new void OnDrop(PointerEventData eventData)
        {
            Block draggedBlock = CodeUIElement.Instance.DraggedBlock;
            base.OnDrop(eventData);
            if(draggedBlock != null)
                Destroy(draggedBlock.gameObject);
        }
    }
}