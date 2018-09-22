using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeUI;

public class BlockSpawn : Block, IDragHandler, IEndDragHandler
{
    protected Block spawnedBlock;
    public new void OnDrag(PointerEventData eventData)
    {
        spawnedBlock.OnDrag(eventData);
    }

    public new void OnEndDrag(PointerEventData eventData)
    {
        spawnedBlock.OnEndDrag(eventData);
    }

}
