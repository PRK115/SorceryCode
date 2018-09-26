using UnityEngine.EventSystems;

namespace CodeUI
{
    public class CommandListBlock : ScopedBlock, IDropHandler
    {
        protected override bool IsBlockValid(Block block) => !block.IsRune;
        public new void OnDrop(PointerEventData eventData)
        {
            Block draggedBlock = CodeUIElement.Instance.DraggedBlock;
            base.OnDrop(eventData);
            Destroy(draggedBlock.gameObject);
        }
    }
}