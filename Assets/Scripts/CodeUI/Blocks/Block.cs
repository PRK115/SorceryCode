using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeUI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Block : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Vector3 OriginalPosition;
        public Transform OriginalParent;
        public bool IsInSlot;

        public RectTransform rectTransform;
        private CanvasGroup canvasGroup;

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        protected virtual void Start()
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OriginalPosition = transform.position;
            OriginalParent = transform.parent;
            transform.SetParent(CodeUIElement.Instance.transform);
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.position = OriginalPosition;
            transform.SetParent(OriginalParent);
            canvasGroup.blocksRaycasts = true;
        }
    }
}