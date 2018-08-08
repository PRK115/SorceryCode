using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeUI
{
    public class Block : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public int LinePos;
        public int LineHeight;
        public int Indent;

        public Vector3 OriginalPosition;
        public bool IsInSlot;

        public void OnBeginDrag(PointerEventData eventData)
        {
            OriginalPosition = transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.position = OriginalPosition;
        }
    }
}