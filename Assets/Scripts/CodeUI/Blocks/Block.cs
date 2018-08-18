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

        [NonSerialized] public bool IsInSlot = false;

        private GameObject placeHolder = null;

        private Vector2 dragStartPosOffset;

        [NonSerialized] public RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private LayoutElement layoutElement;

        protected ICommandManager commandMgr;

        public bool isMovable = true;

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            layoutElement = GetComponent<LayoutElement>();
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
            OriginalParent = placeHolder.transform.parent;
            OriginalPosition = placeHolder.transform.position;
            OriginalSiblingIndex = placeHolder.transform.GetSiblingIndex();
            Destroy(placeHolder);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!isMovable) return;
            dragStartPosOffset = new Vector2(transform.position.x, transform.position.y) - eventData.position;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!isMovable) return;
            placeHolder = new GameObject();
            var placeHolderTransform = placeHolder.AddComponent<RectTransform>();
            placeHolderTransform.SetParent(transform.parent);
            placeHolderTransform.SetSiblingIndex(transform.GetSiblingIndex());
            placeHolderTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.width);
            placeHolderTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.rect.height);

            OriginalPosition = transform.position;
            OriginalParent = transform.parent;
            OriginalSiblingIndex = transform.GetSiblingIndex();

            IsInSlot = false;

            transform.SetParent(CodeUIElement.Instance.transform);
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isMovable) return;
            transform.position = eventData.position + dragStartPosOffset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isMovable) return;
            Destroy(placeHolder);
            transform.position = OriginalPosition;
            transform.SetParent(OriginalParent);
            transform.SetSiblingIndex(OriginalSiblingIndex);
            canvasGroup.blocksRaycasts = true;
        }
    }
}