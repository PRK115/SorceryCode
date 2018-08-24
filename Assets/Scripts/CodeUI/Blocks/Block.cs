using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeUI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Block : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [NonSerialized] public Vector3 OriginalPosition;
        [NonSerialized] public Transform OriginalParent;
        [NonSerialized] public int OriginalSiblingIndex;
        [NonSerialized] public float OriginalWidth;
        [NonSerialized] public float OriginalHeight;

        [NonSerialized] public bool IsInSlot = false;

        private GameObject placeHolder = null;

        private Vector2 dragStartPosOffset;

        [NonSerialized] public RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        [NonSerialized] public LayoutElement layoutElement;

        protected ICommandManager commandMgr;

        public bool IsMovable = true;
        public ExprSlot ContainedSlot = null;
        public ScopedBlock ContainedScopedBlock = null;
        public int Depth = 0;
        public bool IsRune = true;

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            layoutElement = GetComponent<LayoutElement>();

            OriginalWidth = rectTransform.rect.width;
            OriginalHeight = rectTransform.rect.height;

            // Find contained slot or scoped block
            Transform parent = transform.parent;
            while (parent != null)
            {
                ScopedBlock scopedBlock = parent.GetComponent<ScopedBlock>();
                if (scopedBlock != null)
                {
                    ContainedScopedBlock = scopedBlock;
                    break;
                }
                ExprSlot exprSlot = parent.GetComponent<ExprSlot>();
                if (exprSlot != null)
                {
                    ContainedSlot = exprSlot;
                    break;
                }
                parent = parent.parent;
            }

            // Update depth of this block
            if (ContainedScopedBlock != null)
                Depth = ContainedScopedBlock.Depth + 1;
        }

        protected virtual void Start()
        {
            if (CommandManager.Inst != null)
                commandMgr = CommandManager.Inst;
            else
                commandMgr = DummyCommandManager.Inst;
        }

        public void SetPlaceholderBlock(Transform parent, Vector3 position)
        {
            if (placeHolder != null)
            {
                placeHolder.transform.position = position;
                if (placeHolder.transform.parent != parent)
                {
                    placeHolder.transform.SetParent(parent);
                }

                int newSiblingIndex = parent.childCount;

                for (int i = 0; i < parent.childCount; ++i)
                {
                    if (position.y > parent.GetChild(i).position.y)
                    {
                        newSiblingIndex = i;
                        if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
                            newSiblingIndex--;
                        break;
                    }
                }

                placeHolder.transform.SetSiblingIndex(newSiblingIndex);
            }
        }

        public void SetRealBlock()
        {
            if (placeHolder != null)
            {
                OriginalParent = placeHolder.transform.parent;
                OriginalPosition = placeHolder.transform.position;
                OriginalSiblingIndex = placeHolder.transform.GetSiblingIndex();
                Destroy(placeHolder);

                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(0, 1);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, OriginalWidth);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, OriginalHeight);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsMovable) return;
            dragStartPosOffset = new Vector2(transform.position.x, transform.position.y) - eventData.position;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!IsMovable) return;

            CodeUIElement.Instance.DraggedBlock = this;

            placeHolder = new GameObject();
            var placeHolderTransform = placeHolder.AddComponent<RectTransform>();
            placeHolderTransform.SetParent(transform.parent);
            placeHolderTransform.SetSiblingIndex(transform.GetSiblingIndex());
            placeHolderTransform.anchorMin = new Vector2(0, 1);
            placeHolderTransform.anchorMax = new Vector2(0, 1);
            LayoutElement layoutElement = placeHolder.AddComponent<LayoutElement>();
            layoutElement.minWidth = OriginalWidth;
            layoutElement.minHeight = OriginalHeight;

            OriginalPosition = transform.position;
            OriginalParent = transform.parent;
            OriginalSiblingIndex = transform.GetSiblingIndex();

            IsInSlot = false;

            transform.SetParent(CodeUIElement.Instance.transform);
            canvasGroup.blocksRaycasts = false;

            if (ContainedSlot != null)
            {
                ContainedSlot.Filled = false;
                ContainedSlot = null;
            }
            if (ContainedScopedBlock != null)
            {
                ContainedScopedBlock.UpdateBlocks();
                ContainedScopedBlock = null;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!IsMovable) return;
            transform.position = eventData.position + dragStartPosOffset;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (!IsMovable) return;

            CodeUIElement.Instance.DraggedBlock = null;

            Destroy(placeHolder);
            transform.position = OriginalPosition;
            transform.SetParent(OriginalParent);
            transform.SetSiblingIndex(OriginalSiblingIndex);
            canvasGroup.blocksRaycasts = true;

            if (ContainedScopedBlock != null)
            {
                ContainedScopedBlock.UpdateBlocks();
            }
        }
    }
}
