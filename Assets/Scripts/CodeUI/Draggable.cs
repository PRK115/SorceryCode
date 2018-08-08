using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Thanks to https://github.com/Xander93/unity3d-draganddrop

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentToReturnTo = null;
    [HideInInspector] public Transform placeHolderParent = null;

    private GameObject placeHolder = null;

    private LayoutElement layoutElement;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        layoutElement = GetComponent<LayoutElement>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        placeHolder = new GameObject();
        placeHolder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeHolder.AddComponent<LayoutElement>();
        le.preferredWidth = this.layoutElement.preferredWidth;
        le.preferredHeight = this.layoutElement.preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        if (placeHolder.transform.parent != placeHolderParent)
        {
            placeHolder.transform.SetParent(placeHolderParent);
        }

        int newSiblingIndex = placeHolderParent.childCount;

        for (int i = 0; i < placeHolderParent.childCount; i++)
        {
            if (transform.position.x < placeHolderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;
                if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
                {
                    newSiblingIndex--;
                }

                break;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentToReturnTo);
        this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
        this.canvasGroup.blocksRaycasts = true;
        Destroy(placeHolder);
    }
	
}
